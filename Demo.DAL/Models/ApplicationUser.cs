global using Microsoft.AspNetCore.Identity;

namespace Demo.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
