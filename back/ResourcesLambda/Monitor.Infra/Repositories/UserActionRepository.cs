using Microsoft.EntityFrameworkCore;
using Monitor.Infra.Entities;
using Monitor.Infra.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Infra.Repositories
{
    public class UserActionRepository : IUserActionRepository
    {
        private AppDbContext _dbContext;

        public UserActionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public UserAction Add(UserAction entity)
        {
            _dbContext.Set<UserAction>().Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }

        public void Delete(UserAction entity)
        {
            throw new NotImplementedException();
        }

        public Task<UserAction> GetById(int id)
        {
            var resource = _dbContext.UserAction
                   .Include(rs => rs.Id)
                   .SingleOrDefaultAsync(rs => rs.Id == id);

            return resource;
        }

        public async Task<IEnumerable<UserAction>> GetByUserId(int id)
        {
            var resources = _dbContext.UserAction.Where(res => res.UserId == id).ToListAsync();
            return await resources;
            
        }

        public async Task<IReadOnlyList<UserAction>> ListAllAsync(int userId)
        {
            var resources = _dbContext.UserAction
                   .Where(rs => rs.UserId == userId).ToListAsync();

            return await resources;
        }

        public void Update(UserAction entity)
        {
            throw new NotImplementedException();
        }
    }
}
