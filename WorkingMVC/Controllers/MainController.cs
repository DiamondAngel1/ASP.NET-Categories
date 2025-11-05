using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkingMVC.Data;
using WorkingMVC.Data.Entitys;
using WorkingMVC.Interfaces;
using WorkingMVC.Models.Category;

namespace WorkingMVC.Controllers
{
    public class MainController(
        MyAppDbContext myAppDbContext,
        IConfiguration configuration,
        IMapper mapper,
        IImageService imageService,
        IHiddenCategoryService hiddenService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            //отримується список прихованих категорій
            var hiddenIds = hiddenService.GetHiddenIds();
            //завантажуються всі категорії з бд, крім прихованих
            var categories = await myAppDbContext.Categories
                .Where(c => !hiddenIds.Contains(c.Id))
                .ToListAsync();

            var list = mapper.Map<List<CategoryItemModel>>(categories);
            return View(list);
        }

        //Для того, щоб побачити сторінку створення категорії
        [HttpGet]//Щоб побачити сторінку і внести інформацію про категорію
        public IActionResult Create() {
            return View();
        } 

        [HttpPost]//Збереження даних
        public async Task<IActionResult> Create(CategoyCreateModel model)
        {
            //перевіряється чи модель валідна
            if (!ModelState.IsValid)
            {
                return View(model); // якщо модель не валідна викидаємо дані назад,
                                    //Щоб користувач знав, що він невірно вніс
            }

            var name = model.Name.Trim().ToLower();
            var entity = myAppDbContext.Categories
                .FirstOrDefault(c => c.Name.ToLower() == name);
           
            if (entity != null)
            {
                ModelState.AddModelError("", "Категорія з такою назвою вже існує");
                return View(model);
            }

            entity = new CategoryEntity
            {
                Name = model.Name
            };
            
            if(model.Image != null)
            {
                entity.Image = await imageService.UploadImageAsync(model.Image);
            }

            myAppDbContext.Categories.Add(entity);
            await myAppDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //показує сторінку редагування категорії
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //якщо категорія прихована, то повертаємо 404
            if (hiddenService.IsHidden(id))
            {
                return NotFound();
            }

            //знаходимо категорію З бд
            var category = await myAppDbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            //створюємо модель для редагування і заповнюємо її даними
            var model = new CategoryEditModel
            {
                Id = category.Id,
                Name = category.Name,
                ExistingImage = category.Image
            };

            return View(model);
        }

        [HttpPost] //обробляє редагування категорії
        public async Task<IActionResult> Edit(CategoryEditModel model)
        {
            //перевіряється валідність моделі
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //якщо категорія прихована, повертаємо 404
            if (hiddenService.IsHidden(model.Id))
            {
                return NotFound();
            }

            var category = await myAppDbContext.Categories.FindAsync(model.Id);
            if (category == null)
            {
                return NotFound();
            }

            var normalizedName = model.Name.Trim().ToLower();
            var duplicate = await myAppDbContext.Categories
                .AnyAsync(c => c.Id != model.Id && c.Name.ToLower() == normalizedName);
            if (duplicate)
            {
                ModelState.AddModelError("", "Категорія з такою назвою вже існує");
                model.ExistingImage = category.Image; //щоб не втратити посилання на існуюче зображення
                return View(model); 
            }


                //оновлюємо дані категорії
                category.Name = model.Name;

            //якщо завантажено нове зображення, видаляємо старе і завантажуємо нове
            if (model.NewImage != null)
            {
                var oldFile = Path.Combine(Directory.GetCurrentDirectory(), configuration["DirPath"]!, Path.GetFileName(category.Image));
                if (System.IO.File.Exists(oldFile)) System.IO.File.Delete(oldFile);

                category.Image = await imageService.UploadImageAsync(model.NewImage);
            }

            await myAppDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost] //soft delete категорії - приховує категорію не видаляючи її з бд
        public IActionResult DeleteCategory(int id)
        {
            //Додаємо id категорії до списку прихованих
            hiddenService.Hide(id);
            return RedirectToAction("Index");
        }
    }
}
