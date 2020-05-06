using Monitor.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitor.Core.ViewModels
{ 
    public class ResourceHistoryResultViewModel
    {
        private ResourcesHistory _resourcehistory;

        public ResourceHistoryResultViewModel(ResourcesHistory resourcehistory)
        {
            _resourcehistory = resourcehistory;
        }

        public int Id { get { return _resourcehistory.Id; } }
        public string ResourceId { get { return _resourcehistory.ResourceId; } }
        public string RequestDate => _resourcehistory.RequestDate;

        public string Result { get { return _resourcehistory.Result; } }
    }
}
