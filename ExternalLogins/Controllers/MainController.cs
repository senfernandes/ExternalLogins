using ExternalLogins.Models;
using ExternalLogins.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExternalLogins.Controllers
{
    public class MainController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public MainController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginResults =
                    await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe,
                            lockoutOnFailure: false);
                if (loginResults.Succeeded)
                {
                    return RedirectToAction("Index", "LoggedIn");

                }
                ModelState.AddModelError(string.Empty, "Invalid Login Information.");
                return View(model);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Main/Login")]
        public IActionResult LoginApi([FromBody]LoginViewModel model)
        {
            var loginResults = _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe,
                        lockoutOnFailure: false).Result;
            if (loginResults.Succeeded)
            {

                return Ok("True");
            }
            return BadRequest("False");

        }
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var identityUser = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Username

                };

                var identityResults = await _userManager.CreateAsync(identityUser, model.Password);
                if (identityResults.Succeeded)
                {

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);

                    var callbackUrl = Url.Action("ConfirmEmail", "Main", new { userId = identityUser.Id, code = code },
                        protocol: HttpContext.Request.Scheme);

                    await _signInManager.SignInAsync(identityUser, isPersistent: false);
                    return View(model);
                    ;
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "Error in Creating my User.");
                    return View(model);
                }

            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return View("Error");

            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View("ConfirmEmail");

        }

        public IActionResult ResetPassword()
        {

            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByEmailAsync(model.Email);

        //        if (user == null)
        //        {


        //            return RedirectToAction("Index", "Main");
        //        }

        //        var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
        //        if (!result.Succeeded) return View(model);
        //        await _signInManager.SignInAsync(user, isPersistent: false);
        //        return RedirectToAction("Index", "LoggedIn");
        //    }

        //    return View(model);

        //}

        public IActionResult ForgotPassword()
        {

            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByEmailAsync(model.Email);
        //        var confirmed = await _userManager.IsEmailConfirmedAsync(user);
        //        if (user == null || !confirmed)
        //        {

        //            return View("ForgotPasswordConfirmation");
        //        }
        //        // Send Email Confirmation

        //        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        //        var callbackUrl = Url.Action("ResetPassword", "Main", new { userId = user.Id, code = code },
        //              protocol: HttpContext.Request.Scheme);
        //        await
        //            _emailSend.SendEmailAsync(model.Email, "Reset Password",
        //                $"Please Reset you Password by " +
        //                $"clicking this link:<a href='{callbackUrl}'>Link</a>");


        //    }

        //    return View(model);

        //}

        //public IActionResult ForgotPasswordConfirmation()
        //{

        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {

            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Main");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Main", new { returnUrl = "~/LoggedIn/Index" });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }


        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                return View(nameof(ExternalLoginFailure));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return RedirectToAction(nameof(ExternalLoginFailure));
            }

            var result =
                await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            else
            {
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLoginConfirmation",
                    new ExternalLoginConfirmationViewModel { Email = email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model,
            string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

                var alreadyRegistered = await _userManager.FindByEmailAsync(model.Email);
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded || alreadyRegistered.UserName != null)

                {
                    result = await _userManager.AddLoginAsync(user, info);

                    if (result.Succeeded || alreadyRegistered.UserName != null)
                    {

                        await _signInManager.SignInAsync(user, isPersistent: false);

                        RedirectToAction("Index", "LoggedIn");
                    }
                }
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        public IActionResult ExternalLoginFailure()
        {
            return View();
        }
    }
}
