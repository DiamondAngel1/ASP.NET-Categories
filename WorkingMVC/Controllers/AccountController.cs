using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        SignInManager<UserEntity> signInManager, //сервіс Identity для роботи з авторизацією
        IMapper mapper,
        WorkingMVC.Interfaces.IEmailSender emailSender) : Controller //AutoMapper для мапінгу моделей RegisterViewModel в UserEntity
    {
        [HttpGet]
        public IActionResult Login()
        {
            //Повертаємо порожню форму реєстрації
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //Пробуємо авторизувати користувача через SignInManager
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null) {
                var result = await signInManager
                    .PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Main");
                }
            }
            ModelState.AddModelError("", "Невірний логін або пароль");
            return View(model);
        }
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
                await signInManager.SignInAsync(user,false);
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

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            //Вихід користувача з системи
            await signInManager.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Користувача не знайдено");
                return View(model);
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

            var html = $"""
        <p>Привіт, {user.FirstName}!</p>
        <p>Щоб змінити пароль, перейдіть за <a href="{callbackUrl}">цим посиланням</a>.</p>
        <p>Якщо ви не запитували зміну пароля — просто ігноруйте цей лист.</p>
            
    """
            ;

            await emailSender.SendEmailAsync(user.Email, "Скидання пароля", html);

            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            return View(new ResetPasswordViewModel { Token = token, Email = email });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Користувача не знайдено");
                return View(model);
            }

            var result = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var model = new UserProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Image = user.Image
            };

            return View(model);
        }



    }
}
