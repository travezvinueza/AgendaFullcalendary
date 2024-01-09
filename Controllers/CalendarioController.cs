using Agenda.Models.Domain;
using CalendarEvents.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.Controllers
{
    [Authorize]
    public class CalendarioController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DatabaseContext _dbContext;
        private readonly ICalendarEventRepository _eventRepository;
        public CalendarioController(
            UserManager<ApplicationUser> userManager,
            ICalendarEventRepository eventRepository,
            DatabaseContext dbContext)

        {
            _userManager = userManager;
            _eventRepository = eventRepository;
            _dbContext = dbContext;
        }

        //mostrar sweet alert
        [AllowAnonymous]
        public new IActionResult Unauthorized()
        {
            ViewBag.Message = TempData["UnauthorizedMessage"]?.ToString();
            return View();
        }

        public IActionResult Display()
        {
            return View();
        }

        // Esta acción retornará los eventos en formato JSON para ser consumidos por FullCalendar
        public IActionResult GetEvents()
        {
            var events = _eventRepository.GetAll();

            // Mapear los eventos a un formato que incluya datos del usuario
            var eventsWithLawyerEmail = events.Select(e => new
            {
                id = e.Id,
                title = $"{e.Title} - {e.Lawyer?.Email}",
                description = e.Description,
                start = e.Start,
                end = e.End,
                color = e.Color,
                allDay = e.AllDay,
                lawyerId = e.LawyerId,
                nameLawyer = e.NameLawyer,
                lawyerEmail = e.Lawyer?.Email,
                phone = e.Lawyer?.Phone,
                profilePicture = e.Lawyer?.ProfilePicture,


            });

            return Json(eventsWithLawyerEmail);
        }

        // Acción para mostrar el formulario de edición
        [HttpGet]
        public async Task<IActionResult> EditAsync(string lawyerId, int id)
        {
            var eventToEdit = _eventRepository.GetByIdAndLawyerId(id, lawyerId);

            if (eventToEdit == null)
            {
                return NotFound();
            }
            // Verifica si el usuario actual es el propietario del evento
            var currentUser = await _userManager.GetUserAsync(User);
            if (eventToEdit.LawyerId != currentUser!.Id)
            {
                // Usuario no autorizado
                TempData["UnauthorizedMessage"] = "No estás autorizado para ver este evento.";
                return RedirectToAction("Unauthorized");
            }

            return View(eventToEdit);
        }

        // Acción para procesar el formulario de edición
        [HttpPost]
        public async Task<IActionResult> EditAsync(CalendarEvent editedEvent)
        {
            if (ModelState.IsValid)
            {
                // Verifica si el usuario actual es el propietario del evento
                var currentUser = await _userManager.GetUserAsync(User);
                if (editedEvent.LawyerId != currentUser!.Id)
                {
                    // Usuario no autorizado
                    TempData["UnauthorizedMessage"] = "No estás autorizado para ver este evento.";
                    return RedirectToAction("Unauthorized");
                }

                _eventRepository.Update(editedEvent);

                return RedirectToAction("Display");
            }
            return View(editedEvent);
        }

        // Metodo para que me traiga los detalles del evento
        // y asi poder eliminarlo con el siguiente metodo
        [HttpGet]
        public async Task<IActionResult> DetailsAsync(string lawyerId, int id)
        {
            var eventDetailsModel = _eventRepository.GetByIdAndLawyerId(id, lawyerId);

            if (eventDetailsModel == null)
            {
                TempData["UnauthorizedMessage"] = "No estás autorizado para ver este evento.";
                return RedirectToAction("Unauthorized");
            }
            // Verifica si el usuario actual es el propietario del evento
            var currentUser = await _userManager.GetUserAsync(User);
            if (eventDetailsModel.LawyerId != currentUser!.Id)
            {
                TempData["UnauthorizedMessage"] = "No estás autorizado para ver este evento.";
                return RedirectToAction("Unauthorized");
            }

            return View(eventDetailsModel);
        }

        // Metodo para eliminar
        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> DeleteAsync(string lawyerId, int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var eventToDelete = _eventRepository.GetByIdAndLawyerId(id, lawyerId);

            if (eventToDelete == null)
            {
                TempData["UnauthorizedMessage"] = "No estás autorizado para ver este evento.";
                return RedirectToAction("Unauthorized");
            }

            if (await _userManager.IsInRoleAsync(currentUser!, "Admin") || eventToDelete.LawyerId == currentUser!.Id)
            {
                _eventRepository.Delete(eventToDelete);
                return RedirectToAction("Display");
            }
            else
            {
                TempData["UnauthorizedMessage"] = "No estás autorizado para ver este evento.";
                return RedirectToAction("Unauthorized");
            }
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
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CalendarEvent calendarEvent)
        {
            Console.WriteLine("Entró al método Create");
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = await _userManager.GetUserAsync(User);
                    using (var dbContext = _dbContext)
                    {
                        var nuevoEvento = new CalendarEvent
                        {
                            LawyerId = currentUser!.Id,
                            NameLawyer = currentUser!.UserName!,
                            Title = calendarEvent.Title,
                            Start = calendarEvent.Start,
                            End = calendarEvent.End,
                            Description = calendarEvent.Description,
                            AllDay = calendarEvent.AllDay,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now
                        };

                        dbContext.CalendarEvents.Add(nuevoEvento);
                        await dbContext.SaveChangesAsync();

                        return RedirectToAction("Display");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al crear el evento: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                    ModelState.AddModelError(string.Empty, $"Error al crear el evento: {ex.Message}");
                    return View(calendarEvent);
                }
            }
            return View(calendarEvent);
        }
    }
}