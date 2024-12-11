global using Microsoft.AspNetCore.Identity;

namespace Demo.PL.Controllers
{
    public class AccountController
        (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
            };


            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded) return RedirectToAction(nameof(Login));

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is not null)
            {
                if (await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var result = await signInManager.
                        PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded) return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).ControllerName());


                }
            }
            ModelState.AddModelError(string.Empty, "Invalid Email Or Password");
            return View(model);


        }

        public new async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {

            if (!ModelState.IsValid) return View(model);
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is not null)
            {
                // Create Password Reset Token
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                // Create Password Reset URL 
                var url = Url.Action(nameof(ResetPassword), nameof(AccountController).ControllerName(),
                    new { Email = user.Email, token = token }, Request.Scheme);
                // Create Email Object 
                var email = new Email
                {
                    Subject = "Password Reset",
                    Body = url,
                    Recipient = user.Email!
                };

                //Send Email
                MailSettings.SendEmail(email);
                return View("CheckYourInBox");
            }
            ModelState.AddModelError(string.Empty, "User Not Found");

            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (email == null || token == null) return BadRequest();

            TempData["Email"] = email;
            TempData["Token"] = token;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {

            model.Email = TempData["Email"]?.ToString() ?? string.Empty;
            model.Token = TempData["Token"]?.ToString() ?? string.Empty;

            if (!ModelState.IsValid) return View(model);
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded) return RedirectToAction(nameof(Login));

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }

            ModelState.AddModelError(string.Empty, "User Not Found");

            return View();
        }



    }
}
// P@ssw0rd

// Pa$$w0rd