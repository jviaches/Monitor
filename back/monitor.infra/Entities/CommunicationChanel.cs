using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace monitor_infra.Entities
{
    public class CommunicationChanel
    {
        [Key]
        public Guid Id { get; set; }
        public bool NotifyByEmail { get; set; }
        public bool NotifyBySlack { get; set; }
        public string SlackChanel { get; set; }

        public CommunicationChanel()
        {
            NotifyByEmail = true;
            NotifyBySlack = false;
            SlackChanel = string.Empty;
        }
    }
}
