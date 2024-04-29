using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpressVoitures.Data;
using Microsoft.CodeAnalysis.Differencing;

namespace ExpressVoitures.Controllers
{
    public class CarModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CarModels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Modeles.Include(c => c.Marque);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CarModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carModel = await _context.Modeles
                .Include(c => c.Marque)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carModel == null)
            {
                return NotFound();
            }

            return View(carModel);
        }

        /// <summary>
        /// Méthode permettant de vérifier si le nom du modèle est déjà présent en base de données
        /// </summary>
        /// <param name="carModel"></param>
        /// <param name="editing"></param>
        /// <returns></returns>
        private async Task<IActionResult> CheckExistingModel(CarModel carModel, bool editing = false)
        {
            var existingModel = await _context.Modeles
                    .FirstOrDefaultAsync(m => m.MarqueId == carModel.MarqueId && m.Nom == carModel.Nom);

            if (existingModel != null)
            {
                ModelState.AddModelError("Nom", "Un modèle avec ce nom existe déjà pour cette marque.");
                if (editing)
                    ViewData["MarqueId"] = new SelectList(_context.Marques, "Id", "Nom", carModel.MarqueId);
                else
                    ViewBag.Marque = new SelectList(_context.Marques, "Id", "Nom");
                return View(carModel);
            }
            if (editing)
            {
                try
                {
                    _context.Update(carModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarModelExists(carModel.Id))
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
                _context.Add(carModel);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: CarModels/Create
        public IActionResult Create()
        {
            ViewBag.Marque = new SelectList(_context.Marques, "Id", "Nom");
            return View();
        }

        // POST: CarModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MarqueId,Nom")] CarModel carModel)
        {
            ModelState.Remove("Marque");
            if (ModelState.IsValid)
            {
                return await CheckExistingModel(carModel, false);
            }
            ViewBag.Marque = new SelectList(_context.Marques, "Id", "Nom");
            return View(carModel);
        }

        // GET: CarModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carModel = await _context.Modeles.FindAsync(id);
            if (carModel == null)
            {
                return NotFound();
            }
            ViewData["MarqueId"] = new SelectList(_context.Marques, "Id", "Nom", carModel.MarqueId);
            return View(carModel);
        }

        // POST: CarModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MarqueId,Nom")] CarModel carModel)
        {
            ModelState.Remove("Marque");
            if (id != carModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                return await CheckExistingModel(carModel, true);
            }
            ViewData["MarqueId"] = new SelectList(_context.Marques, "Id", "Nom", carModel.MarqueId);
            return View(carModel);
        }

        // GET: CarModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carModel = await _context.Modeles
                .Include(c => c.Marque)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carModel == null)
            {
                return NotFound();
            }

            return View(carModel);
        }

        // POST: CarModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carModel = await _context.Modeles.FindAsync(id);
            if (carModel != null)
            {
                _context.Modeles.Remove(carModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarModelExists(int id)
        {
            return _context.Modeles.Any(e => e.Id == id);
        }
    }
}
