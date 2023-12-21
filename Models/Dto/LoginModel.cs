using System.ComponentModel.DataAnnotations;

namespace Agenda.Models.Dto
{

        public class LoginModel
        {
                [Required]
                public string Username { get; set; }
                [Required]
                public string Password { get; set; }

                public LoginModel()
                {
                        Username = string.Empty;
                        Password = string.Empty;
                }
        }
}