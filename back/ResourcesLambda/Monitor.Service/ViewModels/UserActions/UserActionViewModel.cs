using System;
using System.Collections.Generic;
using System.Text;

namespace Monitor.Service.ViewModels.UserActions
{
    public class UserActionViewModel
    {
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string Action { get; set; }
        public string Data { get; set; }
    }
}
