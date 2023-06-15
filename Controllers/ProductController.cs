using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopKnitting.Models;
using ShopKnitting.Models.DataModel;
using ShopKnitting.Models.HelpModels;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Newtonsoft.Json;
using System.Collections;
using Microsoft.AspNetCore.Identity;

namespace ShopKnitting.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        const int ImageWidth = 1000;
        const int ImageHeight = 1000;
        public ProductController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.ProductModels.Include(p => p.Brand);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["WebRootPath"] = _webHostEnvironment.WebRootPath;
            var productModel = await _context.ProductModels
                .Include(p => p.Brand).Include(p=>p.Images)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productModel == null)
            {
                return NotFound();
            }

            return View(productModel);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.BrandModel, "Id", "Id");
            ViewData["Brand"] = new SelectList(_context.BrandModel, "Name", "Name");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //public async Task<IActionResult> Create([Bind("Id,BrandId,Model,Year,CarBody,Description,Cost,CountInStack")] ProductModel productModel)
        public async Task<IActionResult> Create(ProductModel productModel, IFormFile upload)
        {
            productModel.Brand = _context.BrandModel.Where(c => c.Name == (string)Request.Form["Brand"]).FirstOrDefault();
            productModel.BrandId = productModel.Brand.Id;
            ImageModel imageModel = new();
            if (upload != null)
            {
                string fileName = Path.GetFileName(upload.FileName);
                string extFile = Path.GetExtension(fileName);
                if (extFile.Contains(".png") || extFile.Contains(".jpg") ||
                    extFile.Contains(".bmp"))
                {
                    var image = Image.Load(upload.OpenReadStream());
                    image.Mutate(x => x.Resize(ImageWidth, ImageHeight));
                    string imgGuid = Guid.NewGuid().ToString();
                    string today = DateTime.Today.ToString("yyyy-MM-dd");
                    fileName = today + "-" + imgGuid + extFile;

                    string path = _webHostEnvironment.WebRootPath + "/servicedata/" + fileName;
                    image.Save(path);
                    imageModel.Path = fileName;
                    _context.ImageModel.Add(imageModel);
                await _context.SaveChangesAsync();
                }
            }
            if (ModelState.IsValid)
            {
                productModel.Images = imageModel;
                _context.Add(productModel);
                await _context.SaveChangesAsync();
                imageModel.ProductId =productModel.Id;
                _context.ImageModel.Update(imageModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productModel);
        }

        // GET: Product/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel =  _context.ProductModels.Include(p=> p.Brand).Include(p=> p.Images).FirstOrDefault(c=> c.Id == id);
            if (productModel == null)
            {
                return NotFound();
            }
            ViewData["Brand"] = new SelectList(_context.BrandModel, "Name", "Name", productModel.Brand);
            return View(productModel);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BrandId,Model,Year,CarBody,Description,Cost,CountInStack")] ProductModel productModel, IFormFile upload)
        {
            if (id != productModel.Id)
            {
                return NotFound();
            }
            productModel.Brand = _context.BrandModel.Where(c => c.Name == (string)Request.Form["Brand"]).FirstOrDefault();
            productModel.BrandId = productModel.Brand.Id;
            bool ChangeImg = true;
            ImageModel imageModel = new();
            if (upload != null)
            {
                string fileName = Path.GetFileName(upload.FileName);
                if (productModel.Images.Path != fileName)
                {
                    string extFile = Path.GetExtension(fileName);
                    if (extFile.Contains(".png") || extFile.Contains(".jpg") ||
                        extFile.Contains(".bmp"))
                    {
                        var image = Image.Load(upload.OpenReadStream());
                        image.Mutate(x => x.Resize(ImageWidth, ImageHeight));
                        string imgGuid = Guid.NewGuid().ToString();
                        string today = DateTime.Today.ToString("yyyy-MM-dd");
                        fileName = today + "-" + imgGuid + extFile;

                        string path = _webHostEnvironment.WebRootPath + "/servicedata/" + fileName;
                        image.Save(path);
                        imageModel.Path = fileName;
                    }
                }
                else ChangeImg = false;
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (ChangeImg)
                    {
                        productModel.Images = imageModel;
                        imageModel.ProductId = productModel.Id;
                        _context.ImageModel.Add(imageModel);
                    }
                    _context.Update(productModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductModelExists(productModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.BrandModel, "Id", "Id", productModel.BrandId);
            ViewData["Brand"] = new SelectList(_context.BrandModel, "Name", "Name");
            return View(productModel);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.ProductModels
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productModel == null)
            {
                return NotFound();
            }

            return View(productModel);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productModel = await _context.ProductModels.FindAsync(id);
            _context.ProductModels.Remove(productModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductModelExists(int id)
        {
            return _context.ProductModels.Any(e => e.Id == id);
        }
    }
}
