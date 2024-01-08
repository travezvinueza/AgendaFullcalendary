using Agenda.Models.Domain;
using Microsoft.EntityFrameworkCore;


namespace CalendarEvents.Models
{
    public class CalendarEventRepository : ICalendarEventRepository
    {
        private readonly DatabaseContext _context;

        public CalendarEventRepository(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<CalendarEvent> GetAll()
        {
            return _context.CalendarEvents.Include(e => e.Lawyer).ToList();
        }

        public CalendarEvent GetById(int id)
        {
            return _context.CalendarEvents.Find(id);
        }

        public void Add(CalendarEvent calendarEvent)
        {
            try
            {
                calendarEvent.DateCreated = DateTime.Now;
                calendarEvent.DateModified = DateTime.Now;
                _context.CalendarEvents.Add(calendarEvent);
                _context.SaveChanges();
                Console.WriteLine("Evento agregado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar evento: {ex.Message}");
                // Puedes lanzar la excepción nuevamente si lo deseas
                throw;
            }
        }

        // Similar para Update y Delete


        public void Update(CalendarEvent calendarEvent)
        {
            calendarEvent.DateModified = DateTime.Now;
            _context.CalendarEvents.Update(calendarEvent);
            _context.SaveChanges();
        }

        public void Delete(CalendarEvent calendarEvent)
        {
            var existingEvent = _context.CalendarEvents.Find(calendarEvent.Id);

            if (existingEvent != null)
            {
                _context.CalendarEvents.Remove(existingEvent);
                _context.SaveChanges();
            }

        }

        public CalendarEvent GetByIdAndLawyerId(int id, string lawyerId)
        {
            // Aquí debes implementar la lógica para recuperar un evento por Id y LawyerId desde tu base de datos
            return _context.CalendarEvents.FirstOrDefault(e => e.Id == id && e.LawyerId == lawyerId);
        }

    }
}
