using Agenda.Models.Domain;

namespace CalendarEvents.Models
{
    public class CalendarEvent
    {
         public int Id { get; set; }
        public string LawyerId { get; set; }  = string.Empty;// Cambiado a string      
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string Color { get; set; } = string.Empty;
        public Boolean AllDay { get; set; }
        public string NameLawyer { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

         // Propiedad de navegación para representar la relación con ApplicationUser
        public virtual ApplicationUser? Lawyer { get; set; } 
    }
}