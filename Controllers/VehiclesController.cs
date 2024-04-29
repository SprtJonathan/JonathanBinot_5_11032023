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
    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehiclesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vehicles.Include(v => v.Finition).Include(v => v.Marque).Include(v => v.Modele);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .Include(v => v.Finition)
                .Include(v => v.Marque)
                .Include(v => v.Modele)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            ViewData["FinitionId"] = new SelectList(_context.Finitions, "Id", "Nom");
            ViewData["MarqueId"] = new SelectList(_context.Marques, "Id", "Nom");
            ViewData["ModeleId"] = new SelectList(_context.Modeles, "Id", "Nom");
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehicle vehicle, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();

                foreach (var imageFile in images)
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Enregistrer le fichier sur le serveur
                        var filePath = "chemin/vers/le/dossier/ou/enregistrer/le/fichier"; // Spécifiez le chemin d'accès approprié
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var fullPath = Path.Combine(filePath, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // Enregistrer le chemin d'accès à l'image dans la base de données
                        var carImage = new CarImage
                        {
                            VehicleId = vehicle.Id,
                            ImageLink = fullPath // Enregistrez le chemin d'accès complet
                        };
                        _context.VehicleImages.Add(carImage);
                    }
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Si la validation échoue, rechargez les listes déroulantes et affichez à nouveau le formulaire avec les erreurs de validation
            ViewData["FinitionId"] = new SelectList(_context.Finitions, "Id", "Nom", vehicle.FinitionId);
            ViewData["MarqueId"] = new SelectList(_context.Marques, "Id", "Nom", vehicle.MarqueId);
            ViewData["ModeleId"] = new SelectList(_context.Modeles, "Id", "Nom", vehicle.ModeleId);
            return View(vehicle);
        }


        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            ViewData["FinitionId"] = new SelectList(_context.Finitions, "Id", "Nom", vehicle.FinitionId);
            ViewData["MarqueId"] = new SelectList(_context.Marques, "Id", "Nom", vehicle.MarqueId);
            ViewData["ModeleId"] = new SelectList(_context.Modeles, "Id", "Nom", vehicle.ModeleId);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CodeVIN,Annee,DateAchat,PrixAchat,DateDisponibiliteVente,PrixVente,DateVente,MarqueId,ModeleId,FinitionId,Description")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
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
            ViewData["FinitionId"] = new SelectList(_context.Finitions, "Id", "Nom", vehicle.FinitionId);
            ViewData["MarqueId"] = new SelectList(_context.Marques, "Id", "Nom", vehicle.MarqueId);
            ViewData["ModeleId"] = new SelectList(_context.Modeles, "Id", "Nom", vehicle.ModeleId);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .Include(v => v.Finition)
                .Include(v => v.Marque)
                .Include(v => v.Modele)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.Id == id);
        }
    }
}
