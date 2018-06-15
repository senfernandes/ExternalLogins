using ExternalLogins.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace ExternalLogins.ViewComponents
{
    [ViewComponent(Name = "Login")]
    public class LoginViewComponent : ViewComponent
    {
        public ViewViewComponentResult Invoke()
        {
            var model = new LoginViewModel();

            return View(model);
        }
    }
}
