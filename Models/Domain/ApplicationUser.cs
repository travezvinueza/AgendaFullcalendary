using Microsoft.AspNetCore.Identity;

namespace Agenda.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public string DNI { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

    }
}