using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using WorkingMVC.Areas.Admin.Interfaces;
using WorkingMVC.Areas.Admin.Models.Product;
using WorkingMVC.Constants;

namespace WorkingMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class ProductController(
        IProductService productService, 
        ICategoryService categoryService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products = await productService.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(model);
            }

            await productService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await productService.GetEditModelAsync(id);
            ViewBag.Categories = new SelectList(await categoryService.GetAllAsync(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(await categoryService.GetAllAsync(), "Id", "Name");
                return View(model);
            }

            await productService.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await productService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
