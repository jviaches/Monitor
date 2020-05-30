using Monitor.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResourcesLambda.Services.Resources
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

        public int Id { get { return _resource.Id; } }
        public string Url { get { return _resource.Url; } }
        public Guid UserId { get { return _resource.UserId; } }
        public int MonitorPeriod { get { return _resource.MonitorPeriod; } }
        public bool IsMonitorActivated { get { return _resource.IsMonitorActivated; } }
        public DateTime MonitorActivationDate { get { return _resource.MonitorActivationDate; } }
        public string LastStatus { get; private set; }

        public IEnumerable<ResourceHistoryResultViewModel> History { get; set; }
    }
}
