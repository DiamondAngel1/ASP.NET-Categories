using Microsoft.AspNetCore.Mvc;
using WorkingMVC.Areas.Admin.Interfaces;

namespace WorkingMVC.Controllers
{
    public class ProductUserController(IProductService productService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products = await productService.GetAllAsync();
            return View("~/Views/Product/Index.cshtml", products);
        }
    }
}
