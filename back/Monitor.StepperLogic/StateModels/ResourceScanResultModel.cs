using monitor_infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monitor.StepperLogic.StateModels
{
    public class MonitorItemScanResultModel
    {
        /// <summary>
        /// <URL, StatusCode>
        /// </summary>
        public List<MonitorItemModel> ResourcesStatuses { get; set; }

        public MonitorItemScanResultModel()
        {
            ResourcesStatuses = new List<MonitorItemModel>();
        }
    }

    public class MonitorItemModel
    {
        public MonitorItemModel(Guid monitorItemId, string url, string statusCode, CommunicationChanel communicationChanel, int userId)
        {
            MonitorId = monitorItemId;
            Url = url;
            StatusCode = statusCode;
            CommunicationChanel = communicationChanel;
            UserId = userId;
        }

        public Guid MonitorId { get; set; }
        public string Url { get; set; }
        public string StatusCode { get; set; }
        public CommunicationChanel CommunicationChanel { get; set; }
        public int UserId { get; set; }
    }
}
