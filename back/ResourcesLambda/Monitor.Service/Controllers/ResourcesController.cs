using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Monitor.Core.Dto;
using Monitor.Core.Validations;
using Monitor.Infra.Entities;
using Monitor.Service.ViewModels;
using Monitor.Service.ViewModels.Resources;

namespace Monitor.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourcesController : ControllerBase
    {
        private readonly IResourceService _resourceService;

        public ResourcesController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        // GET: api/Resources/5
        [HttpGet("{id}", Name = "/Resources/Get")]
        public async Task<Result> GetById(int id)
        {
            var result = await _resourceService.GetById(id);
            return result == null ? Result.Fail("Requested resource is not found.") : Result.Ok();
        }

        // GET: api/Resources/5
        //[Authorize(Policy = "Admin")]
        [Route("[action]/{id}")]
        [HttpGet]
        public async Task<IEnumerable<ResourceResultViewModel>> GetByUserId(Guid id)
        {
            var resources = await _resourceService.GetByUserId(id);
            return resources.Select(res => new ResourceResultViewModel(res));
        }

        // POST: api/Resources
        [HttpPost]
        [ProducesResponseType(typeof(ErrorViewModel), 400)]
        [ProducesResponseType(200)]
        public IActionResult Add([FromBody] ResourceViewModel resourceHistoryVM)
        {
            _resourceService.Add(new AddResourceDto()
            {
                Url = resourceHistoryVM.Url,
                UserId = new Guid(resourceHistoryVM.UserId),
                MonitorPeriod = resourceHistoryVM.MonitorPeriod,
                IsMonitorActivated = resourceHistoryVM.IsMonitorActivated
            });

            return Ok();
        }

        [HttpPost("{id}", Name = "/Resources/Update")]
        [ProducesResponseType(typeof(ErrorViewModel), 400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Update([FromBody] UpdateResourceViewModel viewModel)
        {
            await _resourceService.Update(new UpdateResourceDto()
            {
                Id = viewModel.Id,
                Url = viewModel.Url,
                UserId = new Guid(viewModel.UserId),
                MonitorPeriod = viewModel.MonitorPeriod,
                IsMonitorActivated = viewModel.IsMonitorActivated
            });

            return Ok();
        }

        //[Authorize(Policy = "Admin")]
        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _resourceService.Delete(id);
            return Ok();
        }
    }
}
