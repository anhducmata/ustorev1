using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Sitecore.Common;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore1MVC.Models;
using Sitecore1MVC.Repository;
using Sitecore1MVC.ViewModels;
using Sitecore2MVC.ViewModels;

namespace Sitecore2MVC.Controllers
{
    public class SiteController : Controller
    {
        private readonly Sitecore.Data.Database _context = Sitecore.Context.Database;
        IBrandRepository IBrand = new BrandRepository();
        IProductRepository IProduct = new ProductRepository();

        public ActionResult Header()
        {
            FormsAuthenticationTicket authTicket = null;
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];//.ASPXAUTH
            if (authCookie != null)
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            string email = null;
            if (authTicket != null)
            {
                email = authTicket.Name;
            }
            email = email?.Replace("default\\", "").Replace("_at_", "@").Replace("_dot_", ".");
            Session["email"] = email;
            Session["Amount"] = GetCartAmount();
            Session["NumProduct"] = GetCartNumberProduct();
            return View("Partials/Header");
        }

        public ActionResult CartContent()
        {
            var data = GetCartFromSession();
            return View("Contents/CartContent", data);
        }

        public ActionResult Footer()
        {
            return View("Partials/Footer");
        }

        public ActionResult Widget()
        {
            return View("Partials/Widget");
        }

        public ActionResult Navigation()
        {
            Sitecore.Data.Items.Item navigation = _context.Items[new ID("{536DA031-227B-45ED-83C0-3095D274B922}")];
            var data = new List<Navigation>();
            foreach (Item nav in navigation.Children)
            {
                var newNav = new Navigation()
                {
                    DisplayName = nav.Fields["name"].Value,
                    Link = LinkUrl(nav.Fields["link"])
                };
                data.Add(newNav);
            }
            return View("Partials/navigation", data);
        }

        public ActionResult ContactContent()
        {
            Sitecore.Data.Items.Item contact = _context.Items[new ID("{900CD388-E141-4C9C-B0B9-04D38EF5FD56}")];
            return View("Contents/ContactContent", contact);
        }

        public ActionResult SearchContent(string keyword)
        {
            Sitecore.Data.Items.Item shop = _context.Items[new ID("{A1751A8C-3930-4D7E-BF0F-491DAD1F5850}")];
            var productViewModels = new ProductViewModels()
            {
                ListProducts = IProduct.ListProducts().FindAll(i => i.Name.ToLower().Contains(keyword.ToLower()))
                    .Take(Convert.ToInt32(shop.Fields["result_number"].Value)).ToList()
            };
            return View("Contents/SearchContent", productViewModels);
        }

        public static String LinkUrl(Sitecore.Data.Fields.LinkField lf)
        {
            switch (lf.LinkType.ToLower())
            {
                case "internal":
                    // Use LinkMananger for internal links, if link is not empty
                    return lf.TargetItem != null ? Sitecore.Links.LinkManager.GetItemUrl(lf.TargetItem) : string.Empty;
                case "media":
                    // Use MediaManager for media links, if link is not empty
                    return lf.TargetItem != null ? Sitecore.Resources.Media.MediaManager.GetMediaUrl(lf.TargetItem) : string.Empty;
                case "external":
                    // Just return external links
                    return lf.Url;
                case "anchor":
                    // Prefix anchor link with # if link if not empty
                    return !string.IsNullOrEmpty(lf.Anchor) ? "#" + lf.Anchor : string.Empty;
                case "mailto":
                    // Just return mailto link
                    return lf.Url;
                case "javascript":
                    // Just return javascript
                    return lf.Url;
                default:
                    // Just please the compiler, this
                    // condition will never be met
                    return lf.Url;
            }
        }

        public List<CartViewModels> GetCartFromSession()
        {
            return Session["cart"] as List<CartViewModels>;
        }

        public decimal? GetCartAmount()
        {
            return (decimal?) (Session["cart"] is List<CartViewModels> listProduct ? (listProduct ?? throw new InvalidOperationException()).Sum(item => item.Product.Price * item.Quantity) : 0);
        }

        public int GetCartNumberProduct()
        {
            return Session["cart"] is List<CartViewModels> listProduct ? (listProduct ?? throw new InvalidOperationException()).Sum(item => item.Quantity) : 0;
        }

        public void AddCookie(string id)
        {
            if (!Request.Cookies.AllKeys.Contains("products"))
            {
                HttpCookie cookie = new HttpCookie("products");
                cookie.Value = id;
                cookie.Expires = DateTime.Now.AddMonths(1);
                Response.Cookies.Add(cookie);
            }
            else
            {
                HttpCookie cookie = Request.Cookies["products"];
                var newCookie = cookie.Value + '*' + id;
                cookie.Expires = DateTime.Now.AddMonths(-1);
                Response.SetCookie(cookie);

                HttpCookie Cookie1 = new HttpCookie("products");
                Cookie1.Value = newCookie;
                Cookie1.Expires = DateTime.Now.AddMonths(1);
                Response.Cookies.Add(Cookie1);
            }
        }

        public HttpCookie GetCookie()
        {
            return Request.Cookies["products"];
        }

        public ActionResult AddToCart(string alias, int quantity)
        {
            var product = IProduct.GetProductByAlias(alias);
            if (product != null)
            {
                if (Session["cart"] == null)
                {
                    var cart = new List<CartViewModels>()
                    {
                        new CartViewModels()
                        {
                            Product = product,
                            Quantity = quantity
                        }
                    };
                    Session["cart"] = cart;
                }
                else
                {
                    var cart = Session["cart"] as List<CartViewModels>;
                    if ((cart ?? throw new InvalidOperationException()).Count(d => d.Product.Alias.Contains(alias)) > 0)
                    {
                        (cart ?? throw new InvalidOperationException()).First(d => d.Product.Alias.Contains(alias)).Quantity++;
                    }
                    else
                    {
                        cart?.Add(new CartViewModels()
                        {
                            Product = product,
                            Quantity = quantity
                        });
                    }
                    Session["cart"] = cart;
                }
            }
            Session["Amount"] = GetCartAmount();
            Session["NumProduct"] = GetCartNumberProduct();
            AddCookie(alias);
            return Redirect(Request.UrlReferrer.ToString());

        }

        public RedirectResult RemoveProductOnCart(string alias)
        {
            var list = (List<CartViewModels>)Session["cart"];
            var listDel = list.First(x => x.Product.Alias.Contains(alias));
            list.Remove(listDel);
            Session["cart"] = list;
            Session["Amount"] = GetCartAmount();
            Session["NumProduct"] = GetCartNumberProduct();
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult _RecommentProduct()
        {
            return PartialView("Partials/_RecommentProduct", GetProductOnCookies());
        }

        public List<Product> GetProductOnCookies()
        {

            var cookie = GetCookie();
            if (cookie != null)
            {
                var model = new List<Product>();
                string[] dataCookie = cookie.Value.Split('*');
                dataCookie = dataCookie.Distinct().Take(3).Reverse().ToArray();
                foreach (string alias in dataCookie)
                {
                    var product = IProduct.GetProductByAlias(alias);
                    if (product != null)
                    {
                        model.Add(product);
                    }
                }
                return model.ToList();
            }
            return new List<Product>();
        }
    }
}