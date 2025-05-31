using FurnitureApp.Core.Dtos.AccountDtos;
using FurnitureApp.Core.Dtos.ProfileDtos;
using FurnitureApp.Core.Entities.Identity;
using FurnitureApp.Core.Services.Contract.Profile;
using FurnitureApp.Repository.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Service.Services.ProfileService
{
    public class ProfileService : IProfileService
    {
        private readonly FurnitureDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileService(FurnitureDbContext context , UserManager<AppUser> userManager,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CurrentUserDto> UpdateUserProfile(UpdateProfileDto updateProfileDto)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (userId == null) return null;
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return null;

                user.FullName = updateProfileDto.FullName;
                user.Email = updateProfileDto.Email;
                user.NormalizedEmail = updateProfileDto.Email.ToUpper();

                _userManager.UpdateAsync(user);

                var userDto = new CurrentUserDto()
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    UserName = user.UserName,
                    ProfileImage = user.ProfileImage
                };
                return userDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while updating profile: {ex.Message}");
                return null;
            }
        }

        public string Upload(ProfileImageDto profileImageDto)
        {
            //D:\FurnitureApp\FurnitureShoppingApp\FurnitureApp.PL\wwwroot\Images\ProfileImage\Gamal.jpg
            string currentDirectory = Directory.GetCurrentDirectory();
            string folderPath = (currentDirectory + "\\wwwroot\\Images\\ProfileImage");

            string fileName = profileImageDto.File.FileName;

            string filePath = Path.Combine(folderPath, fileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            profileImageDto.File.CopyTo(fileStream);

            return fileName;
        }
        public void Delete(string fileName)
        {
            string CurrentDirectory = Directory.GetCurrentDirectory();
            string filePath = (CurrentDirectory + $"\\wwwroot\\{fileName}");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
