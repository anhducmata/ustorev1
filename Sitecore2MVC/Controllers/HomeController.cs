using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Data;
using Sitecore1MVC.Models;
using Sitecore1MVC.Repository;
using Sitecore1MVC.ViewModels;
using Sitecore2MVC.ViewModels;

namespace Sitecore2MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly Sitecore.Data.Database _context = Sitecore.Context.Database;
        IBrandRepository IBrand = new BrandRepository();
        IProductRepository IProduct = new ProductRepository();

        // GET: Home
        public ActionResult HomeContent()
        {
            Sitecore.Data.Items.Item navigation = _context.Items[new ID("{536DA031-227B-45ED-83C0-3095D274B922}")];
            var productViewModels = new ProductViewModels();
            productViewModels.ListProducts = IProduct.ListProducts();
            return View("Contents/HomeContent", productViewModels);
        }
    }
}