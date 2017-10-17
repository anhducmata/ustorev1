using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore1MVC.Models;

namespace Sitecore1MVC.ViewModels
{
    public class ProductViewModels
    {
        public ProductViewModels()
        {
            this.ListProducts = new List<Product>();
        }
        public IEnumerable<Product> ListProducts { get; set; }

    }
}