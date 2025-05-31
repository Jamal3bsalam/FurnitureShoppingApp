using FurnitureApp.Core.Dtos.AccountDtos;
using FurnitureApp.Core.Entities.Identity;
using FurnitureApp.Core.Services.Contract.AccountService;
using FurnitureApp.PL.Response.Error;
using FurnitureApp.PL.Response.GeneralResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureApp.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(IUserService userService,UserManager<AppUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<UserDto>>> LogIn(LoginDto loginDto)
        {
            var user = await _userService.LogInAsync(loginDto);
            if (user == null) return BadRequest(new ErrorResponse(400));
            return Ok(new ApiResponse<UserDto>(true,200, "User logged in successfully",user));
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Register(RegisterDto registerDto)
        {
            var user = await _userService.RegisterAsync(registerDto);
            if (user == null) return BadRequest(new ErrorResponse(400));
            return Ok(new ApiResponse<UserDto>(true, 200, "Registration completed successfully", user));
        }
    }
}
