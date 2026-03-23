using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackDragonAIAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BlackDragonAIAPI.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/auth")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private const string BaseAuthUrl = "https://id.twitch.tv/oauth2/authorize";
        private const string RedirectUrl = "https://blackdragonai.nl/api/auth/authorized";
        private const string ClientId = "4vuzwgaosquwq1298vf94452k5s2iw";

        private readonly WebhookManager _webhookManager;

        public AuthController(WebhookManager webhookManager)
        {
            _webhookManager = webhookManager;
        }
    
        [HttpGet]
        public async Task<ActionResult> GetAuthUrl()
        {
            if (!IsAuthorized()) return Unauthorized(new UnauthorizedError());

            var res = new TwitchAuthInformation()
            {
                Url = GetAuthorizationUrl()
            };
            return Ok(res);
        }

        [HttpPost("authorized")]
        [HttpGet("authorized")]
        public async Task<ActionResult> ProcessTwitchAuthResult()
        {
            var authToken = HttpContext.Request.Query["code"];
            Console.WriteLine($"Received auth token: {authToken}");
            _webhookManager.SendUpdateNotification("/authorized",authToken.First());
            return Ok("Authorized! You can safely leave this page.");
        }
    
        private string GetAuthorizationUrl() =>
            $"{BaseAuthUrl}?client_id={ClientId}&response_type=code&scope={GetAuthScopes().Aggregate((scope1, scope2) => $"{scope1}+{scope2}")}&redirect_uri={RedirectUrl}";
        
        private IEnumerable<string> GetAuthScopes() => new string[]{
            "analytics:read:extensions", "analytics:read:games", "bits:read", "channel:edit:commercial", 
            "channel:read:hype_train", "channel:read:subscriptions", "clips:edit", "user:edit", "user:edit:broadcast",
            "user:edit:follows", "user:read:broadcast", "user:read:email", "moderator:read:followers",
            "moderator:manage:banned_users", "user:bot"
        };
    
    
        private bool IsAuthorized() => HttpContext.MeetsAuthorizationLevel(EAuthorizationLevel.ADMIN);
    }   
}