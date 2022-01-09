using Microsoft.AspNetCore.Identity;

namespace Fiorello.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Fullname { get; set; }
        public bool IsActivated { get; set; } = true;
    }
}
