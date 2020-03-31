using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcesLambda.ViewModels
{
    public class ResourceViewModel
    {
        public string Url { get; set; }
        public string UserId { get; set; }

        /// <summary>
        /// In minutes
        /// </summary>
        public int MonitorPeriod { get; set; }
        public int IsMonitorActivated { get; set; }
        public string MonitorActivationDate { get; set; }
    }
}
