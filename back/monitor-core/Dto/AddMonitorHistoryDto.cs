using System;
using System.Collections.Generic;
using System.Text;

namespace monitor_core.Dto
{
    public class AddMonitorHistoryDto
    {
        public Guid ResourceId { get; set; }
        public DateTime ScanDate { get; set; }
        public string Result { get; set; }
    }
}