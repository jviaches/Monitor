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
        public string id { get; set; }
        public bool isMonitorActivated { get; set; }
        public string monitorActivationDate { get; set; }
        public string monitorActivationType { get; set; }
        public string url { get; set; }
        public string userId { get; set; }
    }
}
