using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            var appDbContext = _context.BasketModel.Include(b => b.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Basket/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basketModel = await _context.BasketModel
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (basketModel == null)
            {
                return NotFound();
            }

            return View(basketModel);
        }

        // GET: Basket/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Basket/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId")] BasketModel basketModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(basketModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", basketModel.UserId);
            return View(basketModel);
        }

        // GET: Basket/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basketModel = await _context.BasketModel.FindAsync(id);
            if (basketModel == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", basketModel.UserId);
            return View(basketModel);
        }

        // POST: Basket/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId")] BasketModel basketModel)
        {
            if (id != basketModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(basketModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BasketModelExists(basketModel.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", basketModel.UserId);
            return View(basketModel);
        }

        // GET: Basket/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basketModel = await _context.BasketModel
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (basketModel == null)
            {
                return NotFound();
            }

            return View(basketModel);
        }

        // POST: Basket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var basketModel = await _context.BasketModel.FindAsync(id);
            _context.BasketModel.Remove(basketModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BasketModelExists(int id)
        {
            return _context.BasketModel.Any(e => e.Id == id);
        }
    }
}
