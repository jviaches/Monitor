using System;
using System.Collections.Generic;
using System.Text;

namespace Monitor.Core.Dto
{
    public class AddResourceHistoryDto
    {
        public int ResourceId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Result { get; set; }
    }
}
