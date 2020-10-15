using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.XRay.Recorder.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using monitor.infra.Attributes;
using monitor_core.Dto;
using monitor_infra.Models.Response;
using monitor_infra.Services.Interfaces;
using monitorback.ViewModels;

namespace monitor_back.Controllers
{
    [EnableCors("GlobalCorPolicy")]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UsersController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        // POST api/users
        [AllowAnonymous]
        [SkipJwtToken]
        [HttpPost]
        [Route("[action]")]
        public IActionResult SignUp([FromBody]AddUserViewModel vm)
        {
            AWSXRayRecorder.Instance.BeginSubsegment("Users: SignIn call");

            if (string.IsNullOrEmpty(vm.Email) || string.IsNullOrEmpty(vm.Email))
                return BadRequest(new { message = "input data is empty!" });

            var user = _userService.Add(vm.Email, vm.Password);

            AWSXRayRecorder.Instance.EndSubsegment();

            if (user == null)
                return BadRequest(new { message = "User already exist!" });

            return Ok(user);
        }

        [AllowAnonymous]
        [SkipJwtToken]
        [HttpPost]
        [Route("[action]")]
        public IActionResult SignIn([FromBody]SignInViewModel vm)
        {
            AWSXRayRecorder.Instance.BeginSubsegment("Users: SignIn call");

            var user = _userService.SignIn(vm.Email, vm.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var userModel = new ResponseUserModel(user, _tokenService.GetToken(user));
            
            AWSXRayRecorder.Instance.EndSubsegment();

            return Ok(userModel);

        }

        // GET api/users/GetByEmail/a@a.com
        [HttpGet("[action]/{email}")]
        public IActionResult GetByEmail([FromRoute] string email)
        {
            AWSXRayRecorder.Instance.BeginSubsegment("Users: GetByEmail call");
            var user = _userService.GetByEmail(email);
            AWSXRayRecorder.Instance.EndSubsegment();

            return Ok(user);
        }

        // GET api/users/GetById/5
        [HttpGet("[action]/{userId}")]
        public IActionResult GetById([FromRoute] int userId)
        {
            AWSXRayRecorder.Instance.BeginSubsegment("Users: GetById call");
            var user = _userService.GetById(userId);
            AWSXRayRecorder.Instance.EndSubsegment();

            return Ok(user);
        }

        // GET api/users/confirmation
        [AllowAnonymous]
        [HttpGet("[action]/{email}/{code}")]
        public async Task<IActionResult> Confirmation([FromRoute]string email, [FromRoute]string code)
        {
            AWSXRayRecorder.Instance.BeginSubsegment("Users: GetById call");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(code))
                return BadRequest("No Activation Code provided!");

            var result = await _userService.Activate(email, code);

            AWSXRayRecorder.Instance.EndSubsegment();

            if (result)
                return Ok(result);

            return Ok(false);
        }

        [AllowAnonymous]
        [SkipJwtToken]
        [HttpGet("[action]/{email}")]
        public IActionResult SendActivationCode([FromRoute]string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("No Email provided!");

            AWSXRayRecorder.Instance.BeginSubsegment("Users: SendActivationCode call");

            _userService.ResendActivationCode(email);

            AWSXRayRecorder.Instance.EndSubsegment();

            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult ChangePassword([FromBody]ChangePasswordViewModel vm)
        {
            if (string.IsNullOrEmpty(vm.Email) ||  string.IsNullOrEmpty(vm.OldPassword) || (string.IsNullOrEmpty(vm.NewPassword)))
                return BadRequest("Password is empty!");

            AWSXRayRecorder.Instance.BeginSubsegment("Users: ChangePassword call");

            var result = _userService.ChangePassword(vm.Email, vm.OldPassword, vm.NewPassword);

            AWSXRayRecorder.Instance.EndSubsegment();

            return Ok(result);
        }

        [AllowAnonymous]
        [SkipJwtToken]
        [HttpGet("[action]/{email}")]
        public IActionResult PasswordRetrieval([FromRoute]string email)
        {
            AWSXRayRecorder.Instance.BeginSubsegment("Users: PasswordRetrieval call");

            if (string.IsNullOrEmpty(email))
                return BadRequest("No Email provided!");

            _userService.ResendPassword(email);

            AWSXRayRecorder.Instance.EndSubsegment();

            return Ok();
        }
    }
}
