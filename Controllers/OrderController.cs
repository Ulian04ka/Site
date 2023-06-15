using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopKnitting.Models.DataModel;
using ShopKnitting.Models.HelpModels;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ShopKnitting.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderModel.ToListAsync());
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderModel = await _context.OrderModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderModel == null)
            {
                return NotFound();
            }

            return View(orderModel);
        }
        [HttpGet]
        public IActionResult InProcess()
        {
            return View();
        }

        // GET: Order/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TotalPrice,CreationDate,ConfirmationDate,IssueDate,LastName,Name,Address,Phone,Status")] OrderModel orderModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderModel);
        }
        //public void addPic()
        //{

        //    string fileName = @"D:\Документы (D)\Студия проекты\ShopKnitting\wwwroot\servicedata\car_setting.png";
        //    string extFile = Path.GetExtension(fileName);
        //    if (extFile.Contains(".png") || extFile.Contains(".jpg") ||
        //        extFile.Contains(".bmp"))
        //    {
        //        var image = Image.Load(upload.OpenReadStream());
        //        image.Mutate(x => x.Resize(ImageWidth, ImageHeight));
        //        string imgGuid = Guid.NewGuid().ToString();
        //        string today = DateTime.Today.ToString("yyyy-MM-dd");
        //        fileName = today + "-" + imgGuid + extFile;

        //        string path = _webHostEnvironment.WebRootPath + "/productImg/" + fileName;
        //    }
        //}
        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderModel = await _context.OrderModel.FindAsync(id);
            if (orderModel == null)
            {
                return NotFound();
            }
            return View(orderModel);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TotalPrice,CreationDate,ConfirmationDate,IssueDate,LastName,Name,Address,Phone,Status")] OrderModel orderModel)
        {
            if (id != orderModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderModelExists(orderModel.Id))
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
            return View(orderModel);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderModel = await _context.OrderModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderModel == null)
            {
                return NotFound();
            }

            return View(orderModel);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderModel = await _context.OrderModel.FindAsync(id);
            _context.OrderModel.Remove(orderModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderModelExists(int id)
        {
            return _context.OrderModel.Any(e => e.Id == id);
        }
    }
}
