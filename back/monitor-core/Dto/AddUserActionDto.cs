using monitor_core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace monitor_core.Dto
{
    public class AddUserActionDto
    {
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public UserActiontype Action { get; set; }
        public string Data { get; set; }
    }
}
