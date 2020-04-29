﻿using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitor.Core.Models
{
    [DynamoDBTable("ResourcesHistory-prod")]
    public class ResourcesHistory
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public string ResourceId { get; set; }
        public string RequestDate { get; set; }
        public string Result { get; set; }
    }
}
