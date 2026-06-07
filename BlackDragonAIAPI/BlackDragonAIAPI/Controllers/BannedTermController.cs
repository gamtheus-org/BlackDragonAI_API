using System.Collections.Generic;
using System.Threading.Tasks;
using BlackDragonAIAPI.Models;
using BlackDragonAIAPI.StorageHandlers;
using Microsoft.AspNetCore.Mvc;

namespace BlackDragonAIAPI.Controllers;

[Route("api/banned-terms")]
[ApiController]
public class BannedTermController : ControllerBase
{
    private readonly BannedTermService _bannedTermService;
    private readonly WebhookManager _webhookManager;

    public BannedTermController(BannedTermService bannedTermService, WebhookManager webhookManager)
    {
        _bannedTermService = bannedTermService;
        _webhookManager = webhookManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<string>>> GetBannedTerms()
    {
        if (!IsAdmin())
        {
            return Unauthorized();
        }

        var bannedTerms = await _bannedTermService.GetBannedTermsAsync();
        return Ok(bannedTerms);
    }

    [HttpPut]
    public async Task<ActionResult> SaveBannedTerms(string[] bannedTerms)
    {
        if (!IsAdmin())
        {
            return Unauthorized();
        }

        var bannedTermsChanges = await _bannedTermService.SaveBannedTermsAsync(bannedTerms);
        if (bannedTermsChanges)
        {
            _webhookManager.SendUpdateNotification("/banned-terms");
        }

        return NoContent();
    }

    private bool IsAdmin() => HttpContext.MeetsAuthorizationLevel(EAuthorizationLevel.ADMIN);
}
