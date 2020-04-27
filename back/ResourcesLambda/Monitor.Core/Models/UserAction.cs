using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monitor.Core.Models
{
    public class UserAction
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Date { get; set; }
        public string Action { get; set; }
        public string Data { get; set; }
    }
}
