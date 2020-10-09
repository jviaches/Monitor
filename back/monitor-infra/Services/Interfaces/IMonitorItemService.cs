using monitor_core.Dto;
using monitor_infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace monitor_infra.Services.Interfaces
{
    public interface IMonitorItemService
    {
        void AddHistoryItem(AddMonitorHistoryDto dto);
    }
}
