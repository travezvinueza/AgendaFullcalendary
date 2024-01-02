using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Agenda.ViewModels;
using Agenda.Models.Domain;

namespace Agenda.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Display()
        {
            var username = User.Identity.Name;
            var user = _userManager.FindByNameAsync(username).Result;

            var viewModel = new DashboardViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                ProfilePicture = user.ProfilePicture
            };

            return View(viewModel);
        }
    }
}