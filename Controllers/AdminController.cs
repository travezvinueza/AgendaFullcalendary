using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Agenda.Service.Abstract;

namespace Agenda.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IUserAuthenticationService _userService;

        public AdminController(IUserAuthenticationService userService)
        {
            _userService = userService;
        }
        //este metodo es para mostrar la lista de los usuarios
        public IActionResult Display()
        {
            var users = _userService.GetAll();
            return View(users);
        }
    }
}