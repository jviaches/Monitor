using System;
using System.Collections.Generic;
using System.Text;

namespace monitor_core.Dto
{
    public class UpdateResourceDto
    {
        public Guid ResourceId { get; set; }
        public bool IsMonitorActivate { get; set; }
    }
}
