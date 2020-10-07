using Microsoft.EntityFrameworkCore;
using monitor_infra.Entities;
using monitor_infra.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monitor_infra.Repositories
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

        public Task<UserAction> GetById(Guid id)
        {
            var resource = _dbContext.UserActions
                   .Include(rs => rs.Id)
                   .SingleOrDefaultAsync(rs => rs.Id == id);

            return resource;
        }

        public async Task<IEnumerable<UserAction>> GetByUserId(int id)
        {
            var resources = _dbContext.UserActions.Where(res => res.UserId == id).ToListAsync();
            return await resources;

        }
    }
}
