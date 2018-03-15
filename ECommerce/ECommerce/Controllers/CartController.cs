using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ECommerce.Controllers
{
    public class CartController : Controller
    {
        private EcommerceDBEntities db = new EcommerceDBEntities();

        // GET: Cart
        public ViewResult Index(Cart c)
        {
          
            return View(new CartIndexViewModel
            {
                cart=c
            });
        }

        public ActionResult AddToCart(int? id, Cart cart) {
            Product product = db.Products.FirstOrDefault(p => p.Id == id);
            return RedirectToAction("Products", "Home");

        }

        [HttpPost]
        public RedirectToRouteResult AddToCart(int? id, int quantity, Cart cart)
        {
            if (quantity == 0) {

            if (!string.IsNullOrEmpty(Request["txtAmount"]) && Convert.ToInt32(Request["txtAmount"]) >= 1)
            {
                    TempData["ErrorMessage"] = "";
                    quantity = Convert.ToInt32(Request["txtAmount"]);
                }
            else  {
                TempData["ErrorMessage"] = "Please enter valid quantity";
                return RedirectToRoute(new { controller = "Home", action = "Details", id = id });
            }
            }

            Product product = db.Products.FirstOrDefault(p => p.Id == id);
            if (product != null) {
                cart.AddLine(product, quantity);
            }
           
            return RedirectToRoute(new { controller = "Home", action = "Products" , searchString = Session["search"] });
        }

        public ActionResult RemoveFromCart(int? id, Cart cart)
        {
            Product product = db.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", "Cart");
        }


        public PartialViewResult Summary(Customer customer, Cart cart) {
            var tuple = new Tuple<Customer, Cart>(customer, cart);
            return PartialView(tuple);
        }

    }
}