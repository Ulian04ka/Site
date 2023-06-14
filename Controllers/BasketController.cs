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
        [HttpGet]
        public IActionResult Index()
        {
            List<BasketProductLinkModel> basketProductLinks = new List<BasketProductLinkModel>();
            List<ProductModel> products = _context.ProductModels.Include(p => p.Brand).Include(p=> p.Images).ToList();
            Dictionary<ProductModel, int> basketFromCookie =
                BasketHelper.GetBasketFromCookie(Request, Response)
                    .ToDictionary(kvp => products.Find(x => x.Id == kvp.Key), kvp => kvp.Value);

            Dictionary<ProductModel, int> actualBasket = new Dictionary<ProductModel, int>();
            foreach (var product_kvp in basketFromCookie)
            {
                ProductModel productModel = _context.ProductModels.FirstOrDefault(p => p.Id == product_kvp.Key.Id);
                if (productModel != null)
                {
                    basketProductLinks.Add(new BasketProductLinkModel(productModel, product_kvp.Value));
                    actualBasket.Add(productModel, product_kvp.Value);
                }
            }

            BasketHelper.RestructureBasket(actualBasket.ToDictionary(x => x.Key.Id, x => x.Value), Response, Request);
            return View(basketProductLinks);
        }
        [HttpGet]
        public int GetBasketProductCount()
        {

            int TotalCount = 0;
                var basket = BasketHelper.GetBasketFromCookie(Request, Response);

                foreach (var productKVP in basket)
                {
                    ProductModel product = _context.ProductModels.Include(p=> p.Brand).Include(p=>p.Images).ToList().Find(p => p.Id == productKVP.Key);
                    TotalCount += productKVP.Value;
                }

            return TotalCount;
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


            // BASKET: Remove product from basket
            try
            {
                    BasketHelper.RemoveFromCookieBasket(product_id, Request, Response);

                    return true;
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

                var basket = BasketHelper.GetBasketFromCookie(Request, Response);

                foreach (var productKVP in basket)
                {
                    ProductModel product = _context.ProductModels.ToList().Find(p => p.Id == productKVP.Key);
                    TotalCost += product.Cost * productKVP.Value;
                }

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


            // BASKET: Update count od product
            try
            {
                    BasketHelper.UpdateCount(product_id, product_count, Response, Request);

                    return true;
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
            ProductModel product = _context.ProductModels.Include(p=>p.Brand).Include(p=>p.Images).FirstOrDefault(p => p.Id == product_id);


            // BASKET: Add new product to basket
            try
            {
                    BasketHelper.AddToCookieBasket(product, Request, Response);

                    return true;
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
        [HttpGet]
        public IActionResult Clear()
        {            
                BasketHelper.RestructureBasket(new Dictionary<int, int>(), Response, Request);
           

            ViewData["ProductCountInShop"] = _context.ProductModels.Count();

            return RedirectToAction("Index", "Basket", new List<BasketProductLinkModel>());
        }
    }
}
