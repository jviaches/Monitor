using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitor.Core.Models
{
    [DynamoDBTable("Resources")]
    public class Resource
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public string Url { get; set; }
        public string UserId { get; set; }
        public int MonitorPeriod { get; set; }
        public int IsMonitorActivated { get; set; }
        public string MonitorActivationDate { get; set; }

        [DynamoDBIgnore]
        public List<ResourcesHistory> History { get; set; }
    }
}
