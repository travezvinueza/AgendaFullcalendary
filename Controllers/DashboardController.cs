using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Agenda.Models.Domain;
using Agenda.Models.Dto;
using Agenda.Service.Abstract;

namespace Agenda.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserAuthenticationService _userService;

        public DashboardController(UserManager<ApplicationUser> userManager, IUserAuthenticationService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        public IActionResult Display()
        {
            return View();
        }
        //este metodo es para traer el usuario y y poder actualizarlo 
        //con el siguiente metodo
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            // Obtener el usuario por su Id
            var userModel = await _userService.GetById(id);

            if (userModel == null)
            {
                // Manejar el caso en el que el usuario no se pueda recuperar
                return RedirectToAction("Display");
            }
            return View(userModel);
        }
        //este es el suiguiente metodo que es para actualizarlo el usuario logueado
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(string userId, RegistrationModel model)
        {
            var status = new Status();

            // Obtener el usuario por su Id
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Usuario no encontrado";
                // Debes crear una vista para mostrar errores
                return View("ErrorView", status);
            }

            // Actualizar propiedades del usuario
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Username;
            user.ProfilePicture = model.ProfilePicture;

            // Actualizar el usuario en la base de datos
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "Error al actualizar el usuario";
                // Debes crear una vista para mostrar errores
                return View("ErrorView", status);
            }

            status.StatusCode = 1;
            status.Message = "Usuario actualizado exitosamente";
            return RedirectToAction("Display");
        }

    }
}
