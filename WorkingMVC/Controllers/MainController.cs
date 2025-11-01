using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WorkingMVC.Data;
using WorkingMVC.Data.Entitys;
using WorkingMVC.Models;

namespace WorkingMVC.Controllers
{
    public class MainController(MyAppDbContext myAppDbContext) : Controller
    {
        public IActionResult Index()
        {
            var list = myAppDbContext.Categories.ToList();
            return View(list);
        }

        [HttpPost]
        public IActionResult AddCategory(string Name, string Image){
            if (!string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Image))
            {
                var newCategory = new CategoryEntity{
                    Name = Name,
                    Image = Image
                };
                myAppDbContext.Categories.Add(newCategory);
                myAppDbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            var category = myAppDbContext.Categories.Find(id);
            if (category != null)
            {
                myAppDbContext.Categories.Remove(category);
                myAppDbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
