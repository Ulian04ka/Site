using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
            ProductListModel productListModel = new ProductListModel();
            productListModel.productList = _dbContext.ProductModels.Include(p => p.Brand).Include(c => c.Images);
            ViewData["WebRootPath"] = _webHostEnvironment.WebRootPath;
            ViewData["TotalProduct"] =productListModel.productList.Count();
            

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
    }
}
