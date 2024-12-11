using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Demo.PL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole(model.Name);
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded) return RedirectToAction(nameof(Index));
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
        public async Task<IActionResult> Index(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                var roles = await _roleManager.Roles.Select(role => new RoleViewModel
                {
                    Id = role.Id,
                    Name = role.Name
                }).ToListAsync();

                return View(roles);
            }
            var role = await _roleManager.FindByNameAsync(name.Trim());
            if (role is null) return View(Enumerable.Empty<RoleViewModel>());
            var model = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };

            return View(new List<RoleViewModel> { model });
        }

        public async Task<IActionResult> Details(string? id, string ViewName = nameof(Details))
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound();
            var model = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };

            return View(ViewName, model);
        }
        public async Task<IActionResult> Edit(string? id) => await Details(id, nameof(Edit));
        [HttpPost]
        public async Task<IActionResult> Edit(string? id, RoleViewModel model)
        {
            if (id != model.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role is null) return NotFound();
                role.Name = model.Name;
                var result = await _roleManager.UpdateAsync(role);
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
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound();
            try
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded) return RedirectToAction(nameof(Index));
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            var model = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(model);
        }
        public async Task<IActionResult> AddOrRemoveUsers(string? roleId)
        {
            if (string.IsNullOrWhiteSpace(roleId)) return BadRequest();
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();
            ViewBag.RoleId = role.Id;
            var users = await _userManager.Users.Select(user => new UserInRoleViewModel
            {
                Name = user.UserName,
                UserId = user.Id,
                IsInRole = _userManager.IsInRoleAsync(user, role.Name).GetAwaiter().GetResult()
            }).ToListAsync();
            return View(users);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string? roleId, List<UserInRoleViewModel> users)
        {
            if (string.IsNullOrWhiteSpace(roleId)) return BadRequest();
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();

            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if (appUser is null) continue;
                    if (user.IsInRole && !await _userManager.IsInRoleAsync(appUser, role.Name))
                        await _userManager.AddToRoleAsync(appUser, role.Name);
                    if (!user.IsInRole && await _userManager.IsInRoleAsync(appUser, role.Name))
                        await _userManager.RemoveFromRoleAsync(appUser, role.Name);

                }
                return RedirectToAction(nameof(Edit), new { id = role.Id });
            }
            ViewBag.RoleId = role.Id;
            return View(users);
        }
    }
}
