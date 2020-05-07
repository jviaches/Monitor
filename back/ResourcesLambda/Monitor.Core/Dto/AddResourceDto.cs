using System;
using System.Collections.Generic;
using System.Text;

namespace Monitor.Core.Dto
{
    public class AddResourceDto
    {
        public string Url { get; set; }
        public Guid UserId { get; set; }

        /// <summary>
        /// In minutes
        /// </summary>
        public int MonitorPeriod { get; set; }
        public bool IsMonitorActivated { get; set; }
    }
}
