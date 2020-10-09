using monitor_infra.Entities;
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
        public ResourceModel(Guid resourceId, string url, string statusCode, CommunicationChanel communicationChanel, int userId)
        {
            ResourceId = resourceId;
            Url = url;
            StatusCode = statusCode;
            CommunicationChanel = communicationChanel;
            UserId = userId;
        }

        public Guid ResourceId { get; set; }
        public string Url { get; set; }
        public string StatusCode { get; set; }
        public CommunicationChanel CommunicationChanel { get; set; }
        public int UserId { get; set; }
    }
}
