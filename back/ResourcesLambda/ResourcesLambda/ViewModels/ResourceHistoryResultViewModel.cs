using ResourcesLambda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcesLambda.ViewModels
{
    public class ResourceHistoryResultViewModel
    {
        private ResourcesHistory _resourcehistory;

        public ResourceHistoryResultViewModel(ResourcesHistory resourcehistory)
        {
            _resourcehistory = resourcehistory;
        }

        public string id { get { return _resourcehistory.id; } }
        public string resourceId { get { return _resourcehistory.resourceId; } }
        public string monitorTypeId { get { return _resourcehistory.monitorTypeId; } }
        public string requestDate { get { return _resourcehistory.requestDate; } }
        public string responseDate { get { return _resourcehistory.responseDate; } }
        public string result { get { return _resourcehistory.result; } }
    }
}
