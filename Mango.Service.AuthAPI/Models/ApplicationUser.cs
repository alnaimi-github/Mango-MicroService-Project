using Microsoft.AspNetCore.Identity;

namespace Mango.Service.AuthAPI.Models
{
    public class ApplicationUser:IdentityUser
    {
       public string Name {get; set; }=string.Empty;
    }
}
