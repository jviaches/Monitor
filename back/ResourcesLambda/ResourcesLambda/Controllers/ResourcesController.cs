  using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monitor.Core.Interfaces;
using Monitor.Core.Validations;
using Monitor.Core.ViewModels;

namespace ResourcesLambda.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private IResourceService _resourceService;

        public ResourcesController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        // GET: api/Resources/5
        [HttpGet("{id}", Name = "/Resources/Get")]
        public async Task<Result> GetById(string id)
        {
            var result = await _resourceService.GetById(id);
            return result == null ? Result.Fail("Requested resource is not found.") : Result.Ok();
        }

        // GET: api/Resources/5
        //[Authorize(Policy = "Admin")]
        [Route("[action]/{id}")]
        [HttpGet]
        public async Task<IEnumerable<ResourceResultViewModel>> GetByUserId(string id)
        {
            var resources = await _resourceService.GetByUserId(id);
            return resources.Select(res => new ResourceResultViewModel(res));
        }

        // POST: api/Resources
        [HttpPost]
        [ProducesResponseType(typeof(ErrorViewModel), 400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Add([FromBody] ResourceViewModel resourceHistoryVM)
        {
           var result = await _resourceService.Add(resourceHistoryVM);
            if (!result.Success)
                return BadRequest(new ErrorViewModel { Message = "Unable to add new resource" });
            
            return Ok();
        }

        [HttpPost("{id}", Name = "/Resources/Update")]
        [ProducesResponseType(typeof(ErrorViewModel), 400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Update([FromBody] UpdateResourceViewModel viewModel)
        {
            var result = await _resourceService.Update(viewModel);
            if (!result.Success)
                return BadRequest(new ErrorViewModel { Message = $"Error to update resource: {viewModel.Url}" });

            return Ok();
        }

        //[Authorize(Policy = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] UpdateResourceViewModel viewModel)
        {
            var result = await _resourceService.Delete(viewModel);
            if (!result.Success)
                return BadRequest(new ErrorViewModel { Message = $"Error deleting resource: {viewModel.Url}" });

            return Ok();
        }
    }
}
