using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.XRay.Recorder.Core;
using Microsoft.AspNetCore.Mvc;
using monitor_core.Dto;
using monitor_infra.Services.Interfaces;
using monitorback.ViewModels;

namespace monitor.back.Controllers
{
    [Route("api/[controller]")]
    public class ResourcesController : ControllerBase
    {
        private readonly IResourceService _resourceService;

        public ResourcesController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        // GET api/Resources/GetById/5
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var resource = await _resourceService.GetById(id);
            return Ok(resource);
        }

        // GET api/Resources/GetByUserId/5
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetByUserId(int id)
        {
            AWSXRayRecorder.Instance.BeginSubsegment("Resources: GetByUserId call");

            var resourceList = await _resourceService.GetByUserId(id);

            AWSXRayRecorder.Instance.EndSubsegment();

            return Ok(resourceList);
        }

        // POST api/Resources
        [HttpPost]
        [Route("[action]")]
        public IActionResult Add([FromBody]AddResourceViewModel vm)
        {
            var resource = _resourceService.Add(new AddResourceDto()
            {
                UserId = vm.UserId,
                Url = vm.Url,
                Periodicity = vm.Periodicity,
                MonitorActivated = vm.MonitorActivated
            });

            return Ok(resource);
        }

        // PUT api/values/5
        [HttpPost]
        [Route("[action]")]
        public void Update([FromBody]UpdateResourceViewModel vm)
        {
            _resourceService.Update(new UpdateResourceDto()
            {
                ResourceId = vm.ResourceId,
                IsMonitorActivate = vm.IsMonitorActivate
            });
        }

        // DELETE api/values/5
        [HttpDelete("[action]/{id}")]
        public void Delete(Guid id)
        {
           _resourceService.Delete(id);
        }
    }
}
