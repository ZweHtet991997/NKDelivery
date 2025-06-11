using DeliverySystemAPI.Controllers.Account;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Middleware
{
    [EnableCors]
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private IConfiguration Configuration;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];
            var requestPath = context.Request.Path;
            if(requestPath== "/api/account/login" || requestPath == "/swagger" || requestPath== "/favicon.ico")
            {
                await _next.Invoke(context);
            }
            else
            {
                if (authHeader != null && authHeader.StartsWith("Bearer"))
                {
                    string[] header_and_token = authHeader.Split(' ');
                    string header = header_and_token[0];
                    string token = header_and_token[1];
                    RegisterLoginController accountController = new RegisterLoginController(Configuration);
                    ObjectResult objectResult = (ObjectResult)accountController.Validate_JWT_Toke(context);
                    if (objectResult.StatusCode == 200)
                    {
                        if (requestPath == "/api/jwt")
                        {
                            context.Response.StatusCode = 200;
                            return; 
                        }
                        else
                        {
                            await _next.Invoke(context);
                        }
                        
                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }
                }
                //if authHeader null or not Bearer
                else
                {
                    context.Response.StatusCode = 401;
                    return;
                }
            }
        }
    }
}

