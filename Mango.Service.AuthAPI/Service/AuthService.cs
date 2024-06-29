using System.Runtime.InteropServices.JavaScript;
using Mango.Service.AuthAPI.Data;
using Mango.Service.AuthAPI.Models;
using Mango.Service.AuthAPI.Models.Dto;
using Mango.Service.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mango.Service.AuthAPI.Service
{
    public class AuthService: IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(AppDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber,
            };
            try
            {
                var result=await _userManager.CreateAsync(user,registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = await _userManager.FindByEmailAsync(registrationRequestDto.Email);
                    UserDto userDto=  new ()
                    {
                        Id = userToReturn!.Id,
                        Email = userToReturn.Email!,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber!
                    };
                    return default!;
                }
                else
                {
                    return result.Errors.FirstOrDefault()!.Description;
                }
            }
            catch (Exception e)
            {
                
            }

            return "Error Encountered";
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(u =>
                u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            var isValid = await _userManager.CheckPasswordAsync(user!,loginRequestDto.Password);
            if (isValid is false || user is null)
            {
                return new LoginResponseDto(){User = default!,Token = default!};
            }

            //if user was found ,Generate token
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user,roles);
            UserDto userDto = new()
            {
                Id = user.Id,
                Email = user.Email!,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber!,

            };
            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token =token
            };
            return loginResponseDto;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(u =>
                u.Email!.ToLower() == email.ToLower());
            if (user is not null)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }

            return false;
        }
    }
}
