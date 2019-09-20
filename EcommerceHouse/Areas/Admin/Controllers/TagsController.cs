using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceHouse.Data;
using EcommerceHouse.Models;
using EcommerceHouse.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceHouse.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class TagsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TagsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.Tags.ToList());
        }
        //Get Create a product Type action method
        public IActionResult Create()
        {
            return View();
        }

        //POST create product action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tags tags)
        {
            if (ModelState.IsValid)
            {
                _db.Add(tags);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tags);
        }
        //Get Edit a product Type action method
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tags = await _db.Tags.FindAsync(id);

            if (tags == null)
            {
                return NotFound(tags);
            }

            return View(tags);
        }

        //POST Edit product action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tags tags)
        {
            if (id != tags.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Update(tags);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tags);
        }
        //GET Details product Type action method
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tags = await _db.Tags.FindAsync(id);

            if (tags == null)
            {
                return NotFound(tags);
            }

            return View(tags);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tags = await _db.Tags.FindAsync(id);

            if (tags == null)
            {
                return NotFound(tags);
            }

            return View(tags);
        }

        //POST Delete product action method
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var tags = await _db.Tags.FindAsync(id);
            _db.Tags.Remove(tags);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}