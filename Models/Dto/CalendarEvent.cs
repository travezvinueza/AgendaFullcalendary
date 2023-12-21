// using System.ComponentModel.DataAnnotations.Schema;
// using System.ComponentModel.DataAnnotations;
namespace CalendarEvents.Models
{
    public class CalendarEvent
    {
         public int Id { get; set; }
        public int LawyerId { get; set; }       
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string Color { get; set; } = string.Empty;
        public Boolean AllDay { get; set; }
        public string NameLawyer { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

    }
}