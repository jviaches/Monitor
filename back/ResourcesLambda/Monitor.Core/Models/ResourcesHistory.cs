using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitor.Core.Models
{
    [DynamoDBTable("ResourcesHistory")]
    public class ResourcesHistory
    {
        [DynamoDBHashKey]
        public string id { get; set; }
        public string resourceId { get; set; }
        public string monitorTypeId { get; set; }
        public string requestDate { get; set; }
        public string responseDate { get; set; }
        public string result { get; set; }
    }
}
