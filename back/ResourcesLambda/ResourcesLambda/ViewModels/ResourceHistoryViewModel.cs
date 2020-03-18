using ResourcesLambda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcesLambda.ViewModels
{
    public class ResourceHistoryViewModel
    {
        public string resourceId { get; set; }
        public string monitorTypeId { get; set; }
        public string requestDate { get; set; }
    }
}
