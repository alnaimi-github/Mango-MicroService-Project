using Mango.Web.Models;
using Mango.Web.Models.AuthDto;

namespace Mango.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto?>LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto?>RegisterAsync(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?>AssignRoleAsync(RegistrationRequestDto registrationRequestDto);
    }
}
