using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkingMVC.Data;
using WorkingMVC.Data.Entitys;
using WorkingMVC.Areas.Admin.Interfaces;
using WorkingMVC.Areas.Admin.Models.Category;
using Microsoft.AspNetCore.Authorization;
using WorkingMVC.Constants;

namespace WorkingMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{Roles.Admin}")]
    public class CategoryController(
        ICategoryService categoryService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var model = await categoryService.GetAllAsync();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        } 

        [HttpPost]
        public async Task<IActionResult> Create(CategoyCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }
            try
            {
                await categoryService.CreateAsync(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await categoryService.GetEditModelAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await categoryService.UpdateAsync(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await categoryService.SoftDeleteAsync(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return RedirectToAction("Index");
        }
    }
}
