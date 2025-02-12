using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BlackDragonAIAPI.StorageHandlers;

namespace BlackDragonAIAPI
{
    public class WebhookManager
    {
        private IWebhookSubscriberService _db;
        private static readonly HttpClient _client = new HttpClient();

        public WebhookManager(IWebhookSubscriberService db)
        {
            this._db = db;
        }

        public async void SendUpdateNotification(string endpoint)
        {
            Console.WriteLine("Starting");
            foreach (var ws in await this._db.GetWebhookSubscribers())
            {
                try
                {
                    Console.WriteLine($"URL: {ws.Uri}");
                    SendWithoutWaiting(new Uri($"{ws.Uri}{endpoint}"));
                }
                catch (Exception)
                {
                    Console.WriteLine($"Webhook error to uri: {ws.Uri}");
                }
            }
        }

        private async void SendWithoutWaiting(Uri uri)
        {
            try
            {
                var context = await _client.PostAsync(uri, new StringContent(""));
                Console.WriteLine("Successful");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Webhook error to uri: {uri.AbsolutePath}");
            }
        }
    }
}
