using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcesLambda.ViewModels.IntegrationSettings
{
    public class IntegrationSettingsUpdateViewModel
    {
        public string UserEmail { get; set; }
        public bool NotificationEmail { get; set; }

        public bool NotificationSlack { get; set; }
        public string SlackChannelUrl { get; set; }
    }
}
