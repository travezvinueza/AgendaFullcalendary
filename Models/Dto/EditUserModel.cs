using System.ComponentModel.DataAnnotations;

namespace Agenda.Models.Dto
{
    public class EditUserModel
    {
        public string? ProfilePicture { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string DNI { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;
    }
}
