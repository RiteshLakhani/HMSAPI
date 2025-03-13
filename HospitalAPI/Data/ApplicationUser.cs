using Microsoft.AspNetCore.Identity;

namespace HospitalAPI.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
