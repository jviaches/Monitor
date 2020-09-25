using Microsoft.EntityFrameworkCore;
using Monitor.Infra.Entities;
using Monitor.Infra.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monitor.Infra.Repositories
{
    public class ResourceHistoryRepository : IResourceHistoryRepository
    {
        private AppDbContext _dbContext;

        public ResourceHistoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ResourcesHistory Add(ResourcesHistory entity)
        {
            _dbContext.Set<ResourcesHistory>().Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }

        public void Delete(ResourcesHistory entity)
        {
            throw new NotImplementedException();
        }

        public Task<ResourcesHistory> GetById(int id)
        {
            var resource = _dbContext.ResourcesHistory
                   .Include(rs => rs.Id)
                   .SingleOrDefaultAsync(rs => rs.Id == id);

            return resource;
        }

        public Task<List<Resource>> GetByUserId(int userid)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ResourcesHistory>> ListAllAsync()
        {
            throw new NotImplementedException();
        }

        public void Update(ResourcesHistory entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
