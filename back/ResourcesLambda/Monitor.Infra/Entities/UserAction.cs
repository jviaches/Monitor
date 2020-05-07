using Amazon.DynamoDBv2.DataModel;
using Monitor.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monitor.Infra.Entities
{
    public class UserAction: BaseEntity
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public UserActiontype Action { get; set; }
        public string Data { get; set; }
    }
}
