using Monitor.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitor.Core.ViewModels
{
    public class ResourceResultViewModel
    {
        private Resource _resource;

        public ResourceResultViewModel(Resource resource)
        {
            _resource = resource;
            History = resource.History.OrderBy(item => item.RequestDate).Select(item => new ResourceHistoryResultViewModel(item));
            LastStatus = setLastStatus(resource);
        }

        private string setLastStatus(Resource resource)
        {
            var result = resource.History.OrderBy(item => item.RequestDate).LastOrDefault();
            return result == null ? "XXX" : result.Result;
        }

        public string Id { get { return _resource.Id; } }
        public string Url { get { return _resource.Url; } }
        public string UserId { get { return _resource.UserId; } }
        public int MonitorPeriod { get { return _resource.MonitorPeriod; } }
        public int IsMonitorActivated { get { return _resource.IsMonitorActivated; } }
        public string MonitorActivationDate { get { return _resource.MonitorActivationDate; } }
        public string LastStatus { get; private set; }

        public IEnumerable<ResourceHistoryResultViewModel> History { get; set; }
    }
}
