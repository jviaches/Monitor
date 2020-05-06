using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitor.Core.ViewModels
{
    public class ResourceViewModel
    {
        public string Url { get; set; }
        public int UserId { get; set; }

        /// <summary>
        /// In minutes
        /// </summary>
        public int MonitorPeriod { get; set; }
        public bool IsMonitorActivated { get; set; }
    }
}
