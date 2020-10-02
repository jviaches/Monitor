using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcesLambda.ViewModels.IntegrationSettings
{
    public class IntegrationSettingsResponse
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public bool NotificationEmail { get; set; }
        public bool NotificationSlack { get; set; }
        public string NotificationSlackChannel { get; set; }
    }
}
