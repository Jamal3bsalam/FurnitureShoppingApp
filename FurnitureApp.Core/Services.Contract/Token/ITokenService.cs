using FurnitureApp.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Services.Contract.Token
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user,UserManager<AppUser> userManager);
    }
}
