using Monitor.Core.Dto;
using Monitor.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Infra.Interfaces.Services
{
    public interface IIntegrationSettingsService
    {
        Task<IntegrationSettings> GetByEmail(string email);
        Task<IntegrationSettings> AddDefault(IntegrationSettingsDto integrationSettingsDto);
        Task Update(IntegrationSettingsDto integrationSettingsDto);
        Task Delete(int id);
    }
}
