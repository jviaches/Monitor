using monitor_core.Dto;
using monitor_infra.Entities;
using monitor_infra.Repositories.Interfaces;
using monitor_infra.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace monitor_infra.Services
{
    public class MonitorItemService : IMonitorItemService
    {
        private readonly IMonitorItemRepository _monitorItemRepository;

        public MonitorItemService(IMonitorItemRepository monitorItemRepository)
        {
            _monitorItemRepository = monitorItemRepository;
        }

        public async Task<IEnumerable<MonitorItem>> GetByPeriodicityAndMonitor(int periodicity, bool isMonitored)
        {
            return await _monitorItemRepository.GetByPeriodicityAndMonitor(periodicity, isMonitored);
        }

        public async Task AddHistoryItem(AddMonitorHistoryDto dto)
        {
            await _monitorItemRepository.AddHistoryItem(dto);
        }
    }
}
