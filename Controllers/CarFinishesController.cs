using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpressVoitures.Data;

namespace ExpressVoitures.Controllers
{
    public class CarFinishesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarFinishesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CarFinishes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Finitions.Include(c => c.Modele);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CarFinishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carFinish = await _context.Finitions
                .Include(c => c.Modele)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carFinish == null)
            {
                return NotFound();
            }

            return View(carFinish);
        }

        // GET: CarFinishes/Create
        public IActionResult Create()
        {
            ViewData["ModeleId"] = new SelectList(_context.Modeles, "Id", "Id");
            return View();
        }

        // POST: CarFinishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ModeleId,Nom")] CarFinish carFinish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carFinish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModeleId"] = new SelectList(_context.Modeles, "Id", "Id", carFinish.ModeleId);
            return View(carFinish);
        }

        // GET: CarFinishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carFinish = await _context.Finitions.FindAsync(id);
            if (carFinish == null)
            {
                return NotFound();
            }
            ViewData["ModeleId"] = new SelectList(_context.Modeles, "Id", "Id", carFinish.ModeleId);
            return View(carFinish);
        }

        // POST: CarFinishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModeleId,Nom")] CarFinish carFinish)
        {
            if (id != carFinish.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carFinish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarFinishExists(carFinish.Id))
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
            ViewData["ModeleId"] = new SelectList(_context.Modeles, "Id", "Id", carFinish.ModeleId);
            return View(carFinish);
        }

        // GET: CarFinishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carFinish = await _context.Finitions
                .Include(c => c.Modele)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carFinish == null)
            {
                return NotFound();
            }

            return View(carFinish);
        }

        // POST: CarFinishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carFinish = await _context.Finitions.FindAsync(id);
            if (carFinish != null)
            {
                _context.Finitions.Remove(carFinish);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarFinishExists(int id)
        {
            return _context.Finitions.Any(e => e.Id == id);
        }
    }
}
