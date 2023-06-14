using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShopKnitting.Models;
using ShopKnitting.Models.DataModel;
using ShopKnitting.Models.HelpModels;

namespace ShopKnitting.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Basket
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.BasketProductLinkModel.Include(b => b.Basket).Include(b => b.Product);
            return View(await appDbContext.ToListAsync());
        }
        [HttpPost]
        public bool RemoveItemFromBasket()
        {

            Dictionary<string, object> request = new Dictionary<string, object>();

            MemoryStream stream = new MemoryStream();
            Request.Body.CopyTo(stream);
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream))
            {
                string requestBody = reader.ReadToEnd();
                if (requestBody.Length > 0)
                {
                    request = JsonConvert.DeserializeObject<Dictionary<string, object>>(requestBody);
                }
            }

            int product_id = int.Parse(request["product_id"].ToString());

           // bool userIsSignedIn = _signInManager.IsSignedIn(User);

            // BASKET: Remove product from basket
            try
            {
                //if (!userIsSignedIn)
                //{ // in cookies
                    BasketHelper.RemoveFromCookieBasket(product_id, Request, Response);

                    return true;
                //}
                //else
                //{ // in db
                //    UserModel user = _userManager.GetUserAsync(User).Result;

                //    BasketModel basketModel = _context.Baskets.FirstOrDefault(b => b.UserId == user.Id);
                //    BasketProductLinkModel basketProductLinkModel = _context.BasketProductLinks
                //        .Where(b => b.BasketId == basketModel.Id)
                //        .FirstOrDefault(p => p.ProductId == product_id);
                //    _context.Remove(basketProductLinkModel);
                //    _context.SaveChanges();

                //    return true;
                //}
            }
            catch
            {
                return false;
            }
        }

        [HttpGet]
        public double GetBasketTotalCost()
        {

            double TotalCost = 0;

            //bool userIsSignedIn = _signInManager.IsSignedIn(User);

            // BASKET: Get total sum of products
            //if (!userIsSignedIn)
            //{ // from cookies
                var basket = BasketHelper.GetBasketFromCookie(Request, Response);

                foreach (var productKVP in basket)
                {
                    ProductModel product = _context.ProductModels.ToList().Find(p => p.Id == productKVP.Key);
                    TotalCost += product.Cost * productKVP.Value;
                }
            //}
            //else
            //{ // from db
            //    UserModel user = _userManager.GetUserAsync(User).Result;

            //    BasketModel basketModel = _context.Baskets.FirstOrDefault(b => b.UserId == user.Id);
            //    if (basketModel == null)
            //    {
            //        basketModel = new BasketModel();
            //        basketModel.User = user;
            //        _context.Baskets.Add(basketModel);
            //        _context.SaveChanges();
            //    }

            //    TotalCost = _context.BasketProductLinks.Where(b => b.BasketId == basketModel.Id)
            //        .Sum(p => p.Product.Cost * p.CountCopies);
            //}

            return TotalCost;
        }

        [HttpPost]
        public bool ChangeProductCount()
        {

            Dictionary<string, object> request = new Dictionary<string, object>();
            Dictionary<string, object> response = new Dictionary<string, object>();

            MemoryStream stream = new MemoryStream();
            Request.Body.CopyTo(stream);
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream))
            {
                string requestBody = reader.ReadToEnd();
                if (requestBody.Length > 0)
                {
                    request = JsonConvert.DeserializeObject<Dictionary<string, object>>(requestBody);
                }
            }

            int product_id = int.Parse(request["product_id"].ToString());
            int product_count = int.Parse(request["product_count"].ToString());

           // bool userIsSignedIn = _signInManager.IsSignedIn(User);

            // BASKET: Update count od product
            try
            {
                //if (!userIsSignedIn)
                //{ // in cookies
                    BasketHelper.UpdateCount(product_id, product_count, Response, Request);

                    return true;
                //}
                //else
                //{ // in db
                //    UserModel user = _userManager.GetUserAsync(User).Result;

                //    BasketModel basketModel = _context.Baskets.FirstOrDefault(b => b.UserId == user.Id);
                //    BasketProductLinkModel basketProductLinkModel = _context.BasketProductLinks
                //        .Where(b => b.BasketId == basketModel.Id)
                //        .FirstOrDefault(p => p.ProductId == product_id);
                //    basketProductLinkModel.CountCopies = product_count;
                //    _context.BasketProductLinks.Update(basketProductLinkModel);
                //    _context.SaveChanges();

                //    return true;
                //}
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public bool AddProductToBasket()
        {

            Dictionary<string, object> response = new Dictionary<string, object>();

            MemoryStream stream = new MemoryStream();
            Request.Body.CopyTo(stream);
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream))
            {
                string requestBody = reader.ReadToEnd();
                if (requestBody.Length > 0)
                {
                    response = JsonConvert.DeserializeObject<Dictionary<string, object>>(requestBody);
                }
            }

            int product_id = int.Parse(response["product_id"].ToString());
            ProductModel product = _context.ProductModels.FirstOrDefault(p => p.Id == product_id);

            //bool userIsSignedIn = _signInManager.IsSignedIn(User);

            // BASKET: Add new product to basket
            try
            {
                //if (!userIsSignedIn)
                //{ // into cookies
                    BasketHelper.AddToCookieBasket(product, Request, Response);

                    return true;
                //}
                //else
                //{ // into db
                //    UserModel user = _userManager.GetUserAsync(User).Result;

                //    BasketModel basketModel = _context.Baskets.FirstOrDefault(b => b.UserId == user.Id);

                //    if (basketModel == null)
                //    {
                //        basketModel = new BasketModel();
                //        basketModel.User = user;
                //        _context.Baskets.Add(basketModel);
                //        _context.SaveChanges();
                //    }

                //    BasketProductLinkModel basketProductLinkModel = _context.BasketProductLinks
                //        .Where(b => b.BasketId == basketModel.Id).FirstOrDefault(b => b.ProductId == product_id);

                //    if (basketProductLinkModel != null)
                //    {
                //        basketProductLinkModel.CountCopies += 1;
                //        _context.BasketProductLinks.Update(basketProductLinkModel);
                //        _context.SaveChanges();
                //    }
                //    else
                //    {
                //        basketProductLinkModel = new BasketProductLinkModel();
                //        basketProductLinkModel.Basket = basketModel;
                //        basketProductLinkModel.Product = product;
                //        _context.BasketProductLinks.Add(basketProductLinkModel);
                //        _context.SaveChanges();

                //    }

                //    return true;
                //}
            }
            catch
            {
                return false;
            }
        }

        private bool BasketProductLinkModelExists(int id)
        {
            return _context.BasketProductLinkModel.Any(e => e.BasketId == id);
        }
    }
}
