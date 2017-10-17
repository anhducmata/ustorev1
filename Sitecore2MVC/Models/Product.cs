using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore1MVC.Models
{
    public class Product
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public string Brand { get; set; }
     
        public string Alias { get; set; }
    }
}