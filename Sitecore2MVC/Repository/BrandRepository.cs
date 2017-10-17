using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore1MVC.Models;

namespace Sitecore1MVC.Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly Sitecore.Data.Database _context = Sitecore.Context.Database;
        public List<Brand> ListBrands()
        {
            Sitecore.Data.Items.Item brands = _context.Items[new ID("{93D072C2-26E9-4487-8D0D-88E6C8DB03C5}")];
            var data = new List<Brand>();
            foreach (Item brand in brands.Children)
            {
                var name = brand.Fields["Name"].Value;
                var logo = brand.Fields["Logo"].Value;
                var newBrand = new Brand
                {
                    Name = name,
                    Logo = logo
                };
                data.Add(newBrand);
            }
            return data;
        }
    }
}