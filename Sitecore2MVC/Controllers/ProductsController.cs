using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore1MVC.Models;
using Sitecore1MVC.Repository;

namespace Sitecore2MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly Sitecore.Data.Database _context = Sitecore.Context.Database;
        IBrandRepository IBrand = new BrandRepository();
        IProductRepository IProduct = new ProductRepository();

        public ActionResult DetailContent(string name)
        {
            Product data = IProduct.ListProducts().FirstOrDefault(i => i.Alias.Equals(name));

            return data != null ? (ActionResult)View("Contents/DetailContent",data) : View("Error");
        }
    }
}