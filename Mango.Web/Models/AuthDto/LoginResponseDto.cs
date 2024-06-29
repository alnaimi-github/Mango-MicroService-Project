namespace Mango.Web.Models.AuthDto
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
