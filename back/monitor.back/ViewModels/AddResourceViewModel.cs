using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace monitorback.ViewModels
{
    public class AddResourceViewModel
    {
        [Required]
        public string Url { get; set; }

        [Required]
        public int UserId { get; set; }
        public bool MonitorActivated { get; set; }
        
        /// <summary>
        /// In minutes
        /// </summary>
        public int Periodicity { get; set; }
    }
}
