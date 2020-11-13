using AuthServer.PasswordHasher;
using AuthServer.Utils;
using AuthServer.Models;
using AuthServer.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AuthServer
{
    public class Startup
    {
        #region Members

        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        private string _databaseConnectionString;

        #endregion

        #region Constructor

        public Startup(IConfiguration configuration, ILogger<Startup> logger, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _logger = logger;
            LoggingUtils.LoggerFactory = loggerFactory;
        }

        #endregion

        #region Logic

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddScoped<IPasswordHasher<User>, BCryptPasswordHasher<User>>();
            services.AddMvc(option => option.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            _databaseConnectionString = _configuration.GetConnectionString("DB");
            SetupConfig(services);

            var key = Convert.FromBase64String(_configuration["AppSettings:Secret"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var userId = context.Principal.Identity.Name;

                        if (string.IsNullOrEmpty(userId))
                            context.Fail("Unauthorized");
                        else
                        {
                            var user = userService.GetUserById(userId);
                            if (user == null)
                                context.Fail("Unauthorized");
                        }

                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    // Time drift compensation
                    ClockSkew = TimeSpan.FromMinutes(5),
                    // Issuer key stuff
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    RequireSignedTokens = true,
                    // Expiration
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    // Validation stuff, leave false for now
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddScoped<IUserService, UserService>();
        }

        private void SetupConfig(IServiceCollection services)
        {
            var config = GetConfigFromDB();

            _logger.LogInformation("Setting up configuration...");

            // Get encryption key
            _configuration["AppSettings:Secret"] = config.Configuration.GetValueOrDefault("encryption_key", string.Empty);

            var appSettingsSection = _configuration.GetSection("AppSettings");
            appSettingsSection["AppSettings:Secret"] = _configuration["AppSettings:Secret"];

            var appSettings = appSettingsSection.Get<AppSettings>();
            appSettings.Secret = appSettingsSection["AppSettings:Secret"];

            // Propagate the AppSettings throughout the API
            services.Configure<AppSettings>(_configuration.GetSection("AppSettings"));

            _logger.LogInformation("Configuration setup complete");
        }

        private Config GetConfigFromDB()
        {
            _logger.LogInformation("Getting configuration from DB...");

            var client = new MongoClient(_databaseConnectionString);
            var database = client.GetDatabase("Auth");
            var configurations = database.GetCollection<Config>("Configuration");
            var apiConfig = configurations.Find(configuration => configuration.Id == "auth_configuration").FirstOrDefault();

            if (apiConfig == null)
                return GenerateDefaultConfig(configurations);

            _logger.LogInformation("Configuration found");

            return apiConfig;
        }

        public Config GenerateDefaultConfig(IMongoCollection<Config> configurations)
        {
            _logger.LogInformation("No configuration found, generating one...");

            // Generate new encryption key
            var defaultConfig = new Config
            {
                Id = "auth_configuration",
                Configuration = new Dictionary<string, string> { { "encryption_key", GenerateJwtSecret() } }
            };


            configurations.InsertOne(defaultConfig);

            _logger.LogInformation("Configuration generated");
            return defaultConfig;
        }

        private static string GenerateJwtSecret()
        {
            using var hmac = new HMACSHA256();
            var key = Convert.ToBase64String(hmac.Key);
            return key;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseMvc();
        }

        #endregion
    }
}
