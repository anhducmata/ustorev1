using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore2MVC.ViewModels
{
    public class CartHeaderViewModels
    {
        public int Quantity { get; set; }

        public decimal? TotalPrice { get; set; }
    }
}