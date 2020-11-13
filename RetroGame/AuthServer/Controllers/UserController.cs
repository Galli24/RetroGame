using AuthServer.Dtos;
using AuthServer.Exceptions;
using AuthServer.Models;
using AuthServer.Services;
using AuthServer.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : Controller
    {
        #region Members

        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        #endregion

        #region Constructor

        public UserController(IUserService userService, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        #endregion

        #region Logic

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserDto userDto)
        {
            var user = _userService.Authenticate(userDto);
            if (user == null)
                return BadRequest(new { error = "Wrong username or passsword" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                username = user.Username,
                email = user.Email,
                token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            try
            {
                _userService.Create(user, userDto.Password);
                return Ok();
            }
            catch (RegisterException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion

    }
}