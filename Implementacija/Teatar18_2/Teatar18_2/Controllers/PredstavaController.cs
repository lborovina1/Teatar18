using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Teatar18_2.Data;
using Teatar18_2.Models;

namespace Teatar18_2.Controllers
{
    public class PredstavaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PredstavaController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string TruncateByWords(string text, int wordLimit)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var words = text.Split(' ');
            if (words.Length <= wordLimit)
                return text;

            return string.Join(" ", words.Take(wordLimit)) + "...";
        }

        // GET: Izvedba
        [Authorize(Roles = "Administrator, Zaposlenik")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.Predstava
                                                      .ToListAsync();

            var model = applicationDbContext.Select(i => new Predstava
            {
                ID = i.ID,
                naziv = i.naziv,
                glumci = i.glumci,
                scenaristi = i.scenaristi,
                reziseri = i.reziseri,
                scenografi = i.scenografi,
                zanr = i.zanr,
                opis = TruncateByWords(i.opis, 20),
                poster = i.poster,
                trajanje = i.trajanje,
                uRepertoaru = i.uRepertoaru
            }).ToList();

            return View("Index", model);
        }

        // GET: Predstava/Details/5
        [Authorize(Roles = "Administrator, Zaposlenik")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var predstava = await _context.Predstava
                .FirstOrDefaultAsync(m => m.ID == id);
            if (predstava == null)
            {
                return NotFound();
            }

            return View(predstava);
        }

        // GET: Predstava/Create
        [Authorize(Roles = "Administrator, Zaposlenik")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Predstava/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,naziv,glumci,scenaristi,reziseri,scenografi,zanr,opis,poster,trajanje,uRepertoaru")] Predstava predstava)
        {
            if (ModelState.IsValid)
            {
                _context.Add(predstava);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(predstava);
        }

        // GET: Predstava/Edit/5
        [Authorize(Roles = "Administrator, Zaposlenik")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var predstava = await _context.Predstava.FindAsync(id);
            if (predstava == null)
            {
                return NotFound();
            }
            return View(predstava);
        }

        // POST: Predstava/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Zaposlenik")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,naziv,glumci,scenaristi,reziseri,scenografi,zanr,opis,poster,trajanje,uRepertoaru")] Predstava predstava)
        {
            if (id != predstava.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(predstava);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PredstavaExists(predstava.ID))
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
            return View(predstava);
        }

        // GET: Predstava/Delete/5
        [Authorize(Roles = "Administrator, Zaposlenik")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var predstava = await _context.Predstava
                .FirstOrDefaultAsync(m => m.ID == id);
            if (predstava == null)
            {
                return NotFound();
            }

            return View(predstava);
        }

        // POST: Predstava/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Zaposlenik")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var predstava = await _context.Predstava.FindAsync(id);
            if (predstava != null)
            {
                _context.Predstava.Remove(predstava);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PredstavaExists(int id)
        {
            return _context.Predstava.Any(e => e.ID == id);
        }

        // Pomocna metoda koja se samo poziva iz dajPreporuke
        public async Task<double> izracunajProsjecnuOcjenu(int predstavaID)
        {
            var ocjene = await _context.Ocjena
                                       .Where(e => e.IDPredstave == predstavaID)
                                       .Select(e => e.ocjena)
                                       .ToListAsync();

            if (ocjene.Count == 0)
            {
                return 0;
            }
            
            double prosjecnaOcjena = ocjene.Average();
            return Math.Round(prosjecnaOcjena, 2);
        }

        // Poziva se iz HomeControllera za prikaz preporuka na home page-u
        public async Task<List<Predstava>> dajPreporuke()
        {
            var predstave = await _context.Predstava.ToListAsync();
            var predstaveOcjene = new List<(Predstava predstava, double prosjecnaOcjena)>();

            foreach(var predstava in predstave)
            {
                double prosjecnaOcjena = await izracunajProsjecnuOcjenu(predstava.ID);
                predstaveOcjene.Add((predstava, prosjecnaOcjena));
            }

            var najbolje = predstaveOcjene
                .OrderByDescending(e => e.prosjecnaOcjena)
                .Take(3)
                .Select(e => e.predstava)
                .ToList();

            return najbolje;
        }

        public async Task<IActionResult> Rezervisi(int predstavaID)
        {
            if (User.IsInRole("Korisnik"))
            {
                var izvedbe = await _context.Izvedba
                    .Where(i => i.IDPredstave == predstavaID && i.vrijeme > DateTime.Now)
                    .OrderBy(i => i.vrijeme)
                    .ToListAsync();

                return View(izvedbe);
            }
            else
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
        }
    }
}
