using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BlackDragonAIAPI.Models;
using BlackDragonAIAPI.StorageHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using TokenContent = BlackDragonAIAPI.Models.TokenContent;

namespace BlackDragonAIAPI
{
    public class AuthenticationMiddleware
    {
        private readonly IEnumerable<string> _routeExemptions = new string[]{"api/users/register", "api/users/login", "api/streamplannings"};
        private string _authorizedUrlPath = "api/auth/authorized";
        private readonly RequestDelegate _next;
        private readonly string _secret;

        public AuthenticationMiddleware(RequestDelegate next, IConfiguration config)
        {
            _secret = config["AuthConfig:Secret"];
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (this._routeExemptions.Any(route => context.Request.GetEncodedUrl().EndsWith(route)) ||
                context.Request.GetEncodedUrl().Contains(_authorizedUrlPath))
            {
                await this._next(context);
                return;
            }

            if (context.Request.Headers.TryGetValue("X-Access-Token", out var token) && IsValidToken(token, this._secret, out var tokenContent))
            {
                context.Items.Add("jwt", tokenContent);
                await this._next(context);
                return;
            }
            else
            {
                context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                context.Response.Headers.Add("Content-Type", new StringValues("application/json"));
                var error = new
                {
                    message = "401 Unauthorized Error. See: https://tools.ietf.org/html/rfc7235#section-3.1",
                    statusCode = (int) HttpStatusCode.Unauthorized
                };
                await context.Response.BodyWriter.WriteAsync(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(error)));
            }
        }

        private static bool IsValidToken(string token, string secret, out TokenContent tokenContent)
        {
            var secretKey = Encoding.ASCII.GetBytes(secret);
            try
            {
                var tokenDecodedString = Jose.JWT.Decode(token, secretKey);
                tokenContent = JsonConvert.DeserializeObject<TokenContent>(tokenDecodedString);
                return true;
            }
            catch (Exception)
            {
                tokenContent = null;
                return false;
            }
        }
    }

    public static class AuthenticationStorageExtensions
    {
        public static bool TryGetAuthLevel(this HttpContext context, out TokenContent tokenContent)
        {
            if(context.Items.TryGetValue("jwt", out var uncastTokenContent))
            {
                tokenContent = uncastTokenContent as TokenContent;
                return true;
            }
            else
            {
                tokenContent = null;
                return false;
            }
        }

        public static bool MeetsAuthorizationLevel(this HttpContext context, EAuthorizationLevel authLevel) =>
            context.TryGetAuthLevel(out var tokenContent) && tokenContent.AuthorizationLevel >= authLevel;
    }
}
