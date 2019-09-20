using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EcommerceHouse.Models;
using EcommerceHouse.Data;
using Microsoft.EntityFrameworkCore;
using EcommerceHouse.Extensions;

namespace EcommerceHouse.Controllers
{

    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var productList = await _db.Products.Include(m => m.ProductTypes).Include(m => m.Tags).ToListAsync();

            return View(productList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _db.Products.Include(m => m.ProductTypes).Include(m => m.Tags).Where(m => m.Id == id).FirstOrDefaultAsync();

            return View(product);
        }

        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsCart(int id)
        {
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("sShoppingCart");
            if(lstShoppingCart==null)
            {
                lstShoppingCart = new List<int>();
            }
            lstShoppingCart.Add(id);
            HttpContext.Session.Set("sShoppingCart", lstShoppingCart);

            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        public IActionResult Remove(int id)
        {
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("sShoppingCart");
            if(lstShoppingCart.Count > 0)
            {
                if(lstShoppingCart.Contains(id))
                {
                    lstShoppingCart.Remove(id);
                }
            }
            HttpContext.Session.Set("sShoppingCart", lstShoppingCart);

            return RedirectToAction(nameof(Index));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
