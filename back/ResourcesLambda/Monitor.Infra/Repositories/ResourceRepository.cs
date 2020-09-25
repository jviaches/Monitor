using Microsoft.EntityFrameworkCore;
using Monitor.Infra.Entities;
using Monitor.Infra.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitor.Infra.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private AppDbContext _dbContext;

        public ResourceRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Resource Add(Resource entity)
        {
            _dbContext.Set<Resource>().Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }

        public void Delete(Resource entity)
        {
            _dbContext.Set<Resource>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public Task<Resource> GetById(int id)
        {
            var resource = _dbContext.Resources
                   .Include(rs => rs.History)
                   .SingleOrDefaultAsync(rs => rs.Id == id);

            return resource;
        }

        public async Task<IEnumerable<Resource>> GetByUserId(Guid id)
        {
            var resources = _dbContext.Resources
                   .Include(res => res.History)
                   .Where(res => res.UserId == id).ToListAsync();

            return await resources;
            
        }

        public async Task<IEnumerable<Resource>> GetByPeriodicityAndMonitor(int periodicity, bool isMonitored)
        {
            var resources = _dbContext.Resources
                   .Where(res => res.MonitorPeriod == periodicity && res.IsMonitorActivated == isMonitored).ToListAsync();

            return await resources;
        }

        public async Task<IReadOnlyList<Resource>> ListAllAsync()
        {
            throw new NotImplementedException();
        }

        public void Update(Resource entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
