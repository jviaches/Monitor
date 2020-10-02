using Monitor.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Infra.Interfaces.Repository
{
    public interface IIntegrationSettingsRepository
    {
        Task<IntegrationSettings> AddDefault(IntegrationSettings integrationSettings);
        Task<IntegrationSettings> GetByEmail(string email);
        Task Update(IntegrationSettings integrationSettings);
    }
}
