using Monitor.Core.Enums;
using System;

namespace Monitor.Core.Dto
{
    public class UserActionDto
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public UserActiontype Action { get; set; }
        public string Data { get; set; }
    }
}
