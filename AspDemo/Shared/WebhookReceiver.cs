
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shared
{
    // note route is made of randomstring to make sure it is not guessable 
    // route would be http://[host.domain]/[base-prefix]/receiving-6FB9095B71E94B62904FF9F7CD693AC1
    [Route("receiving-6FB9095B71E94B62904FF9F7CD693AC1")]
    [ApiController]
    public class WebhookReceiver
    {
        private const string SubscriptionConfirmation = "SubscriptionConfirmation";
        private readonly HttpClient _httpClient;
        private readonly IRevocationMessageHandler messageHandler;

        public WebhookReceiver(ILogger<WebhookReceiver> logger, HttpClient httpClient, IRevocationMessageHandler messageHandler)
        {
            _logger = logger;
            _httpClient = httpClient;
            this.messageHandler = messageHandler;
        }

        private readonly ILogger _logger;

        // POST base/receiving-6FB9095B71E94B62904FF9F7CD693AC1
        [HttpPost]
        [AllowAnonymous]
        public async Task Post([FromBody] string json)
        {
            
            var sm = Amazon.SimpleNotificationService.Util.Message.ParseMessage(json);
            if (sm.Type.Equals(SubscriptionConfirmation)) //for confirmation
            {
                _logger.LogInformation("Received Confirm subscription request");
                if (!string.IsNullOrEmpty(sm.SubscribeURL))
                {
                    var uri = new Uri(sm.SubscribeURL);
                    _logger.LogInformation("uri:" + uri.ToString());
                    var baseUrl = uri.GetLeftPart(System.UriPartial.Authority);
                    var resource = sm.SubscribeURL.Replace(baseUrl, "");
                    await Task.Delay(1); // replace this with await _httpClient.GetAsync(resource);
                    return ; 
                }
            }
            else // For processing of messages
            {
                _logger.LogInformation("Message received from SNS:" + sm.TopicArn);
                dynamic message = JsonConvert.DeserializeObject(sm.MessageText);
                _logger.LogInformation($"EventTime : {message.detail.eventTime}") ;
                _logger.LogInformation($"EventName : {message.detail.eventName}");
                _logger.LogInformation($"RequestParams : {message.detail.requestParameters}");
                _logger.LogInformation($"ResponseParams : {message.detail.responseElements}");
                _logger.LogInformation($"RequestID : {message.detail.requestID}");

                messageHandler.HandleMessage(message);
            }
            return ;
        }

    }
}
