using System;
using System.Collections.Generic;
using System.Text;

namespace Monitor.Core.ViewModels.UserActions
{
    public class UserActionViewModel
    {
        public string UserId { get; set; }
        public string Date { get; set; }
        public string Action { get; set; }
        public string Data { get; set; }
    }
}
