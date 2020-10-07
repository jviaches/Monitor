using System;
using System.Collections.Generic;
using System.Text;

namespace monitor_core.Dto
{
    public class AddMonitorItemDto
    {
        public Guid ResourceId { get; set; }
        public int Period { get; set; }
        public bool IsActive { get; set; }
    }
}
