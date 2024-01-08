
namespace CalendarEvents.Models
{
    public interface ICalendarEventRepository
    {
        IEnumerable<CalendarEvent> GetAll();
        CalendarEvent GetById(int id);
        void Add(CalendarEvent calendarEvent);
        void Update(CalendarEvent calendarEvent);
        void Delete(CalendarEvent calendarEvent);

         CalendarEvent GetByIdAndLawyerId(int id, string lawyerId);
    }
}
