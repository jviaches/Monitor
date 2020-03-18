using ResourcesLambda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcesLambda.ViewModels
{
    public class ResourceResultViewModel
    {
        private Resource _resource;

        public ResourceResultViewModel(Resource resource)
        {
            _resource = resource;
            history = new List<ResourceHistoryResultViewModel>();
        }

        public string id { get { return _resource.id; } }
        public bool isMonitorActivated { get { return _resource.isMonitorActivated; } }
        public string monitorActivationDate { get { return _resource.monitorActivationDate; } }
        public string monitorActivationType { get { return _resource.monitorActivationType; } }
        public string url { get { return _resource.url; } }
        public string userId { get { return _resource.userId; } }
        public IEnumerable<ResourceHistoryResultViewModel> history { get; set; }
    }
}
