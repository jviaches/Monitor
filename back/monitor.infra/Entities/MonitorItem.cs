using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace monitor_infra.Entities
{
    public class MonitorItem
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ResourceId { get; set; }
        public int Period { get; set; }
        public bool IsActive { get; set; }
        public DateTime ActivationDate { get; set; }
        public List<MonitorHistory> History { get; set; }

        public MonitorItem()
        {
            History = new List<MonitorHistory>();
        }
    }
}
