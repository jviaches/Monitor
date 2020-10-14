using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using monitor_infra.Services;
using monitor_infra.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monitor.infra.Attributes
{
    public class JwtTokenRenewAttribute : TypeFilterAttribute
    {
        public JwtTokenRenewAttribute() : base(typeof(JwtTokenRenewAttributeImplementation)) { }

        private class JwtTokenRenewAttributeImplementation : ActionFilterAttribute
        {
            private readonly ITokenService _tokenService;
            public JwtTokenRenewAttributeImplementation(ITokenService tokenService)
            {
                _tokenService = tokenService;
            }

            public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                await next();

                if (context.Filters.Any(filter => filter is SkipJwtTokenAttribute))
                    return;

                if (!context.HttpContext.User.Identity.IsAuthenticated)
                    return;

                var currentToken = await context.HttpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "access_token");
                context.HttpContext.Response.Headers.Remove("Bearer");
                context.HttpContext.Response.Headers.Add("Bearer", _tokenService.RenewToken(currentToken));
            }
        }
    }
}
