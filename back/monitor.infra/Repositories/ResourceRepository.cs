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
    public class ResourceRepository : IResourceRepository
    {
        private AppDbContext _dbContext;
        private IMonitorItemRepository _monitorItemRepository;

        public ResourceRepository(AppDbContext dbContext, IMonitorItemRepository monitorItemRepository)
        {
            _dbContext = dbContext;
            _monitorItemRepository = monitorItemRepository;
        }

        public Resource Add(AddResourceDto addResourceDto)
        {
            var resource = new Resource()
            {
                Url = addResourceDto.Url,
                UserId = addResourceDto.UserId,
                MonitoringActivated = addResourceDto.MonitorActivated,
                CommunicationChanel = new CommunicationChanel(),
            };

            var newResource = _dbContext.Set<Resource>().Add(resource);

            var monitorItem =_monitorItemRepository.Add(new AddMonitorItemDto()
            {
                IsActive = addResourceDto.MonitorActivated,
                Period = addResourceDto.Periodicity,
                ResourceId = newResource.Entity.Id
            });

            newResource.Entity.MonitorItem = monitorItem;
            _dbContext.SaveChanges();

            return resource;
        }

        public Task<Resource> Delete(Guid resourceId)
        {
            var resource = GetById(resourceId);
            _dbContext.Set<Resource>().Remove(resource.Result);
            _dbContext.SaveChanges();

            return resource;
        }

        public Task<Resource> GetById(Guid id)
        {
            var resource = _dbContext.Resources
                .Include(rs => rs.MonitorItem)
                .Include(rs => rs.CommunicationChanel)
                .SingleOrDefaultAsync(rs => rs.Id == id);
            return resource;
        }

        public async Task<IEnumerable<Resource>> GetByUserId(int userId)
        {
            // TODO: return History data in asc order
            var resource = await _dbContext.Resources
                .Include(rs => rs.MonitorItem)
                    .ThenInclude(l => l.History)
                .Include(rs => rs.CommunicationChanel)
                .Where(rs => rs.UserId == userId)
                .ToListAsync();


            for (int i = 0; i < resource.Count; i++)
                resource[i] = monitorHistorySortAsc(resource[i]);

            return resource;
        }


        private Resource monitorHistorySortAsc(Resource resource)
        {
            var monitorHistories =  resource.MonitorItem.History.OrderBy(e => e.ScanDate);
            resource.MonitorItem.History = monitorHistories.ToList();
            return resource;
        }
        public Dictionary<Resource, MonitorItem> GetMonitoredItemsByPeriodicity(int periodicity)
        {
            var result = new Dictionary<Resource, MonitorItem>();

            var foundItems = _dbContext.Resources
                .Include(rs => rs.MonitorItem)
                .Include(rs => rs.CommunicationChanel)
                .Where(rs => rs.MonitoringActivated && rs.MonitorItem.IsActive && rs.MonitorItem.Period == periodicity)
                .Select(res => new KeyValuePair<Resource, MonitorItem>(res,res.MonitorItem));

            foreach (var item in foundItems)
                result.Add(item.Key, item.Value);

            return result;
        }

        public Task<Resource> Update(UpdateResourceDto resourcedto)
        {
            var resource = GetById(resourcedto.ResourceId);
            resource.Result.MonitorItem.IsActive = resourcedto.IsMonitorActivate;

            _dbContext.Set<Resource>().Update(resource.Result);
            _dbContext.SaveChanges();

            return resource;
        }
    }
}