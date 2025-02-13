using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackDragonAIAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BlackDragonAIAPI.Controllers;

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
        _webhookManager.SendUpdateNotification("/authorized",authToken.First());
        return Ok("Processing authorization");
    }
    
    private string GetAuthorizationUrl() =>
        $"{BaseAuthUrl}?client_id={ClientId}&redirect_uri=${RedirectUrl}&response_type=code&scope={GetAuthScopes().Aggregate((scope1, scope2) => $"{scope1}+{scope2}")}";
        
    private IEnumerable<string> GetAuthScopes() => new[] {
        "analytics:read:extensions", "analytics:read:games", "bits:read", "channel:edit:commercial", 
        "channel:read:hype_train", "channel:read:subscriptions", "clips:edit", "user:edit", "user:edit:broadcast",
        "user:edit:follows", "user:read:broadcast", "user:read:email"
    };
    
    
    private bool IsAuthorized() => HttpContext.MeetsAuthorizationLevel(EAuthorizationLevel.ADMIN);
}