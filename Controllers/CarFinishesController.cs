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
            var marques = _context.Marques.ToList();
            ViewBag.Marques = marques.Select(m => new { Id = m.Id, Nom = m.Nom }).ToList();
            ViewBag.Modeles = _context.Modeles.Select(model => new { Id = model.Id, Nom = model.Nom, MarqueId = model.MarqueId }).ToList();

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

        /// <summary>
        /// Méthode permettant de vérifier si le nom de la finition est déjà présent en base de données
        /// </summary>
        /// <param name="carModel"></param>
        /// <param name="editing"></param>
        /// <returns></returns>
        private async Task<IActionResult> CheckExistingFinish(CarFinish carFinish, bool editing = false)
        {
            var existingFinish = await _context.Finitions
                    .FirstOrDefaultAsync(m => m.ModeleId == carFinish.ModeleId && m.Nom == carFinish.Nom);

            if (existingFinish != null)
            {
                ModelState.AddModelError("Nom", "Une finition avec ce nom existe déjà pour ce modèle.");
                if (editing)
                    ViewData["ModeleId"] = new SelectList(_context.Modeles, "Id", "Nom", carFinish.ModeleId);
                else
                {
                    var marques = _context.Marques.ToList();
                    ViewBag.Marques = marques.Select(m => new { Id = m.Id, Nom = m.Nom }).ToList();
                    ViewBag.Modeles = _context.Modeles.Select(model => new { Id = model.Id, Nom = model.Nom, MarqueId = model.MarqueId }).ToList();
                }
                return View(carFinish);
            }
            if (editing)
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
            }
            else
            {
                _context.Add(carFinish);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: CarFinishes/Create
        public IActionResult Create()
        {
            var marques = _context.Marques.ToList();
            ViewBag.Marques = marques.Select(m => new { Id = m.Id, Nom = m.Nom }).ToList();
            ViewBag.Modeles = _context.Modeles.Select(model => new { Id = model.Id, Nom = model.Nom, MarqueId = model.MarqueId }).ToList();
            return View();
        }

        // POST: CarFinishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModeleId,Nom")] CarFinish carFinish)
        {
            ModelState.Remove("Modele");
            if (ModelState.IsValid)
            {
                return await CheckExistingFinish(carFinish);
            }
            var marques = _context.Marques.ToList();
            ViewBag.Marques = marques.Select(m => new { Id = m.Id, Nom = m.Nom }).ToList();
            ViewBag.Modeles = _context.Modeles.Select(model => new { Id = model.Id, Nom = model.Nom, MarqueId = model.MarqueId }).ToList();
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
            ViewData["ModeleId"] = new SelectList(_context.Modeles, "Id", "Nom", carFinish.ModeleId);
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
