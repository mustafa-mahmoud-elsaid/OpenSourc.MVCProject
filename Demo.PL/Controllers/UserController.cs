using Microsoft.AspNetCore.Authorization;

namespace Demo.PL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UserController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                var users = await _userManager.Users.Select(user => new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult(),
                }).ToListAsync();

                return View(users);
            }
            var user = await _userManager.FindByEmailAsync(email.Trim());
            if (user is null) return View(Enumerable.Empty<UserViewModel>());
            var model = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Roles = await _userManager.GetRolesAsync(user)
            };

            return View(new List<UserViewModel> { model });
        }

        public async Task<IActionResult> Details(string? id, string ViewName = nameof(Details))
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();
            var model = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
            };

            return View(ViewName, model);
        }
        public async Task<IActionResult> Edit(string? id) => await Details(id, nameof(Edit));
        [HttpPost]
        public async Task<IActionResult> Edit(string? id, UserViewModel model)
        {
            if (id != model.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is null) return NotFound();
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded) return RedirectToAction(nameof(Index));
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
        public async Task<IActionResult> Delete(string? id) => await Details(id, nameof(Delete));
        [ActionName("Delete")]
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string? id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();
            try
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded) return RedirectToAction(nameof(Index));
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            var model = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
            };
            return View(model);
        }

    }
}
