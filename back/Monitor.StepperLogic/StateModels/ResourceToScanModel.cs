using System;
using System.Collections.Generic;
using System.Text;

namespace Monitor.StepperLogic.StateModels
{
    public class ResourceToScanModel
    {
        /// <summary>
        /// Collection of web sites to monitor their status
        /// </summary>
        public List<string> UrlList { get; set; }

        public ResourceToScanModel()
        {
            UrlList = new List<string>();
        }
    }
}
