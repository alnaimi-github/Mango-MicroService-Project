using Mango.Service.AuthAPI.Models.Dto;
using Mango.Service.AuthAPI.Service.IService;
using Mango.Services.AuthAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Service.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _responseDto;

        public AuthApiController(IAuthService authService)
        {
            _authService = authService;
            _responseDto = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message=errorMessage;
                return BadRequest(_responseDto);
            }

            return Ok(_responseDto);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Invalid model state!";
                return BadRequest(_responseDto);
            }
            var loginResponse = await _authService.Login(model);
            if(loginResponse.User==null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Username or password is incorrect!";
                return BadRequest(_responseDto);
            }
            _responseDto.Result=loginResponse;
            return Ok(_responseDto);
        }
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(model.Email,model.Role!.ToUpper());
            if (!assignRoleSuccessful)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Error encountered";
                return BadRequest(_responseDto);
            }

            return Ok(_responseDto);
        }
    }
}
