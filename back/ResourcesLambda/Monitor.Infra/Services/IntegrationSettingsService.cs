using Monitor.Core.Dto;
using Monitor.Infra.Entities;
using Monitor.Infra.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Infra.Interfaces.Services
{
    public class IntegrationSettingsService : IIntegrationSettingsService
    {
        private IIntegrationSettingsRepository _integrationSettingsRepository;

        public IntegrationSettingsService(IIntegrationSettingsRepository integrationSettingsRepository)
        {
            _integrationSettingsRepository = integrationSettingsRepository;
        }

        public Task<IntegrationSettings> AddDefault(IntegrationSettingsDto integrationSettingsDto)
        {
            return _integrationSettingsRepository.AddDefault(new IntegrationSettings()
            {
                UserEmail = integrationSettingsDto.UserEmail,
                NotificationEmail = true,
                NotificationSlack = false,
                SlackChannelUrl = string.Empty
            });
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IntegrationSettings> GetByEmail(string email)
        {
            return _integrationSettingsRepository.GetByEmail(email);
        }

        public async Task Update(IntegrationSettingsDto integrationSettingsDto)
        {
            await _integrationSettingsRepository.Update(new IntegrationSettings()
            {
                UserEmail = integrationSettingsDto.UserEmail,
                NotificationEmail = integrationSettingsDto.NotificationEmail,
                NotificationSlack = integrationSettingsDto.NotificationSlack,
                SlackChannelUrl = integrationSettingsDto.NotificationSlackChannel
            });
        }
    }
}
