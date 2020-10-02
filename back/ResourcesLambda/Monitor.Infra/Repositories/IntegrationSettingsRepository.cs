using Microsoft.EntityFrameworkCore;
using Monitor.Infra.Entities;
using Monitor.Infra.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Infra.Repositories
{
    public class IntegrationSettingsRepository : IIntegrationSettingsRepository
    {
        private AppDbContext _dbContext;

        public IntegrationSettingsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IntegrationSettings Add(IntegrationSettings entity)
        {
            throw new NotImplementedException();
        }

        public Task<IntegrationSettings> AddDefault(IntegrationSettings integrationSettings)
        {
            _dbContext.Set<IntegrationSettings>().Add(integrationSettings);
            _dbContext.SaveChanges();

            return Task.FromResult(integrationSettings);
        }

        public Task<IntegrationSettings> GetByEmail(string email)
        {
            return _dbContext.IntegrationSettings.SingleOrDefaultAsync(rs => rs.UserEmail == email);
        }

        public async Task Update(IntegrationSettings integrationSettings)
        {
            var existingSettings = await GetByEmail(integrationSettings.UserEmail);
            existingSettings.NotificationEmail = integrationSettings.NotificationEmail;
            existingSettings.NotificationSlack = integrationSettings.NotificationSlack;
            existingSettings.SlackChannelUrl = integrationSettings.SlackChannelUrl;

            _dbContext.IntegrationSettings.Update(existingSettings);
            _dbContext.SaveChanges();
        }
    }
}
