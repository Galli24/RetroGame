using AuthServer.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace AuthServer.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class GameServerAuthAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public GameServerAuthAttribute()
        { }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var headerKey = HeaderKey.Get();
            if (!context.HttpContext.Request.Headers.ContainsKey("GameServerAuth")
                || context.HttpContext.Request.Headers["GameServerAuth"] != headerKey)
                context.Result = new ForbidResult();
        }
    }
}
