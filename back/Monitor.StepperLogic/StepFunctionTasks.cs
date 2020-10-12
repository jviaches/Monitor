using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Lambda;
using Amazon.Lambda.Core;
using Amazon.Lambda.Model;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Monitor.StepperLogic.StateModels;
using monitor_core.Dto;
using monitor_infra;
using monitor_infra.Services;
using monitor_infra.Services.Interfaces;
using Newtonsoft.Json;
using Environment = System.Environment;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Monitor.StepperLogic
{
    public class StepFunctionTasks
    {
        private static AmazonLambdaClient lambdaClient;
        private IResourceService _resourceService;
        private IUserService _userService;
        private IMonitorItemService _monitorItemService;
        private IEmailSenderService _emailSenderService;

        public StepFunctionTasks()
        {
            var awsOptions = new AWSOptions()
            {
                Credentials = new BasicAWSCredentials("AKIATCF5UENNOJWDLUVU", "YUy4AL06ZBfHpNuRWn2F2TE04vJ/UUeZz+fmN5+i"),
                Region = RegionEndpoint.USEast2
            };

            lambdaClient = new AmazonLambdaClient(awsOptions.Credentials, awsOptions.Region);

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IResourceService, ResourceService>();
            serviceCollection.AddSingleton<IUserService, UserService>();
            serviceCollection.AddSingleton<IMonitorItemService, MonitorItemService>();
            serviceCollection.AddSingleton<IEmailSenderService, EmailSenderService>();

            //var DBHostName = Environment.GetEnvironmentVariable("DBHostName");
            //var DBName = Environment.GetEnvironmentVariable("DBName");
            //var DBUserName = Environment.GetEnvironmentVariable("DBUserName");
            //var DBPassword = Environment.GetEnvironmentVariable("DBPassword");
            //var DBPort = Environment.GetEnvironmentVariable("DBPort");

            serviceCollection.AddDbContext<AppDbContext>(options =>
            {
                // options.UseNpgsql($"Host={DBHostName};Port={DBPort};Username={DBUserName};Password={DBPassword};Database={DBName};", b => b.MigrationsAssembly("Monitor.Infra"));
                var connection = Environment.GetEnvironmentVariable("DefaultConnection");
                options.UseNpgsql(connection, b => b.MigrationsAssembly("monitor.infra"));
            });

            var serviceProvider = serviceCollection.BuildServiceProvider();

            _resourceService = serviceProvider.GetService<IResourceService>();
            _userService = serviceProvider.GetService<IUserService>();
            _monitorItemService = serviceProvider.GetService<IMonitorItemService>();
            _emailSenderService = serviceProvider.GetService<IEmailSenderService>();
        }

        /// <summary>
        /// Lamda function responsible to retrieve web-sites stored in DB by specified periodicity
        /// </summary>
        /// <param name="model">Periodicity</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public ResourceScanResultModel RetrieveRecords(RetrieveRecordModel model, ILambdaContext context)
        {
            context.Logger.LogLine($"---------- RetrieveRecords  from DB with Periodicity [{(model.Periodicity)} milisec] ----------");

            var monitorItems = _resourceService.GetMonitoredItemsByPeriodicity(model.Periodicity);
            var urlList = monitorItems.Select(record => new ResourceModel(record.Key.MonitorItem.Id, record.Key.Url, string.Empty, record.Key.CommunicationChanel, record.Key.UserId)).ToList();
            var resourceToScanModel = new ResourceScanResultModel() { ResourcesStatuses = urlList };

            context.Logger.LogLine($"Retrieved {resourceToScanModel.ResourcesStatuses.Count} resources for scan with specified periodicity");

            return resourceToScanModel;
        }

        public async Task<ResourceScanResultModel> MonitorRecords(ResourceScanResultModel model, ILambdaContext context)
        {
            context.Logger.LogLine($"---------- Start MonitorRecords [{model.ResourcesStatuses.Count} resources] ----------");
            var resourceScanResultModel = new ResourceScanResultModel();

            foreach (var item in model.ResourcesStatuses)
            {
                var lamdaRequest = new InvokeRequest
                {
                    //Todo: call lamda depending on environment
                    FunctionName = "ResourceStepperStaging-GetResourceStatusTask-WKMVBYB2Q548",
                    InvocationType = Amazon.Lambda.InvocationType.RequestResponse,
                    Payload = JsonConvert.SerializeObject(item)
                };

                try
                {
                    var lamdaResponse = await lambdaClient.InvokeAsync(lamdaRequest);
                    var stReader = new StreamReader(lamdaResponse.Payload);

                    var jReader = new JsonTextReader(stReader);

                    var jSerializer = new JsonSerializer();
                    var statusCode = jSerializer.Deserialize(jReader);

                    resourceScanResultModel.ResourcesStatuses.Add(new ResourceModel(item.ResourceId, item.Url, statusCode.ToString(), item.CommunicationChanel, item.UserId));
                }
                catch (Exception e)
                {
                    context.Logger.LogLine($"Exception Occured: {e}");
                    resourceScanResultModel.ResourcesStatuses.Add(new ResourceModel(item.ResourceId, item.Url, "000", item.CommunicationChanel, item.UserId));
                }
            }

            context.Logger.LogLine($"---------- End MonitorRecords ----------");

            return resourceScanResultModel;
        }

        public ResourceScanResultModel ProcessRecords(ResourceScanResultModel model, ILambdaContext context)
        {
            context.Logger.LogLine($"---------- Start ProcessRecords ----------");

            //TODO: make 1 DB batch operation
            //TODO: handle exception

            foreach (var item in model.ResourcesStatuses)
            {
                var addResourceHistoryDto = new AddMonitorHistoryDto()
                {
                    MonitorItemId = item.ResourceId,
                    ScanDate = DateTime.UtcNow,
                    Result = string.IsNullOrEmpty(item.StatusCode) ? "000" : item.StatusCode
                };

                _monitorItemService.AddHistoryItem(addResourceHistoryDto);
            }

            context.Logger.LogLine($"---------- End ProcessRecords ----------");

            return model;
        }

        public async Task<string> GetResourceStatus(ResourceModel resource, ILambdaContext context)
        {
            //TODO: improve by checking existing URL + Status code
            context.Logger.LogLine($"---------- GetResourceStatus Invocation: [{resource.Url}] ----------");

            try
            {
                var client = new HttpClient();
                var result = await client.GetAsync(resource.Url);
                var statusCodeResult = ((int)result.StatusCode).ToString();
                context.Logger.LogLine($"Status code for URL: [{resource.Url}] = [{statusCodeResult}]");

                if (statusCodeResult.StartsWith("3") || statusCodeResult.StartsWith("4") || statusCodeResult.StartsWith("5"))
                {
                    var email = _userService.GetById(resource.UserId).Email;
                    _emailSenderService.SendAbnormalStatus(email, resource.Url, statusCodeResult);
                }

                return statusCodeResult;
            }
            catch (Exception e)
            {
                context.Logger.LogLine($"Exception occured: [{e}] ");
                //await sendEmail(resource, "ERROR", context); //TODO: send to support@projscope.com
                _emailSenderService.SendServicesException("Exception occured: [{e}]");
                return string.Empty;
            }
        }
    }
}
