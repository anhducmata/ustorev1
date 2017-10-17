using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Sitecore.Mvc.Controllers;
using Sitecore1MVC.Repository;

namespace Sitecore2MVC.Controllers
{
    struct SimpleList
    {
        public string name;
        public string alias;
    }
    public class ExtendController : Controller
    {
        IProductRepository iProduct = new ProductRepository();
        // GET: Extend
        public RedirectResult Logout()
        {
            Session.Abandon();
            if (Request.Cookies[".ASPXAUTH"] != null)
            {
                Response.Cookies[".ASPXAUTH"].Expires = DateTime.Now.AddDays(-1);
            }
            return Redirect("~");
        }

        public ActionResult getProducts()
        {
            var listProducts = iProduct.ListProducts();
            var customListProducts = new List<SimpleList>();
            foreach (var product in listProducts)
            {
                customListProducts.Add(new SimpleList()
                {
                    alias = "./products?name=" + product.Alias,
                    name = product.Name
                });
            }
            
            var jsonData = JsonConvert.SerializeObject(customListProducts);
            return Content(jsonData);
        }
    }
}