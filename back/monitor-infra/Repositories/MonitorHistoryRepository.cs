using Microsoft.EntityFrameworkCore;
using monitor_core.Dto;
using monitor_infra.Entities;
using monitor_infra.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace monitor_infra.Repositories
{
    public class MonitorHistoryRepository : IMonitorHistoryRepository
    {
        private AppDbContext _dbContext;

        public MonitorHistoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public MonitorHistory Add(AddMonitorHistoryDto addUserDto)
        {
            var monitorHistory = new MonitorHistory();
            _dbContext.Set<MonitorHistory>().Add(monitorHistory);
            _dbContext.SaveChanges();

            return monitorHistory;

        }
        public MonitorHistory GetById(Guid id)
        {
            var resource = _dbContext.MonitorHistory.SingleOrDefault(rs => rs.Id == id);
            return resource;
        }
    }
}
