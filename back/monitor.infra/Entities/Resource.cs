using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace monitor_infra.Entities
{
    public class Resource
    {
        [Key]
        public Guid Id { get; set; }
        public string Url { get; set; }
        public int UserId { get; set; }
        public CommunicationChanel CommunicationChanel { get; set; }
        public bool MonitoringActivated { get; set; }
        public MonitorItem MonitorItem { get; set; }

        public Resource()
        {
            CommunicationChanel = new CommunicationChanel();
        }
    }
}
