using Agenda.Models.Domain;
using CalendarEvents.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.Controllers
{
    [Authorize]
    public class CalendarioController : Controller
    {
        private readonly DatabaseContext _dbContext;
        private readonly ICalendarEventRepository _eventRepository;
        public CalendarioController(ICalendarEventRepository eventRepository, DatabaseContext dbContext)
        {
            _eventRepository = eventRepository;
            _dbContext = dbContext;
        }

        public IActionResult Display()
        {
            return View();
        }

        // Esta acción retornará los eventos en formato JSON para ser consumidos por FullCalendar
        public IActionResult GetEvents()
        {
            var events = _eventRepository.GetAll();
            return Json(events);
        }

        // Acción para mostrar el formulario de edición
        [HttpGet]
        public IActionResult Edit(int lawyerId, int id)
        {
            var eventToEdit = _eventRepository.GetByIdAndLawyerId(id, lawyerId);

            if (eventToEdit == null)
            {
                return NotFound();
            }

            return View(eventToEdit);
        }

        // Acción para procesar el formulario de edición
        [HttpPost]
        public IActionResult Edit(CalendarEvent editedEvent)
        {
            if (ModelState.IsValid)
            {
                _eventRepository.Update(editedEvent);

                return RedirectToAction("Display");
            }

            return View(editedEvent);
        }

        // Metodo para que me traiga los detalles del evento
        // y asi poder eliminarlo con el siguiente metodo
        [HttpGet]
        public IActionResult Details(int lawyerId, int id)
        {
            var eventDetailsModel = _eventRepository.GetByIdAndLawyerId(id, lawyerId);

            if (eventDetailsModel == null)
            {
                return NotFound();
            }

            return View(eventDetailsModel);
        }

        // Metodo para eliminar
        [HttpPost]
        public IActionResult Delete(int lawyerId, int id)
        {

            var eventToDelete = _eventRepository.GetByIdAndLawyerId(id, lawyerId);

            if (eventToDelete == null)
            {

                return NotFound();
            }

            _eventRepository.Delete(eventToDelete);

            return RedirectToAction("Display");
        }

        // Metodo importante no borrar
        [HttpGet]
        public IActionResult Create(string eventDate)
        {
            ViewBag.EventDate = eventDate;
            var data = new CalendarEvent()
            {

            };

            return View(data);
        }

        // Metodo para crear un evento
        [HttpPost]
        public IActionResult Create(CalendarEvent calendarEvent)
        {
            if (ModelState.IsValid)
            {
                using (var dbContext = _dbContext)
                {
                    var nuevoEvento = new CalendarEvent
                    {
                        NameLawyer = calendarEvent.NameLawyer,
                        Title = calendarEvent.Title,
                        Start = calendarEvent.Start,
                        End = calendarEvent.End,
                        Description = calendarEvent.Description,
                        AllDay = calendarEvent.AllDay,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now
                        // ... otras propiedades según tu modelo
                    };

                    dbContext.CalendarEvents.Add(nuevoEvento);
                    dbContext.SaveChanges();

                    return RedirectToAction("Display");
                }
            }

            return View(calendarEvent);
        }
        

    }
}