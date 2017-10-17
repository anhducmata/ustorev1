using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore1MVC.Models;

namespace Sitecore1MVC.Repository
{
    public interface IProductRepository
    {
        List<Product> ListProducts();

        Product GetProductByAlias(string alias);
    }
}