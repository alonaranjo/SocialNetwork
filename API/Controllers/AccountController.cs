using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using API.BussinesLogic.Services.IServices;

namespace API.Controllers
{
    public class AccountController: BaseApiController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
           return Ok(await _accountService.RegisterAsync(registerDto));
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            return Ok(await _accountService.LoginAsync(loginDto));
        }
    }
}