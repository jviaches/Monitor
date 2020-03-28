using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ScanResources
{
    public class Function
    {
        /// <summary>
        /// The main entry point for the custom runtime.
        /// </summary>
        /// <param name="args"></param>
        private static async Task Main(string[] args)
        {
            Func<string, ILambdaContext, string> func = FunctionHandler;
            using(var handlerWrapper = HandlerWrapper.GetHandlerWrapper(func, new JsonSerializer()))
            using(var bootstrap = new LambdaBootstrap(handlerWrapper))
            {
                await bootstrap.RunAsync();
            }
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        ///
        /// To use this handler to respond to an AWS event, reference the appropriate package from 
        /// https://github.com/aws/aws-lambda-dotnet#events
        /// and change the string input parameter to the desired event type.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string FunctionHandler(string input, ILambdaContext context)
        {
            try
            {
                Uri uri = new Uri("http://www.isra.com/");
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                var statusCode = (int)myHttpWebResponse.StatusCode;

                myHttpWebResponse.Close();

                //var newResourceHistory = new ResourceHistory()
                //{
                //    MonitorTypeId = 1,  // TODO: adjust to real type
                //    ResourceId = 1,     // TODO: adjust to real resourceId
                //    RequestDate = DateTime.UtcNow,
                //    ResponseDate = DateTime.UtcNow,
                //    Result = statusCode.ToString()
                //};

                //_resourceHistoryService.Add(newResourceHistory);

                return statusCode + "";
            }
            catch (WebException e)
            {
                return input?.ToUpper();
            }
            catch (Exception e)
            {
                return input?.ToUpper();
            }
            
        }
    }
}
