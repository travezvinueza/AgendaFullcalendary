using Agenda.Models.Domain;
using Agenda.Models.Dto;
using Agenda.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace Agenda.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserAuthenticationController(IUserAuthenticationService authService, UserManager<ApplicationUser> userManager)
        {
            this._authService = authService;
            this._userManager = userManager;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _authService.LoginAsync(model);
            if (result.StatusCode == 1)
            {
                // Si el inicio de sesión es exitoso
                var user = await _userManager.FindByNameAsync(model.Username);

                var isAdmin = await _userManager.IsInRoleAsync(user, "admin");
                Console.WriteLine($"Usuario es admin: {isAdmin}");

                Console.WriteLine($"Roles del usuario: {string.Join(", ", await _userManager.GetRolesAsync(user))}");

                if (isAdmin)
                {
                    // Si el usuario es administrador, redirigir a la vista de administrador
                    Console.WriteLine("Redirigiendo a la vista de administrador");
                    return RedirectToAction("Display", "Admin");
                }
                else
                {
                    // Si no es administrador, redirigir a la vista del dashboard
                    Console.WriteLine("Redirigiendo a la vista del dashboard");
                    return RedirectToAction("Display", "Dashboard");
                }
            }
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Login));
            }
        }


        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (!ModelState.IsValid) { return View(model); }

            model.Role = "user";
            var result = await this._authService.RegisterAsync(model);
            TempData["msg"] = result.Message;

            if (result.StatusCode == 1)
            {
                // Registro exitoso, redirige a la página de inicio de sesión
                return RedirectToAction(nameof(Login));
            }
            else
            {
                // Algo salió mal en el registro, permanece en la página de registro
                return RedirectToAction(nameof(Registration));
            }
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this._authService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdmin()
        {
            RegistrationModel model = new RegistrationModel
            {
                Username = "Mateo",
                Email = "mateo@gmail.com",
                FirstName = "Mateo",
                LastName = "Eras",
                Password = "Mateo.eras@123"
            };
            model.Role = "admin";

            var result = await this._authService.RegisterAsync(model);
            return Ok(result);
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _authService.ChangePasswordAsync(model, User.Identity.Name);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(ChangePassword));
        }

    }
}