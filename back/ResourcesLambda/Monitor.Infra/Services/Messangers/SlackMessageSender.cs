using Monitor.Infra.Interfaces.Messangers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Monitor.Infra.Services.Messangers
{
    public class SlackMessageSender : IMessageSender
    {
        public Task SendMessage(string channel, string message)
        {
            // TODO: make it dynamic from user profle
            var _url = "https://slack.com/api/chat.postMessage";
            var _client = new HttpClient();
            var token = "xoxb-698276572148-1116340862005-RgWysGGhgdrsOQZrGMzD9fIn";

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var postObject = new { channel, message };
            var json = JsonSerializer.Serialize(postObject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return _client.PostAsync(_url, content);
        }
    }
}
