using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore1MVC.Repository;
using Sitecore1MVC.ViewModels;

namespace Sitecore2MVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly Sitecore.Data.Database _context = Sitecore.Context.Database;
        IBrandRepository IBrand = new BrandRepository();
        IProductRepository IProduct = new ProductRepository();

        public ActionResult ShopContent()
        {
            var productViewModels = new ProductViewModels()
            {
                ListProducts = IProduct.ListProducts()
            };
            return View("Contents/ShopContent",productViewModels);
        }
    }
}