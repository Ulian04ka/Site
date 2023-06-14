using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopKnitting.Models.HelpModels
{
    public static class BasketHelper
    {
        public static Dictionary<int, int> GetBasketFromCookie(
            HttpRequest httpRequest,
            HttpResponse httpResponse
        )
        {
            if (httpRequest.Cookies.ContainsKey("BasketSaveProducts"))
            {
                try
                {
                    Dictionary<int, int> dict_int_int = JsonConvert.DeserializeObject<Dictionary<int, int>>(httpRequest.Cookies["BasketSaveProducts"]);
                    return dict_int_int;
                }
                catch
                {
                    SaveToCookies(new Dictionary<int, int> { }, httpResponse);
                    return new Dictionary<int, int> { };
                }
            }
            else
            {
                SaveToCookies(new Dictionary<int, int> { }, httpResponse);
                return new Dictionary<int, int> { };
            }
        }

        public static void UpdateCount(
            int product_id,
            int product_count,
            HttpResponse httpResponse,
            HttpRequest httpRequest
        )
        {
            Dictionary<int, int> basket = GetBasketFromCookie(httpRequest, httpResponse);
            if (basket.ContainsKey(product_id))
            {
                basket[product_id] = product_count;
            }
            else
            {
                basket.Add(product_id, product_count);
            }
            SaveToCookies(basket, httpResponse);
        }

        public static void RestructureBasket(
            Dictionary<int, int> productList,
            HttpResponse httpResponse,
            HttpRequest httpRequest
        )
        {
            httpResponse.Cookies.Delete("BasketSaveProducts");
            SaveToCookies(productList, httpResponse);
        }

        public static void AddToCookieBasket(
            ProductModel product,
            HttpRequest httpRequest,
            HttpResponse httpResponse
        )
        {
            if (httpRequest.Cookies.ContainsKey("BasketSaveProducts"))
            {
                Dictionary<int, int> basketSaveProducts = JsonConvert.DeserializeObject<Dictionary<int, int>>(httpRequest.Cookies["BasketSaveProducts"]);
                if (basketSaveProducts.ContainsKey(product.Id))
                {
                    basketSaveProducts[product.Id] += 1;
                }
                else
                {
                    basketSaveProducts.Add(product.Id, 1);
                }
                SaveToCookies(basketSaveProducts, httpResponse);
            }
            else
            {
                SaveToCookies(new Dictionary<int, int> { { product.Id, 1 } }, httpResponse);
            }
        }

        public static void RemoveFromCookieBasket(
            int product_id,
            HttpRequest httpRequest,
            HttpResponse httpResponse
        )
        {
            if (httpRequest.Cookies.ContainsKey("BasketSaveProducts"))
            {
                Dictionary<int, int> basketSaveProducts = JsonConvert.DeserializeObject<Dictionary<int, int>>(httpRequest.Cookies["BasketSaveProducts"]);

                if (basketSaveProducts.ContainsKey(product_id))
                {
                    basketSaveProducts.Remove(product_id);
                }

                SaveToCookies(basketSaveProducts, httpResponse);
            }
            else
            {
                SaveToCookies(new Dictionary<int, int> { }, httpResponse);
            }
        }

        private static void SaveToCookies(
            object Object,
            HttpResponse httpResponse
        )
        {
            var jsonObject = JsonConvert.SerializeObject(Object);
            httpResponse.Cookies.Append(
                "BasketSaveProducts",
                jsonObject,
                new CookieOptions()
                {
                    MaxAge = new TimeSpan(30, 0, 0, 0)
                }
            );
        }
    }
}
