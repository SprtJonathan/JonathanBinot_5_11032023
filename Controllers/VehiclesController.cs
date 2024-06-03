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
using ExpressVoitures.Models;

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
        public async Task<IActionResult> Index(VehicleFilters filters)
        {
            var query = _context.Vehicles
                .Include(v => v.Finition)
                .Include(v => v.Marque)
                .Include(v => v.Modele)
                .Include(v => v.Images)
                .Where(v => v.IsPublished == true)
                .AsQueryable();

            if (filters != null)
            {
                if (filters.MarqueId.HasValue)
                {
                    query = query.Where(v => v.MarqueId == filters.MarqueId.Value);
                }

                if (filters.ModeleId.HasValue)
                {
                    query = query.Where(v => v.ModeleId == filters.ModeleId.Value);
                }

                if (filters.FinitionId.HasValue)
                {
                    query = query.Where(v => v.FinitionId == filters.FinitionId.Value);
                }

                if (filters.Annee.HasValue)
                {
                    query = query.Where(v => v.Annee == filters.Annee.Value);
                }
                ViewBag.Filters = filters;
            }

            var marques = _context.Marques.ToList();
            ViewBag.Marques = marques.Select(m => new { Id = m.Id, Nom = m.Nom }).ToList();
            ViewBag.Modeles = _context.Modeles.Select(model => new { Id = model.Id, Nom = model.Nom, MarqueId = model.MarqueId }).ToList();
            ViewBag.Finitions = _context.Finitions.Select(f => new { Id = f.Id, Nom = f.Nom, ModeleId = f.ModeleId }).ToList();

            var vehicles = await query.ToListAsync();
            return View(vehicles);
        }


        // GET: Vehicles/AdminIndex
        [Authorize]
        public async Task<IActionResult> AdminIndex()
        {
            var applicationDbContext = _context.Vehicles
                .Include(v => v.Finition)
                .Include(v => v.Marque)
                .Include(v => v.Modele)
                .Include(v => v.Images);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Vehicles/Listing/5
        public async Task<IActionResult> Listing(int? id)
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

        // GET: Vehicles/Details/5
        [Authorize]
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

            var vehicle = await _context.Vehicles
                .Include(v => v.Images)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vehicle == null)
            {
                return NotFound();
            }

            var marques = _context.Marques.ToList();
            ViewBag.Marques = marques.Select(m => new { m.Id, m.Nom }).ToList();
            ViewBag.Modeles = _context.Modeles.Select(model => new { model.Id, model.Nom,model.MarqueId }).ToList();
            ViewBag.Finitions = _context.Finitions.Select(f => new { f.Id, f.Nom, f.ModeleId }).ToList();
            return View(vehicle);
        }


        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, Vehicle vehicle, List<IFormFile> images)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Marque");
            ModelState.Remove("Modele");
            ModelState.Remove("Finition");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();

                    if (images != null && images.Count > 0)
                    {
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
                    }
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

            var marques = _context.Marques.ToList();
            ViewBag.Marques = marques.Select(m => new { m.Id, m.Nom }).ToList();
            ViewBag.Modeles = _context.Modeles.Select(model => new { model.Id, model.Nom, model.MarqueId }).ToList();
            ViewBag.Finitions = _context.Finitions.Select(f => new { f.Id, f.Nom, f.ModeleId }).ToList();

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
            var vehicle = await _context.Vehicles.Include(v => v.Images).FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle != null)
            {
                foreach (var image in vehicle.Images.ToList())
                {
                    _context.VehicleImages.Remove(image);
                }

                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();

                var uploadDir = Path.Combine(_hostEnvironment.WebRootPath, "img/annonces", vehicle.Id.ToString());
                if (Directory.Exists(uploadDir))
                {
                    try
                    {
                        Directory.Delete(uploadDir, true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors de la suppression du répertoire : {ex.Message}");
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var image = await _context.VehicleImages.FindAsync(imageId);
            if (image != null)
            {
                var vehicleId = image.VehicleId;

                // Supprimer le fichier physique
                var filePath = Path.Combine(_hostEnvironment.WebRootPath, image.ImageLink.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Supprimer l'entrée de la base de données
                _context.VehicleImages.Remove(image);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Edit), new { id = vehicleId });
            }

            return NotFound();
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.Id == id);
        }
    }
}
