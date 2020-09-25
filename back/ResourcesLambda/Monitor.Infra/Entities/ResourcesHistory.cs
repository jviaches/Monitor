using System;

namespace Monitor.Infra.Entities
{
    public class ResourcesHistory: BaseEntity
    {
        public int ResourceId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Result { get; set; }
    }
}
