using CalendarEvents.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Agenda.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public string DNI { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        // Colección de eventos relacionados con este usuario
        [JsonIgnore]
        public virtual ICollection<CalendarEvent> CalendarEvents { get; set; }

        public ApplicationUser()
        {
            CalendarEvents = new List<CalendarEvent>(); // Inicialización de la colección
        }
    }
}