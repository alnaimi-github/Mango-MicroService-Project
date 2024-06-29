using Microsoft.Build.Framework;

namespace Mango.Web.Models.AuthDto
{
    public class RegistrationRequestDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string? Role { get; set; }
        
    }
}
