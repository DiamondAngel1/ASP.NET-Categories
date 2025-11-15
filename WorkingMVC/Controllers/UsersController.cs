using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkingMVC.Data.Entitys.Identity;
using WorkingMVC.Interfaces;
using WorkingMVC.Models.Users;

namespace WorkingMVC.Controllers
{
    public class UsersController(
        IUserServices userService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var model = await userService.GetUsersAsync();
            return View(model);
        }
        // GET: /Users/Edit/2
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await userService.EditRoleAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserRolesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await userService.UpdateRolesAsync(model);
            if (!success)
            {
                ModelState.AddModelError("", "Не вдалося оновити ролі.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

    }
}

