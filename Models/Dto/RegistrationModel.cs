using System.ComponentModel.DataAnnotations;

namespace Agenda.Models.Dto
{
    public class RegistrationModel
    {
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        public string? ProfilePicture { get; set; }
        public IFormFile? ImageFile { get; set; }

        public string DNI { get; set; } 

        public string Phone { get; set; } 

        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Longitud mínima 6 y debe contener 1 mayúscula, 1 minúscula, 1 carácter especial y 1 dígito.")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        public string Role { get; set; }

        public RegistrationModel()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Username = string.Empty;
            DNI = string.Empty;
            Phone = string.Empty;
            Password = string.Empty;
            PasswordConfirm = string.Empty;
            Role = string.Empty;
        }

    }
}
