using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ScanResourcesLambda
{
    public class Function
    {
        Dictionary<string, string> sitesToMonitor = new Dictionary<string, string>();

        public async Task<Dictionary<string, string>> FunctionHandler(string input, ILambdaContext context)
        {
            sitesToMonitor.Add("https://samueleresca.net/", "");
            sitesToMonitor.Add("http://techflask.com/", "");
            sitesToMonitor.Add("http://www.contoso.com", "");

            Stopwatch sw = new Stopwatch();

            sw.Start();
            //foreach (var webSite in sitesToMonitor.Keys.ToArray())
            //    await getSiteStatus(webSite);

            for (int i = 0; i < 1000; i++)
            {
                await getSiteStatus("http://www.contoso.com");
            }

            sw.Stop();
            Console.WriteLine("-------  ELAPSED TIME: {0} sec ------- ", sw.ElapsedMilliseconds / 1000);
            return sitesToMonitor;
        }

        private async Task getSiteStatus(string webSite)
        {
            var client = new HttpClient();
            var result = await client.GetAsync(webSite);
            sitesToMonitor[webSite] = result.StatusCode.ToString();
        }
    }
}
