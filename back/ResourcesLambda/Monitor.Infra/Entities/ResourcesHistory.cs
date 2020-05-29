using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitor.Infra.Entities
{
    public class ResourcesHistory: BaseEntity
    {
        public int ResourceId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Result { get; set; }
    }
}
