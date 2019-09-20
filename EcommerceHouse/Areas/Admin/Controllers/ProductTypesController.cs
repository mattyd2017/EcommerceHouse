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
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductTypesController( ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.ProductTypes.ToList());
        }

        //Get Create a product Type action method
        public IActionResult Create()
        {
            return View();
        }

        //POST create product action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes productTypes)
        {
            if(ModelState.IsValid)
            {
                _db.Add(productTypes);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productTypes);
        }
        //Get Edit a product Type action method
        public async Task<IActionResult> Edit(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }

            var productType = await _db.ProductTypes.FindAsync(id);

            if(productType == null)
            {
                return NotFound(productType);
            }

            return View(productType);
        }

        //POST Edit product action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductTypes productTypes)
        {
            if(id!=productTypes.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Update(productTypes);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productTypes);
        }

        //GET Details product Type action method
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await _db.ProductTypes.FindAsync(id);

            if (productType == null)
            {
                return NotFound(productType);
            }

            return View(productType);
        }

        //GET Delete product action method
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await _db.ProductTypes.FindAsync(id);

            if (productType == null)
            {
                return NotFound(productType);
            }

            return View(productType);
        }

        //POST Delete product action method
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var productTypes = await _db.ProductTypes.FindAsync(id);
            _db.ProductTypes.Remove(productTypes);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));           
 
        }
    }
}