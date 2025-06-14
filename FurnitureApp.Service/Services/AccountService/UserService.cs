using FurnitureApp.Core.Dtos.AccountDtos;
using FurnitureApp.Core.Entities.Identity;
using FurnitureApp.Core.Services.Contract.AccountService;
using FurnitureApp.Core.Services.Contract.Token;
using FurnitureApp.Repository.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Service.Services.AccountService
{
    public class UserService : IUserService
    {
        private readonly FurnitureDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;

        public UserService(FurnitureDbContext context,ITokenService tokenService,UserManager<AppUser> userManager , SignInManager<AppUser> signInManager,IConfiguration configuration)
        {
            _context = context;
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await CheckEmailExistAsync(registerDto.Email)) return null;

            var user = new AppUser()
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                UserName = registerDto.UserName
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return null;
            return new UserDto()
            {
                FullName = user.FullName,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user, _userManager)
            };
        }
        public async Task<UserDto> LogInAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return null;
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password , false);
            if (!result.Succeeded) return null;

            var userDto = new UserDto()
            {
                FullName = user.FullName,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user , _userManager)
            };

            return userDto;
        }

        public async Task<bool> CheckEmailExistAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
