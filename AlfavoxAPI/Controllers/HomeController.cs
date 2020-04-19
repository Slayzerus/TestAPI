using AlfavoxAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlfavoxAPI.Controllers
{
    public class HomeController : Controller
    {
        private ProductContext db = new ProductContext();


        public ActionResult Index()
        {
            return View();
        }

        

        public ActionResult GUI()
        {
            List<Product> products = db.Products.ToList();            

            return View(products);
        }

        [HttpPost]
        public ActionResult GUI(Product model)
        {
            db.Products.Add(model);
            db.SaveChanges();
            List<Product> products = db.Products.ToList();            

            return View(products);
        }


        public ActionResult Delete(long id) 
        {
            if (id > 0) 
            {
                Product product = db.Products.Find(id);
                if (product != null) 
                {
                    db.Products.Remove(product);
                    db.SaveChanges();
                }
            }            

            return RedirectToAction("GUI");
        }

    }
}