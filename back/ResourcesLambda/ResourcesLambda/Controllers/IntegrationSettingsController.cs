using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monitor.Core.Dto;
using Monitor.Core.Validations;
using Monitor.Core.ViewModels;
using Monitor.Infra.Entities;
using Monitor.Infra.Interfaces.Services;
using ResourcesLambda.ViewModels.IntegrationSettings;

namespace ResourcesLambda.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class IntegrationSettingsController : ControllerBase
    {
        private readonly IIntegrationSettingsService _integrationSettingsService;
        public IntegrationSettingsController(IIntegrationSettingsService integrationSettingsService)
        {
            _integrationSettingsService = integrationSettingsService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ErrorViewModel), 400)]
        [Route("api/[action]")]
        [ProducesResponseType(200)]
        public async Task<IntegrationSettingsResponse> AddDefault([FromBody]IntegrationSettingsViewModel viewModel)
        {
            var result = await _integrationSettingsService.AddDefault(new IntegrationSettingsDto()
            {
                UserEmail = viewModel.UserEmail
            });

            var response = new IntegrationSettingsResponse()
            {
                Id = result.Id,
                UserEmail = result.UserEmail,
                NotificationEmail = result.NotificationEmail,
                NotificationSlack = result.NotificationSlack,
                NotificationSlackChannel = result.SlackChannelUrl
            };

            return response;
        }

        [HttpGet]
        [Route("[action]/{email}")]
        public async Task<IntegrationSettings> GetByEmail([FromRoute] string email)
        {
            return await _integrationSettingsService.GetByEmail(email);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ErrorViewModel), 400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Update([FromBody]IntegrationSettingsUpdateViewModel viewModel)
        {
            await _integrationSettingsService.Update(new IntegrationSettingsDto()
            {
                UserEmail = viewModel.UserEmail,
                NotificationEmail = viewModel.NotificationEmail,
                NotificationSlack = viewModel.NotificationSlack,
                NotificationSlackChannel = viewModel.SlackChannelUrl
            });

            return Ok();
        }
    }
}