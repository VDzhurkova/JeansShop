using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Controllers
{
    public class AdminController : Controller
    {
        private EcommerceDBEntities db = new EcommerceDBEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult AllProducts() {
            var products = from p in db.Products select p;
            return PartialView(products);
        }

        public PartialViewResult AllOrders() {
        var orders =from o in db.Orders select o;
            return PartialView(orders);
        }

        public PartialViewResult AllOrderDetails(int? id)
        {
            Order order = db.Orders.Find(id);
            Customer customer = db.Customers.SingleOrDefault(c => c.Id == order.CustomerId);
            var tuple = new Tuple<Order, Customer>(order, customer);
            return PartialView(tuple);
        }

        public RedirectToRouteResult EditOrder(int? id) {
            Order order = db.Orders.Find(id);
            order.IsShipped = true;
            db.SaveChanges();
            return RedirectToRoute(new { controller="Admin", action="Index" });
        }

        public ActionResult EditProduct(int? id)
        {
           
            Product product = db.Products.Find(id);
            return View(product);
        }

        [HttpPost]
        public ActionResult EditProduct(Product product)

        {

            HttpPostedFileBase file = Request.Files["ImageData"];
            byte[] imageBytes = null;

            BinaryReader reader = new BinaryReader(file.InputStream);

            imageBytes = reader.ReadBytes((int)file.ContentLength);
            product.Image = imageBytes;
            if (product.Quantity >= 1)
            {
                product.InStock = true;
            }

            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }

            return View(product);

        }
        public ActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Product product)

        {

            HttpPostedFileBase file = Request.Files["ImageData"];
            byte[] imageBytes = null;

            BinaryReader reader = new BinaryReader(file.InputStream);

            imageBytes = reader.ReadBytes((int)file.ContentLength);
            product.Image = imageBytes;
            if (product.Quantity >= 1)
            {
                product.InStock = true;
            }

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }

            return View(product);

        }

        public RedirectToRouteResult DeleteProduct(int? id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            try { db.SaveChanges();
                return RedirectToRoute(new { controller = "Admin", action = "Index" });
            } catch (Exception e) {
                TempData["ErrorMessage"] = "The Product " +product.Name+ " can't be removed from the datatable. It is still contained in some Order details.";
                return RedirectToRoute(new { controller = "Admin", action = "Index" });
            }
           
        }
        public ActionResult DeleteOrder(int? id) {
            Order order = db.Orders.Find(id);
            var details = from d in db.OrderDetails where d.OrderId == id select d;
            foreach (var d in details) {
                db.OrderDetails.Remove(d);
                //db.SaveChanges();
            }
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }

        public RedirectToRouteResult LogOut()
        {
            Session["userName"] = null;
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
    }
}