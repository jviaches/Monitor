using monitor_core.Dto;
using monitor_infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace monitor_infra.Repositories.Interfaces
{
    public interface IMonitorItemRepository
    {
        MonitorItem Add(AddMonitorItemDto dto);
        Task<MonitorItem> GetById(Guid id);
    }
}
