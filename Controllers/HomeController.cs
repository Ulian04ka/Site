using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShopKnitting.Models;
using ShopKnitting.Models.DataModel;
using ShopKnitting.Models.HelpModels;

namespace ShopKnitting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _dbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            //var list = _dbContext.ProductModels.Where(c => c.Id==3).FirstOrDefault();
            //list.Images = _dbContext.ImageModel.Where(c => c.Id == 2).FirstOrDefault();
            //list.Brand = _dbContext.BrandModel.Where(c => c.Name.Contains("Toyota")).FirstOrDefault();
            //_dbContext.ProductModels.Update(list);
            //var list = _dbContext.ProductModels.Include(c=>c.Brand).Include(c=>c.Images).Where(c => c.Id == 1).FirstOrDefault();
            //list.Brand = _dbContext.BrandModel.Where(c => c.Name.Contains("Geely")).FirstOrDefault();
            //_dbContext.ProductModels.Update(list);
            //_dbContext.SaveChanges();
            //CreateBrand();
            ProductListModel productListModel = new();
            productListModel.ProductList = _dbContext.ProductModels.Include(p => p.Brand).Include(c => c.Images);
            ViewData["WebRootPath"] = _webHostEnvironment.WebRootPath;
            //ViewData["TotalProduct"] =_dbContext.BasketProductLinkModel.ToList().Count();// вывести кол-во товаров в корзине
            return View(productListModel);
        }
        private void CreateBrand()
        {
           // BrandModel brand = new BrandModel()
           // {
           //     Name = "Audi",
           //     Description = "Немецкая автомобилестроительная компания в составе концерна Volkswagen Group, специализирующаяся на выпуске автомобилей под маркой Audi."
           // };
           // _dbContext.BrandModel.Add(brand);
           //// _dbContext.SaveChanges();
           // ProductModel productModel = new ProductModel()
           // {
           //     Model = "Emgrand X7",
           //     Brand = (BrandModel)_dbContext.BrandModel.Where(c => c.Name == "Geely").ToList().FirstOrDefault(),
           //     CarBody = Models.Auxiliary.CarBodyEnum.OffRoadVehicle,
           //     Cost = 1449900,
           //     Year = 2019,
           //     CountInStack = 2,
           //     Images = image,
           //     Description = "Двигатель: бензин, 2.0 л; Мощность: 139 л.с.; Коробка передач: АКПП; Привод: передний; Цвет: белый; Руль: левый; Поколение: 1 поколение, 2 рестайлинг"
           // };
           // _dbContext.ProductModels.Add(productModel);
           // _dbContext.SaveChanges();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Search(string searchString)
        {
            ProductListModel productListModel = new();
            productListModel.ProductList = _dbContext.ProductModels.Include(p => p.Brand).Include(p => p.Images).Where(c => c.Brand.Name.Contains(searchString) || c.Model.Contains(searchString)).ToList();
            ViewBag.RealiseFind = true;
            ViewBag.RealiseFind = false;
            ViewData["WebRootPath"] = _webHostEnvironment.WebRootPath;
            ViewData["TotalProduct"] = _dbContext.BasketProductLinkModel.ToList().Count();
            ViewBag.resultFound = "По вашему запросу \"" + searchString + "\" найдено " + productListModel.ProductList.Count() + " записей";
            return View(productListModel);
        }
    }
}
//Dictionary<string, object> response = new Dictionary<string, object>();

//            MemoryStream stream = new MemoryStream();
//            Request.Body.CopyTo(stream);
//            stream.Position = 0;
//            using (StreamReader reader = new StreamReader(stream))
//            {
//                string requestBody = reader.ReadToEnd();
//                if (requestBody.Length > 0)
//                {
//                    response = JsonConvert.DeserializeObject<Dictionary<string, object>>(requestBody);
//                }
//            }

//            int product_id = int.Parse(response["product_id"].ToString());
//            ProductModel product = _dbContext.ProductModels.Include(p => p.Brand).Include(p => p.Images).FirstOrDefault(p => p.Id == product_id);


//            // BASKET: Add new product to basket
//            try
//            {
//                BasketHelper.AddToCookieBasket(product, Request, Response);

//                return true;
//            }
//            catch
//            {
//                return false;
//            }