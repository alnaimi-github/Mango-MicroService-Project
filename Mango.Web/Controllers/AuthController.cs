using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using Mango.Web.Models;
using Mango.Web.Models.AuthDto;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        private readonly INotyfService _notyf;

        public AuthController(
            IAuthService authService,
            INotyfService notyfService,
            ITokenProvider tokenProvider)
        {
            _authService = authService;
            _notyf = notyfService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {
            var responseDto =await _authService.LoginAsync(obj);
            if(responseDto != null && responseDto.IsSuccess)
            {
                var loginResponseDto= JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result)!)!;
                _tokenProvider.SetToken(loginResponseDto.Token);
                await SignInUser(loginResponseDto);
                return RedirectToAction("Index","Home");
            }
            else
            {
                ModelState.AddModelError("CustomerError", responseDto!.Message);
                return View(obj);
            }
        }
        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text = SD.RoleAdmin,Value = SD.RoleAdmin},
                new SelectListItem{Text = SD.RoleCustomer,Value = SD.RoleCustomer}
            };
            ViewBag.RoleList= roleList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationRequestDto obj)
        {
            ResponseDto? result = await _authService!.RegisterAsync(obj);
            ResponseDto? assignRole;
            if (result!=null&&result.IsSuccess)
            {
                if (string.IsNullOrEmpty(obj.Role))
                {
                    obj.Role= SD.RoleCustomer;
                }

                assignRole = await _authService.AssignRoleAsync(obj)!;
                if (assignRole != null&&assignRole.IsSuccess)
                {
                    _notyf.Success("Registration Successful");
                }

                return RedirectToAction(nameof(Login));
            }
            else
            {
                ModelState.AddModelError("Error",result!.Message);
            }
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text = SD.RoleAdmin,Value = SD.RoleAdmin},
                new SelectListItem{Text = SD.RoleCustomer,Value = SD.RoleCustomer}
            };
            ViewBag.RoleList = roleList;
            return View(obj);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index","Home");
        }

        public async Task SignInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token);

            var claims = jwt.Claims.Select(c => new Claim(c.Type, c.Value)).ToList();
            claims.Add(new Claim(
                ClaimTypes.Name, 
                jwt.Claims.FirstOrDefault(x => x.Type == "name")?.Value!)); // Add 'name' claim

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

    }
}
