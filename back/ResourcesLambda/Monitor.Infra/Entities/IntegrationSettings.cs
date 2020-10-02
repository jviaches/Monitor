using System.ComponentModel.DataAnnotations;

namespace Monitor.Infra.Entities
{
    public class IntegrationSettings : BaseEntity
    {
        [Key]
        public string UserEmail { get; set; }
        public bool NotificationEmail { get; set; }
        
        public bool NotificationSlack { get; set; }
        public string SlackChannelUrl { get; set; }
    }
}
