using Microsoft.EntityFrameworkCore;
using monitor_core.Dto;
using monitor_infra.Entities;
using monitor_infra.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monitor_infra.Repositories
{
    public class MonitorItemRepository : IMonitorItemRepository
    {
        private AppDbContext _dbContext;

        public MonitorItemRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public MonitorItem Add(AddMonitorItemDto dto)
        {
            var monitorItem = new MonitorItem()
            {
                IsActive = dto.IsActive,
                ResourceId = dto.ResourceId,
                Period = dto.Period
            };

            var newItem = _dbContext.Set<MonitorItem>().Add(monitorItem);
            _dbContext.SaveChanges();

            return newItem.Entity;
        }

        public async Task<MonitorItem> GetById(Guid id)
        {
            var result = await _dbContext.MonitorItems.FirstOrDefaultAsync(rs => rs.Id == id);
            return result;
        }
    }
}