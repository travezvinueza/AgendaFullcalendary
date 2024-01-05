using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Agenda.Service.Abstract;
using Agenda.Models.Dto;

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

        //metodo para el boton eliminar
         [HttpPost]
        public IActionResult DeleteUser(string username)
        {
            // Eliminar el usuario y redirigir a la acción Index
            var status = _userService.DeleteUser(username);

            if (status.StatusCode == 1)
            {
                TempData["SuccessMessage"] = status.Message;
            }
            else
            {
                TempData["ErrorMessage"] = status.Message;
            }

            return RedirectToAction("Display");
        }
        
        // Método para editar un usuario
        [HttpGet]
        public async Task<IActionResult> EditUser(string username)
        {
            var editUserModel = await _userService.GetEditUserModelAsync(username);

            if (editUserModel == null)
            {
                return NotFound();
            }

            return View(editUserModel);
        }

        // Método para manejar la actualización de usuario
        [HttpPost]
        public async Task<IActionResult> UpdateUser(EditUserModel model)
        {
            var status = await _userService.UpdateUserAsync(model);

            if (status.StatusCode == 1)
            {
                TempData["SuccessMessage"] = status.Message;
            }
            else
            {
                TempData["ErrorMessage"] = status.Message;
            }

            return RedirectToAction("Display");
        }
    }
}