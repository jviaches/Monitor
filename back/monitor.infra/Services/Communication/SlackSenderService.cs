using monitor.infra.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace monitor.infra.Services.Communication
{
    public class SlackSenderService : ISlackSenderService
    {
        public void SendAbnormalStatus(string channel, string url, string statusCodeResult)
        {
            var data = new NameValueCollection();
            data["token"] = "xoxb-698276572148-1116340862005-RgWysGGhgdrsOQZrGMzD9fIn";
            data["channel"] = channel;
            data["as_user"] = "true";           // to send this message as the user who owns the token, false by default
            data["text"] = "Monitored resource returned abnormal status:";

            //var formattedAttachement = "[{\"fallback\":\"error to display\", \"text\":\"Status code:{0} - Url:{1}\"}]";

            var attachments = new SlackAttachment[]
            {
                    //new SlackAttachment
                    //{
                    //    fallback = "this did not work",
                    //    text = "This is attachment 1",
                    //    color = "good"
                    //},

                    new SlackAttachment
                    {
                        fallback = "Error to attach failed resource! Please contact us!",
                        text = $"Status code: {statusCodeResult} - Url: {url}",
                        color = "danger",
                        image_url = "https://d3mbies4lx0e41.cloudfront.net/assets/img/brand-logo-color.png"
                    }
            };

            var content = JsonConvert.SerializeObject(attachments);
            data["attachments"] = content;

            var client = new WebClient();
            var response = client.UploadValues("https://slack.com/api/chat.postMessage", "POST", data);
            string responseInString = Encoding.UTF8.GetString(response);
        }
    }

    // a slack message attachment
    public class SlackAttachment
    {
        public string fallback { get; set; }
        public string text { get; set; }
        public string image_url { get; set; }
        public string color { get; set; }
    }
}
