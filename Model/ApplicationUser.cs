using Microsoft.AspNetCore.Identity;

namespace TaskFlowAPI.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
