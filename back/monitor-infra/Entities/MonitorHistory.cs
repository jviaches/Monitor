using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace monitor_infra.Entities
{
    public class MonitorHistory
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ResourceId { get; set; }
        public DateTime ScanDate { get; set; }
        public string Result { get; set; }
    }
}
