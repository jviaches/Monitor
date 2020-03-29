using System;
using System.Collections.Generic;
using System.Text;

namespace Monitor.StepperLogic.StateModels
{
    public class ResourceScanResultModel
    {
        /// <summary>
        /// <URL, StatusCode>
        /// </summary>
        public List<ResourceModel> ResourcesStatuses { get; set; }

        public ResourceScanResultModel()
        {
            ResourcesStatuses = new List<ResourceModel>();
        }
    }

    public class ResourceModel
    {
        public ResourceModel(string resourceId, string url, string statusCode)
        {
            ResourceId = resourceId;
            Url = url;
            StatusCode = statusCode;
        }

        public string ResourceId { get; set; }
        public string Url { get; set; }
        public string StatusCode { get; set; }
    }
}
