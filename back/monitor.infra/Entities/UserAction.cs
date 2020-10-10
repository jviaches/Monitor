using monitor_core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace monitor_infra.Entities
{
    public class UserAction
    {
        [Key]
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public UserActiontype Action { get; set; }
        public string Data { get; set; }
    }
}
