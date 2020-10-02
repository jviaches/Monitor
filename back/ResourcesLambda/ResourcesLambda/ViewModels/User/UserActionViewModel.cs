using System;

namespace ResourcesLambda.Services.User
{
    public class UserActionViewModel
    {
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string Action { get; set; }
        public string Data { get; set; }
    }
}
