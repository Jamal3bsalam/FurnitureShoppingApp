using FurnitureApp.Core.Dtos.AccountDtos;
using FurnitureApp.Core.Dtos.ProfileDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Services.Contract.Profile
{
    public interface IProfileService
    {
        Task<CurrentUserDto> UpdateUserProfile(UpdateProfileDto updateProfileDto);
        public string Upload(ProfileImageDto profileImageDto);
        public void Delete(string fileName);
    }
}
