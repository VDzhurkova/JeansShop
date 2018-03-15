using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;

namespace ECommerce.Controllers
{
    public class HomeController : Controller
    {
        private EcommerceDBEntities db = new EcommerceDBEntities();


        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Products(string CategoryName, string searchString) {
            var products = from p in db.Products select p;
            var CategoryList = new List<string>();
            var CategoryQuery = from c in db.Products
                                orderby c.Category
                                select c.Category;
            if (!String.IsNullOrEmpty(searchString))
            {
                Session["search"] = searchString;
                products = products.Where(p => p.Category.Contains(searchString));
                CategoryQuery = CategoryQuery.Where(p => p.Contains(searchString));
            }
            
           
            CategoryList.AddRange(CategoryQuery.Distinct());
            ViewData["CategoryName"] = new SelectList(CategoryList);
             
            if (!String.IsNullOrEmpty(CategoryName))
            {
                TempData["category"] = CategoryName;
                products = products.Where(p=>p.Category == CategoryName);
                
            }
            
            
           
            return View(products);
        }
      

        public ActionResult getImage(int? id) {
            var images = from p in db.Products where p.Id == id select p.Image;

            byte[] cover = images.First();
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else return null;
        }

        public ActionResult Details(int? id)
        {
            Product product = db.Products.Single(p => p.Id == id);
            return View(product);
        }

        public ActionResult CheckOut() {
            return View();
        }

        public ActionResult CheckOutNow(int? id) {
            Customer customer = db.Customers.Find(id);
            return View(customer);
        }

        [HttpPost]
        public ActionResult CheckOutNow(int? id, Cart cart)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                customer = new Customer();
                customer.FirstName = Convert.ToString(Request["txtFirstName"]);
                Debug.WriteLine(customer.FirstName);
            }
            else {
                Order order = new Order();
                order.CustomerId = customer.Id;
                order.Amount = cart.totalValue();
                order.Date = DateTime.Now;
                order.IsShipped = false;
                db.Orders.Add(order);
                db.SaveChanges();

                foreach (var line in cart.Lines)
                {
                    OrderDetail detail = new OrderDetail();
                    detail.OrderId = order.Id;
                    detail.ProductId = line.Product.Id;
                    detail.Quantity = line.Quantity;
                    detail.Price = line.Quantity * (decimal)line.Product.Price;

                    Product product = db.Products.Single(p => p.Id == line.Product.Id);
                    product.Quantity -= line.Quantity;
              
                    db.SaveChanges();
                    db.OrderDetails.Add(detail);
                    db.SaveChanges();
                }
            }
            cart.Clear();
            return View(customer);
        }

        public ViewResult ViewOrder(int? id) {

            var order = from o in db.Orders where o.CustomerId == id orderby o.Date descending select o;
            return View(order);
        }

        public PartialViewResult ViewDetails(int? id) {
            var details = from d in db.OrderDetails where d.OrderId == id select d;
            return PartialView(details);
        }

        public PartialViewResult ViewProduct(int? id) {
            Product product = db.Products.Find(id);
            return PartialView(product);
        }

        public ActionResult CheckOutGuest(Cart cart)
        {
            return View();
        }

        public PartialViewResult AddCustomer()
        {
            return PartialView();
        }

        [HttpPost]
        public ViewResult AddCustomer(Customer customer)
        {

            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                Session["userId"] = customer.Id;
                Session["userName"] = customer.FirstName + " " + customer.LastName;
                return View("Account", customer);

            }
            return View(customer);
        }

        public PartialViewResult LogInCustomer()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult LogInCustomer(string email, string password)
        {
            Admin admin = new Admin();
            email = Convert.ToString(Request["txtEmail"]);
            password = Convert.ToString(Request["txtPassword"]);

            if (email == admin.Email && password == admin.Password) {
                Session["userName"] = "admin";
                return RedirectToAction("Index", "Admin");
            }
            Customer customer = db.Customers.SingleOrDefault(c => c.Email == email && c.Password == password);

            if (customer != null)
            {
                Session["userId"] = customer.Id;
                Session["userName"] = customer.FirstName + " " + customer.LastName;
                return View("Account", customer);
            }
            ModelState.AddModelError("", "Wrong email address or password");
            return View();
        }

        public ViewResult Account(int? id)
        {
            Customer customer = db.Customers.Find(id);
            return View(customer);
        }

        [HttpPost]
        public ViewResult Account(int? id, Cart cart) {
            Customer customer = db.Customers.Find(id);
            return View(customer);
        }


        public ActionResult LogOut(int? id, Cart cart) {
            Session["userName"] = null;
            Session["userId"] = null;
            cart.Clear();
            return RedirectToAction("Index", "Home");
        }
        public PartialViewResult EditCustomer(int? id)
        {
            Customer customer = db.Customers.Find(id);
            return PartialView(customer);
        }

        [HttpPost]
        public PartialViewResult EditCustomer( Customer customer)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
              
                return PartialView( );
            }
            return PartialView(customer);
        }


        [AllowAnonymous]
    [HttpPost]
    public JsonResult checkEmailValidate(string Email)
    {
        if (checkEmail(Email))
        {
            return Json(true);
        }
        return Json(false);
    }



    public Boolean checkEmail(string email)
    {

        Customer customer = (from c in db.Customers
                             where c.Email == email
                             select c).FirstOrDefault();
        if (customer == null)
        {
            return true;
        }


        return false;
    }
  }

}



   
