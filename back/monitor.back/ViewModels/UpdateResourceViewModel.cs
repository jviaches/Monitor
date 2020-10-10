using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace monitorback.ViewModels
{
    public class UpdateResourceViewModel
    {
        public Guid ResourceId { get; set; }
        public bool IsMonitorActivate { get; set; }
    }
}
