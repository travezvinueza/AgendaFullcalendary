using Agenda.Models.Domain;
using Agenda.Models.Dto;
using Agenda.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace Agenda.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileService _fileService;
        public UserAuthenticationController(
               IUserAuthenticationService authService,
               UserManager<ApplicationUser> userManager,
               IFileService fileService)
        {
            this._authService = authService;
            this._userManager = userManager;
            this._fileService = fileService;
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

                var isAdmin = await _userManager.IsInRoleAsync(user!, "admin");
                Console.WriteLine($"Usuario es admin: {isAdmin}");

                Console.WriteLine($"Roles del usuario: {string.Join(", ", await _userManager.GetRolesAsync(user!))}");

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
        public async Task<IActionResult> Registration(RegistrationModel model, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // llamo al servicio de archivos para guardar la imagen
            var imageResult = _fileService.SaveImage(imageFile);

            if (imageResult.Item1 == 1) // 1 indica que la carga de la imagen fue exitosa
            {
                // Asignar la ruta o nombre de archivo de la imagen de perfil al modelo
                model.ProfilePicture = imageResult.Item2;

                model.Role = "user";
                var result = await _authService.RegisterAsync(model);

                TempData["msg"] = result.Message;

                if (result.StatusCode == 1)
                {
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["msg"] = imageResult.Item2; // Mensaje de error de carga de imagen
            }
            return RedirectToAction(nameof(Registration));
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this._authService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        //este metodo es para registrar un usuario admin con esta url
        //http://localhost:5133/UserAuthentication/RegisterAdmin
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