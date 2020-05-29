using Monitor.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitor.Service.ViewModels.Resources
{ 
    public class ResourceHistoryResultViewModel
    {
        private ResourcesHistory _resourcehistory;

        public ResourceHistoryResultViewModel(ResourcesHistory resourcehistory)
        {
            _resourcehistory = resourcehistory;
        }

        public int Id { get { return _resourcehistory.Id; } }
        public int ResourceId { get { return _resourcehistory.ResourceId; } }
        public DateTime RequestDate => _resourcehistory.RequestDate;

        public string Result { get { return _resourcehistory.Result; } }
    }
}
