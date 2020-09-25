using Monitor.Core.Enums;
using System;

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
