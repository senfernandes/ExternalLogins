using ExternalLogins.Models;
using ExternalLogins.Repository;
using ExternalLogins.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExternalLogins.Controllers
{
    [Authorize]
    public class LoggedInController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProfileRepository _profileRepository;

        public LoggedInController(UserManager<ApplicationUser> userManager, IProfileRepository profileRepository)
        {
            _userManager = userManager;
            _profileRepository = profileRepository;
        }

        
        public async Task<IActionResult> Index()
        {
            var userId = await _userManager.GetUserAsync(HttpContext.User);
            var individual = _profileRepository.GetIndividualList(userId.Id);

            var model = new DashboardViewModel
            {
                Individuals = individual
            };

            return View(model);
        }

    }
}
