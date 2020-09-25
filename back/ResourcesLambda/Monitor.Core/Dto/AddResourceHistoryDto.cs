using System;

namespace Monitor.Core.Dto
{
    public class AddResourceHistoryDto
    {
        public int ResourceId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Result { get; set; }
    }
}
