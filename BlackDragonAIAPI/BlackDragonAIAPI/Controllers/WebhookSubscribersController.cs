using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackDragonAIAPI.Models;
using BlackDragonAIAPI.StorageHandlers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BlackDragonAIAPI.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/webhook")]
    [ApiController]
    public class WebhookSubscribersController : ControllerBase
    {
        private IWebhookSubscriberService _db { get; set; }

        public WebhookSubscribersController(IWebhookSubscriberService db)
        {
            this._db = db;
        }

        [HttpPost]
        public async Task<ActionResult<WebhookSubscriber>> CreateWebhookSubscriber(WebhookSubscription webhookSubscription)
        {
            if ((await this._db.GetWebhookSubscribers()).Any(dbWs => dbWs.Uri.Equals(webhookSubscription.Url)))
                return BadRequest(new BadRequestError("A webhook subscriber has already been created for this uri"));
            return await AddWebhookSubscriberToDatabase(webhookSubscription);
        }

        /// <summary>
        /// Creates a webhook subscriber in an idempotent manner. Will always ensure resource exists if the request is valid
        /// </summary>
        [HttpPost("idempotent")]
        public async Task<ActionResult<WebhookSubscriber>> CreateWebhookSubscriberIdempotent(WebhookSubscription webhookSubscription) =>
            (await this._db.GetWebhookSubscribers()).FirstOrDefault(dbWs => dbWs.Uri.Equals(webhookSubscription.Url)) 
            ?? await AddWebhookSubscriberToDatabase(webhookSubscription);

        private async Task<WebhookSubscriber> AddWebhookSubscriberToDatabase(WebhookSubscription webhookSubscription)
        {
            var ws = new WebhookSubscriber()
            {
                Guid = Guid.NewGuid(),
                Uri = webhookSubscription.Url
            };
            return await this._db.CreateWebhookSubscriber(ws);
        }

        private string GetClientAddress() =>
            HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
//            HttpContext.Connection.RemoteIpAddress.ToString();
    }
}
