using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkingMVC.Data.Entitys.Identity;
using WorkingMVC.Interfaces;
using WorkingMVC.Models.Account;
using WorkingMVC.Services;

namespace WorkingMVC.Controllers
{
    public class AccountController(
        UserManager<UserEntity> userManager, //сервіс Identity для роботи з юзерами
        IImageService imageService, //сервіс для роботи з зображеннями
        IMapper mapper) : Controller //AutoMapper для мапінгу моделей RegisterViewModel в UserEntity
    {
        [HttpGet]
        public IActionResult Register()
        {
            //Повертаємо порожню форму реєстрації
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            //Перевіряємо валідність моделі, якщо ні - повертаємо форму з помилками
            if (!ModelState.IsValid) {
                return View(model);
            }
            //Мапінг моделі реєстрації в модель користувача
            var user = mapper.Map<UserEntity>(model);
            //Якщо користувач завантажив фото, то завантажуємо його через сервіс зображень
            var imageStr = model.Image is not null
                ? await imageService.UploadImageAsync(model.Image) : null;
            user.Image = imageStr;
            //Створюємо користувача через Identity сервіс
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //якщо все успішно, перенаправляємо на головну сторінку
                return RedirectToAction("Index", "Main");
            }
            else
            {
                //Якщо є помилки, додаємо їх в ModelState і повертаємо форму з помилками
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                //Повертаємо форму з помилкамиі
                return View(model);
            }

        }
    }
}
