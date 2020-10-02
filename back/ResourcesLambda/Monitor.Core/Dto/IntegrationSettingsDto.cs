using System;
using System.Collections.Generic;
using System.Text;

namespace Monitor.Core.Dto
{
    public class IntegrationSettingsDto
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public bool NotificationEmail { get; set; }
        public bool NotificationSlack { get; set; }
        public string NotificationSlackChannel { get; set; }
    }
}
