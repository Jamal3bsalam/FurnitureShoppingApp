using FurnitureApp.Core.Dtos.AccountDtos;
using FurnitureApp.Core.Dtos.ProfileDtos;
using FurnitureApp.Core.Entities.Identity;
using FurnitureApp.Core.Services.Contract.Profile;
using FurnitureApp.PL.Response.Error;
using FurnitureApp.PL.Response.GeneralResponse;
using FurnitureApp.Repository.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace FurnitureApp.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly UserManager<AppUser> _userManager;
        private readonly FurnitureDbContext _context;
        private readonly IConfiguration _configuration;

        public ProfileController(IProfileService profileService,UserManager<AppUser> userManager,FurnitureDbContext context,IConfiguration configuration)
        {
            _profileService = profileService;
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("currentUser")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse<CurrentUserDto>>> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return BadRequest();

            //var user = await _userManager.FindByEmailAsync(userEmail);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return BadRequest(new ErrorResponse(400));
           var currentUserDto = new CurrentUserDto()
                 {
                     FullName = user.FullName,
                     UserName = user.UserName,
                     Email = user.Email,
                     ProfileImage = _configuration["BASEURL"] + user.ProfileImage,
                 };

            return Ok(new ApiResponse<CurrentUserDto>(true,200,"Current user retrieved successfully.",currentUserDto));
        }

        [HttpPut("updateProfile")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse<CurrentUserDto>>> UpdateProfiel(UpdateProfileDto updateProfileDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return BadRequest(new ErrorResponse(400));

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound(new ErrorResponse(404));

            var result = await _profileService.UpdateUserProfile(updateProfileDto);
            if (result is null) return BadRequest(new ErrorResponse(400));
            return Ok(new ApiResponse<CurrentUserDto>(true,200, "Profile Updated Successfully",result));

        }

        [HttpPost("uploadProfileImage")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse<string>>> UploadProfileImage(ProfileImageDto profileImageDto)
        {
            if (profileImageDto.File == null || profileImageDto.File.Length == 0) return BadRequest(new ErrorResponse(400,"No File Uploaded"));
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ErrorResponse(401));

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound(new ErrorResponse(404));

            if (!string.IsNullOrEmpty(user.ProfileImage)) return BadRequest("Invalid Operation");
            string fileName = _profileService.Upload(profileImageDto);

            user.ProfileImage = $"Images\\ProfileImage\\{fileName}";
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<string>(true,200, "ProfileIamge Updated Successfully",null));
        }

        [HttpDelete("DeleteProfileImage")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteProfileImage()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ErrorResponse(401));

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound(new ErrorResponse(404));

            if (string.IsNullOrEmpty(user.ProfileImage)) return BadRequest( new ErrorResponse(400,"No Image Profile"));
            _profileService.Delete(user.ProfileImage);

            user.ProfileImage = null;
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<string>(true,200,"Image Deleted successfully",null));
        }

        [HttpPut("UpdateProfileImage")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateProfileImage(ProfileImageDto profileImageDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ErrorResponse(401));

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound(new ErrorResponse(404));

            if (!string.IsNullOrEmpty(user.ProfileImage))
            {
                _profileService.Delete(user.ProfileImage);
            }

            string file = _profileService.Upload(profileImageDto);
            if (file == null) return BadRequest(new ErrorResponse(400,"Faild To Upload Image"));

            user.ProfileImage = $"Images\\ProfileImage\\{file}";
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<string>(true,200,"Image Updated successfully",null));
        }
    }
}
