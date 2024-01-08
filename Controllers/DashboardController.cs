using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Agenda.Models.Dto;
using Agenda.Service.Abstract;
using System.Security.Claims;

namespace Agenda.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IUserAuthenticationService _authService;

        public DashboardController(IUserAuthenticationService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> Display()
        {
            // Obtener el ID del usuario actual
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var profileModel = await _authService.GetProfileAsync(userId!);

            if (profileModel == null)
            {
                return View("Error");
            }
            return View(profileModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileModel model)
        {
            // Obtener el ID del usuario actual 
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var status = await _authService.UpdateProfileAsync(userId!, model);

            if (status.StatusCode == 1)
            {
                return RedirectToAction("Display", "Calendario");
            }
            else
            {
                ModelState.AddModelError(string.Empty, status.Message);
                return View(model);
            }
        }
    }
}