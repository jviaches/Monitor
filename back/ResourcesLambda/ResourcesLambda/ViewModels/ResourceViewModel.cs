using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcesLambda.ViewModels
{
    public class ResourceViewModel
    {
        public bool isMonitorActivated { get; set; }
        public string monitorActivationDate { get; set; }
        public string monitorActivationType { get; set; }
        public string url { get; set; }
        public string userId { get; set; }
    }
}
