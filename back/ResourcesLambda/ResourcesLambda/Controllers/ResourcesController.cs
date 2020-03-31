using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Monitor.Core.Interfaces;
using Monitor.Core.Validations;
using Monitor.Core.ViewModels;

namespace ResourcesLambda.Controllers
{
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
        public async Task<IActionResult> Post([FromBody] ResourceViewModel resourceHistoryVM)
        {
           var result = await _resourceService.Add(resourceHistoryVM);
            if (!result.Success)
                return BadRequest(new ErrorViewModel { Message = "Unable to add new resource" });
            
            return Ok();
        }

        // PUT: api/Resources/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
