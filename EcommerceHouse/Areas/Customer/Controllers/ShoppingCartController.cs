using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceHouse.Data;
using EcommerceHouse.Extensions;
using EcommerceHouse.Models;
using EcommerceHouse.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceHouse.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }

        public ShoppingCartController(ApplicationDbContext db)
        {
            _db = db;
            ShoppingCartVM = new ShoppingCartViewModel()
            {
                Products = new List<Models.Products>()
            };
        }
        
        //Get: Index Cart
        public async Task<IActionResult> Index()
        {
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("sShoppingCart");

            try
            {
                if (lstShoppingCart.Count > 0)
                {
                    foreach (int cartItem in lstShoppingCart)
                    {
                        Products prod = _db.Products.Include(p => p.Tags).Include(p => p.ProductTypes).Where(p => p.Id == cartItem).FirstOrDefault();
                        ShoppingCartVM.Products.Add(prod);
                    }
                }
            }
            catch(Exception)
            {
                
                return View(ShoppingCartVM);
                
            }
           
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            List<int> lstCartItems = HttpContext.Session.Get<List<int>>("sShoppingCart");

            ShoppingCartVM.Appointments.AppointmentDate = ShoppingCartVM.Appointments.AppointmentDate
                                                            .AddHours(ShoppingCartVM.Appointments.AppointmentTime.Hour)
                                                            .AddMinutes(ShoppingCartVM.Appointments.AppointmentTime.Minute);

            Appointments appointments = ShoppingCartVM.Appointments;
            _db.Appointments.Add(appointments);
            _db.SaveChanges();

            int appointmentId = appointments.Id;

            foreach(int productId in lstCartItems)
            {
                ProductsSelectedForAppointment productsSelectedForAppointment = new ProductsSelectedForAppointment()
                {
                    AppointmentId = appointmentId,
                    ProductId = productId
                };
                _db.ProductsSelectedForAppointment.Add(productsSelectedForAppointment);
               
            }
            _db.SaveChanges();
            lstCartItems = new List<int>();
            HttpContext.Session.Set("sShoppingCart", lstCartItems);

            return RedirectToAction("AppointmentConfirmation","ShoppingCart", new { Id = appointmentId});
        }

        public IActionResult Remove(int id)
        {
            List<int> lstCartItems = HttpContext.Session.Get<List<int>>("sShoppingCart");

            if(lstCartItems.Count > 0)
            {
                if(lstCartItems.Contains(id))
                {
                    lstCartItems.Remove(id);
                }
            }

            HttpContext.Session.Set("sShoppingCart", lstCartItems);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AppointmentConfirmation(int id)
        {
            ShoppingCartVM.Appointments = _db.Appointments.Where(a => a.Id == id).FirstOrDefault();
            List<ProductsSelectedForAppointment> objProdList = _db.ProductsSelectedForAppointment.Where(p => p.AppointmentId == id).ToList();

            foreach(ProductsSelectedForAppointment prodAptObj in objProdList)
            {
                ShoppingCartVM.Products.Add(_db.Products.Include(p => p.ProductTypes).Include(p => p.Tags).Where(p => p.Id == prodAptObj.ProductId).FirstOrDefault());
            }

            return View(ShoppingCartVM);
        }
    }
}