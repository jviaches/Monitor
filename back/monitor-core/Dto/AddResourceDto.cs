using System;
using System.Collections.Generic;
using System.Text;

namespace monitor_core.Dto
{
    public class AddResourceDto
    {
        public string Url { get; set; }
        public int UserId { get; set; }
        public bool MonitorActivated { get; set; }
        /// <summary>
        /// In minutes
        /// </summary>
        public int Periodicity { get; set; }
    }
}
