using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using monitor_core.Dto;
using monitor_infra.Models.Response;
using monitor_infra.Services.Interfaces;
using monitorback.ViewModels;

namespace monitor_back.Controllers
{
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
        [HttpPost]
        [Route("[action]")]
        public IActionResult SignUp([FromBody]AddUserViewModel vm)
        {
            if (string.IsNullOrEmpty(vm.Email) || string.IsNullOrEmpty(vm.Email))
                return BadRequest(new { message = "input data is empty!" });

            var user = _userService.Add(vm.Email, vm.Password);

            if (user == null)
                return BadRequest(new { message = "User already exist!" });

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public IActionResult SignIn([FromBody]SignInViewModel vm)
        {
            var user = _userService.SignIn(vm.Email, vm.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var userModel = new ResponseUserModel(user, _tokenService.GetToken(user));

            return Ok(userModel);

        }

        // GET api/users/GetByEmail/a@a.com
        [HttpGet("[action]/{email}")]
        public IActionResult GetByEmail([FromRoute] string email)
        {
            var user = _userService.GetByEmail(email);
            return Ok(user);
        }

        // GET api/users/GetById/5
        [HttpGet("[action]/{userId}")]
        public IActionResult GetById([FromRoute] int userId)
        {
            var user = _userService.GetById(userId);
            return Ok(user);
        }

        // GET api/users/confirmation
        [AllowAnonymous]
        [HttpGet("[action]/{email}/{code}")]
        public async Task<IActionResult> Confirmation([FromRoute]string email, [FromRoute]string code)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(code))
                return BadRequest("No Activation Code provided!");

            var result = await _userService.Activate(email, code);
            if (result)
                return Ok(result);

            return Ok(false);
        }

        [AllowAnonymous]
        [HttpGet("[action]/{email}")]
        public IActionResult SendActivationCode([FromRoute]string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("No Email provided!");

            _userService.ResendActivationCode(email);
            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult ChangePassword([FromBody]ChangePasswordViewModel vm)
        {
            if (string.IsNullOrEmpty(vm.Email) ||  string.IsNullOrEmpty(vm.OldPassword) || (string.IsNullOrEmpty(vm.NewPassword)))
                return BadRequest("Password is empty!");

            var result = _userService.ChangePassword(vm.Email, vm.OldPassword, vm.NewPassword);
            return Ok(result);
        }
    }
}
