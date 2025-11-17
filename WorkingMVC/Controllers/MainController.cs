using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkingMVC.Data;
using WorkingMVC.Data.Entitys;
using WorkingMVC.Interfaces;

namespace WorkingMVC.Controllers
{
    public class MainController(
        ICategoryServiceUser categoryService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var model = await categoryService.GetAllAsync();
            return View(model);
        }
    }
}
