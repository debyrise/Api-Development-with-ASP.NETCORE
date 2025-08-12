using Microsoft.AspNetCore.Identity;

namespace WebApiDemo.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
