using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpressVoitures.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;

namespace ExpressVoitures.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public VehiclesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vehicles
                .Include(v => v.Finition)
                .Include(v => v.Marque)
                .Include(v => v.Modele)
                .Include(v => v.Images)
                .Where(v => v.IsPublished == true);
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
                .Include(v => v.Images)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        [Authorize]
        public IActionResult Create()
        {
            var marques = _context.Marques.ToList();
            ViewBag.Marques = marques.Select(m => new { Id = m.Id, Nom = m.Nom }).ToList();
            ViewBag.Modeles = _context.Modeles.Select(model => new { Id = model.Id, Nom = model.Nom, MarqueId = model.MarqueId }).ToList();
            ViewBag.Finitions = _context.Finitions.Select(f => new { Id = f.Id, Nom = f.Nom, ModeleId = f.ModeleId }).ToList();

            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Vehicle vehicle, List<IFormFile> images)
        {
            ModelState.Remove("Marque");
            ModelState.Remove("Modele");
            ModelState.Remove("Finition");
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();

                // Créer le dossier pour stocker les images
                var uploadDir = Path.Combine(_hostEnvironment.WebRootPath, "img/annonces", vehicle.Id.ToString());
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // Sauvegarder les images
                foreach (var image in images)
                {
                    if (image != null && image.Length > 0)
                    {
                        var filePath = Path.Combine(uploadDir, image.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        // Ajouter l'image à la base de données
                        var carImage = new CarImage
                        {
                            VehicleId = vehicle.Id,
                            ImageLink = $"/img/annonces/{vehicle.Id}/{image.FileName}"
                        };
                        _context.VehicleImages.Add(carImage);
                    }
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Si la validation échoue, rechargez les listes déroulantes et affichez à nouveau le formulaire avec les erreurs de validation
            var marques = _context.Marques.ToList();
            ViewBag.Marques = marques.Select(m => new { Id = m.Id, Nom = m.Nom }).ToList();
            ViewBag.Modeles = _context.Modeles.Select(model => new { Id = model.Id, Nom = model.Nom, MarqueId = model.MarqueId }).ToList();
            ViewBag.Finitions = _context.Finitions.Select(f => new { Id = f.Id, Nom = f.Nom, ModeleId = f.ModeleId }).ToList();
            return View(vehicle);
        }


        // GET: Vehicles/Edit/5
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
