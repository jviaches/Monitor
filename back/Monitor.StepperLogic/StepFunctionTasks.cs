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
using monitor_infra.Repositories;
using monitor_infra.Repositories.Interfaces;
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
            
            // Repositories
            serviceCollection.AddScoped<ICommunicationChanelRepository, CommunicationChanelRepository>();
            serviceCollection.AddScoped<IMonitorHistoryRepository, MonitorHistoryRepository>();
            serviceCollection.AddScoped<IMonitorItemRepository, MonitorItemRepository>();
            serviceCollection.AddScoped<IResourceRepository, ResourceRepository>();
            serviceCollection.AddScoped<IUserActionRepository, UserActionRepository>();

            // Services
            serviceCollection.AddScoped<IUserActionService, UserActionService>();
            serviceCollection.AddScoped<IResourceService, ResourceService>();
            serviceCollection.AddScoped<IMonitorItemService, MonitorItemService>();

            // Infra Services
            serviceCollection.AddScoped<IEmailSenderService, EmailSenderService>();


            serviceCollection.AddDbContext<AppDbContext>(options =>
            {
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
        public MonitorItemScanResultModel RetrieveRecords(RetrieveRecordModel model, ILambdaContext context)
        {
            context.Logger.LogLine($"---------- RetrieveRecords  from DB with Periodicity [{(model.Periodicity)} milisec] ----------");

            var monitorItems = _resourceService.GetMonitoredItemsByPeriodicity(model.Periodicity);
            var urlList = monitorItems.Select(record => new MonitorItemModel(record.Key.MonitorItem.Id, record.Key.Url, string.Empty, record.Key.CommunicationChanel, record.Key.UserId)).ToList();
            var resourceToScanModel = new MonitorItemScanResultModel() { ResourcesStatuses = urlList };

            context.Logger.LogLine($"Retrieved {resourceToScanModel.ResourcesStatuses.Count} resources for scan with specified periodicity");

            return resourceToScanModel;
        }

        public async Task<MonitorItemScanResultModel> MonitorRecords(MonitorItemScanResultModel model, ILambdaContext context)
        {
            context.Logger.LogLine($"---------- Start MonitorRecords [{model.ResourcesStatuses.Count} resources] ----------");
            var resourceScanResultModel = new MonitorItemScanResultModel();

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

                    resourceScanResultModel.ResourcesStatuses.Add(new MonitorItemModel(item.MonitorId, item.Url, statusCode.ToString(), item.CommunicationChanel, item.UserId));
                }
                catch (Exception e)
                {
                    context.Logger.LogLine($"Exception Occured: {e}");
                    _emailSenderService.SendServicesException($"MonitorStepper::GetResourceStatus - Exception occured: [{e}]");
                    resourceScanResultModel.ResourcesStatuses.Add(new MonitorItemModel(item.MonitorId, item.Url, "000", item.CommunicationChanel, item.UserId));
                }
            }

            context.Logger.LogLine($"---------- End MonitorRecords ----------");

            return resourceScanResultModel;
        }

        public async Task<MonitorItemScanResultModel> ProcessRecords(MonitorItemScanResultModel model, ILambdaContext context)
        {
            context.Logger.LogLine($"---------- Start ProcessRecords ----------");

            //TODO: make 1 DB batch operation
            //TODO: handle exception

            foreach (var item in model.ResourcesStatuses)
            {
                context.Logger.LogLine($"---------- Found: [{item.Url}] ----------");
                var addResourceHistoryDto = new AddMonitorHistoryDto()
                {
                    MonitorItemId = item.MonitorId,
                    ScanDate = DateTime.UtcNow,
                    Result = string.IsNullOrEmpty(item.StatusCode) ? "000" : item.StatusCode
                };

                context.Logger.LogLine($"---------- adding [{item.Url}] to monitor history ----------");
                await _monitorItemService.AddHistoryItem(addResourceHistoryDto);
                context.Logger.LogLine($"---------- Items has been added ----------");
            }

            context.Logger.LogLine($"---------- End ProcessRecords ----------");

            return model;
        }

        public async Task<string> GetResourceStatus(MonitorItemModel resource, ILambdaContext context)
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
                _emailSenderService.SendServicesException($"MonitorStepper::GetResourceStatus - Exception occured: [{e}]");
                return string.Empty;
            }
        }
    }
}
