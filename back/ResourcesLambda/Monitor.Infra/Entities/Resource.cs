using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitor.Infra.Entities
{
    public class Resource: BaseEntity
    {
        public string Url { get; set; }
        public Guid UserId { get; set; }
        public int MonitorPeriod { get; set; }
        public bool IsMonitorActivated { get; set; }
        public DateTime MonitorActivationDate { get; set; }

        public List<ResourcesHistory> History { get; set; }
    }
}
