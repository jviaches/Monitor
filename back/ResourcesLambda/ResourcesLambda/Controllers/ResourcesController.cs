using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monitor.Core.Dto;
using Monitor.Core.Enums;
using Monitor.Core.Validations;
using Monitor.Core.ViewModels;
using Monitor.Infra.Entities;
using ResourcesLambda.Services.Resources;

namespace ResourcesLambda.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private IResourceService _resourceService;
        private IUserActionService _userActionService;

        public ResourcesController(IResourceService resourceService, IUserActionService userActionService)
        {
            _resourceService = resourceService;
            _userActionService = userActionService;
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
                UserId = resourceHistoryVM.UserId,
                MonitorPeriod = resourceHistoryVM.MonitorPeriod,
                IsMonitorActivated = resourceHistoryVM.IsMonitorActivated
            });

            _userActionService.Add(new UserActionDto()
            {
                UserId = resourceHistoryVM.UserId,
                Date = DateTime.UtcNow,
                Action = UserActiontype.ResourceAdded,
                Data = $@"Url: [{resourceHistoryVM.Url}], 
                       IsActivated: [{resourceHistoryVM.IsMonitorActivated}], 
                       Activation Period: [{resourceHistoryVM.MonitorPeriod}]"
            });

            return Ok();
        }

        //[HttpPost("{id}", Name = "/Resources/Update")]
        //[ProducesResponseType(typeof(ErrorViewModel), 400)]
        //[ProducesResponseType(200)]
        //public async Task<IActionResult> Update([FromBody] UpdateResourceViewModel viewModel)
        //{
        //    var oldResource = await _resourceService.GetById(viewModel.Id);

        //    var result = await _resourceService.Update(viewModel);
        //    if (!result.Success)
        //        return BadRequest(new ErrorViewModel { Message = $"Error to update resource: {viewModel.Url}" });

        //    await _userActionService.Add(new UserActionViewModel()
        //    {
        //        UserId = viewModel.UserId,
        //        Date = DateTime.UtcNow.ToString(),
        //        Action = UserAction.ResourceUpdated.ToString(),
        //        Data = $@"Url: [old: {oldResource.Url}, new: {viewModel.Url}], 
        //                IsActivated: [old: {oldResource.IsMonitorActivated}, new: {viewModel.IsMonitorActivated}], 
        //                Activation Period: [old: {oldResource.MonitorPeriod}, new: {viewModel.MonitorPeriod}]"
        //    });
        //    return Ok();
        //}

        //[Authorize(Policy = "Admin")]
        //[HttpDelete]
        //public async Task<IActionResult> Delete([FromBody] UpdateResourceViewModel viewModel)
        //{
        //    var result = await _resourceService.Delete(viewModel);
        //    if (!result.Success)
        //        return BadRequest(new ErrorViewModel { Message = $"Error deleting resource: {viewModel.Url}" });

        //    await _userActionService.Add(new UserActionViewModel()
        //    {
        //        UserId = viewModel.UserId,
        //        Date = DateTime.UtcNow.ToString(),
        //        Action = UserAction.ResourceRemoved.ToString(),
        //        Data = $@"Url: [{viewModel.Url}], 
        //                IsActivated: [{viewModel.IsMonitorActivated}], 
        //                Activation Period: [{viewModel.MonitorPeriod}]"
        //    });
        //    return Ok();
        //}
    }
}
