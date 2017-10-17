using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore1MVC.Models;

namespace Sitecore1MVC.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly Sitecore.Data.Database _context = Sitecore.Context.Database;
        public List<Product> ListProducts()
        {
            Sitecore.Data.Items.Item products = _context.Items[new ID("{3142007F-547D-464A-8994-54B286AB1DBF}")];
            var data = new List<Product>();
            foreach (Item product in products.Children)
            {
                var newProduct = new Product
                {
                   Brand = product.Name,
                   Name = product.Fields["name"].Value,
                   Description = product.Fields["description"].Value,
                   Image = GetUrl(product.Fields["image"]),
                   Price = double.Parse(product.Fields["price"].Value),
                   Alias = product.Fields["alias"].Value
                };
                data.Add(newProduct);
            }
            return data;
        }

        public Product GetProductByAlias(string alias)
        {
            Sitecore.Data.Items.Item products = _context.Items[new ID("{3142007F-547D-464A-8994-54B286AB1DBF}")];
            var product = products.Children.FirstOrDefault(i => i.Fields["alias"].Value.Equals(alias));
            if (product != null)
            {
                return new Product()
                {
                    Brand = product.Fields["Brand"].Value,
                    Name = product.Fields["name"].Value,
                    Description = product.Fields["description"].Value,
                    Image = GetUrl(product.Fields["image"]),
                    Price = double.Parse(product.Fields["price"].Value),
                    Alias = product.Fields["alias"].Value
                };
            }
            return null;
        }

        public string GetUrl(Sitecore.Data.Fields.ImageField imgId)
        {
            var imageUrl = string.Empty;

            Sitecore.Data.Fields.ImageField imageField = imgId;
            if (imageField?.MediaItem != null)
            {
                var image = new MediaItem(imageField.MediaItem);
                imageUrl = StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(image));
            }
            return imageUrl;
        }
    }
}