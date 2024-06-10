using System;
using System.Collections.Generic;
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
    public class IzvedbaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IzvedbaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Izvedba
        [Authorize(Roles = "Administrator, Zaposlenik")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Izvedba.Include(i => i.Predstava);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Izvedba/Details/5
        [Authorize(Roles = "Administrator, Zaposlenik")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izvedba = await _context.Izvedba
                .Include(i => i.Predstava)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (izvedba == null)
            {
                return NotFound();
            }

            return View(izvedba);
        }

        // GET: Izvedba/Create
        [Authorize(Roles = "Administrator, Zaposlenik")]
        public IActionResult Create()
        {
            ViewData["IDPredstave"] = new SelectList(_context.Predstava, "ID", "naziv");
            return View();
        }

        // POST: Izvedba/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Zaposlenik")]
        public async Task<IActionResult> Create([Bind("ID,IDPredstave,vrijeme,brojSlobodnihKarata")] Izvedba izvedba)
        {
            if (ModelState.IsValid)
            {
                _context.Add(izvedba);
                await _context.SaveChangesAsync();

                // Dodaju se bazu i karte za izvedbu
                await generisiKarteZaIzvedbu(izvedba.ID, izvedba.brojSlobodnihKarata, 10);

                return RedirectToAction(nameof(Index));
            }
            ViewData["IDPredstave"] = new SelectList(_context.Predstava, "ID", "ID", izvedba.IDPredstave);
            return View(izvedba);
        }

        // GET: Izvedba/Edit/5
        [Authorize(Roles = "Administrator, Zaposlenik")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izvedba = await _context.Izvedba.FindAsync(id);
            if (izvedba == null)
            {
                return NotFound();
            }
            ViewData["IDPredstave"] = new SelectList(_context.Predstava, "ID", "naziv", izvedba.IDPredstave);
            return View(izvedba);
        }

        // POST: Izvedba/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Zaposlenik")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,IDPredstave,vrijeme,brojSlobodnihKarata")] Izvedba izvedba)
        {
            if (id != izvedba.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(izvedba);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IzvedbaExists(izvedba.ID))
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
            ViewData["IDPredstave"] = new SelectList(_context.Predstava, "ID", "ID", izvedba.IDPredstave);
            return View(izvedba);
        }

        // GET: Izvedba/Delete/5
        [Authorize(Roles = "Administrator, Zaposlenik")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izvedba = await _context.Izvedba
                .Include(i => i.Predstava)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (izvedba == null)
            {
                return NotFound();
            }

            return View(izvedba);
        }

        // POST: Izvedba/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Zaposlenik")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var izvedba = await _context.Izvedba.FindAsync(id);
            if (izvedba != null)
            {
                _context.Izvedba.Remove(izvedba);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IzvedbaExists(int id)
        {
            return _context.Izvedba.Any(e => e.ID == id);
        }

        public IActionResult pregledRepertoara()
        {
            var predstave = _context.Predstava.Where(p => p.uRepertoaru).ToList();

            return View("Repertoar", predstave);
        }

        // Poziva se iz repertoara za detaljniji pregled jedne predstave
        public IActionResult pregledPredstave(int predstavaID)
        {
            var predstava = _context.Predstava.Find(predstavaID);
            if (predstava == null)
            {
                return NotFound();
            }
            return View("Predstava", predstava);
        }

        [Authorize(Roles = "Administrator, Zaposlenik")]
        public async Task generisiKarteZaIzvedbu(int izvedbaID, int brojKarata, double cijena)
        {
            var karteZaDodati = new List<Karta>();

            for (int i = 1; i <= brojKarata; i++)
            {
                var novaKarta = new Karta
                {
                    IDIzvedbe = izvedbaID,
                    sjediste = i,
                    cijena = cijena,
                    placena = false,
                    IDRezervacije = null
                };
                karteZaDodati.Add(novaKarta);
            }

            _context.Karta.AddRange(karteZaDodati);
            await _context.SaveChangesAsync();
        }

        // ViewBag prosljedjuje u IzmjenaRepertoara view listu predstava
        // Poziv metode kada se odabere izmjena repertoara
        [Authorize(Roles = "Administrator, Zaposlenik")]
        public async Task<IActionResult> izmjenaRepertoara()
        {
            var predstave = await _context.Predstava.ToListAsync();
            foreach(var predstava in predstave)
            {
                predstava.uRepertoaru = false;
            }
            await _context.SaveChangesAsync();

            ViewBag.Predstava = predstave;
            return View("IzmjenaRepertoara");
        }

        // Poziv metode iz multi-select dropdown liste
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Zaposlenik")]
        public async Task<IActionResult> izmjenaRepertoara(int[] odabranePredstaveID)
        {
            if(odabranePredstaveID != null)
            {
                foreach(var predstavaID in odabranePredstaveID)
                {
                    var predstava = await _context.Predstava.FindAsync(predstavaID);
                    if(predstava != null)
                    {
                        predstava.uRepertoaru = true;
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("pregledRepertoara");
        }

        [HttpGet]
        public IActionResult dajNajranijuIzvedbu(int PredstavaID)
        {
            try
            {
                var izvedbe = _context.Izvedba
                                      .Where(p => p.IDPredstave == PredstavaID && p.vrijeme > DateTime.Now)
                                      .Select(p => p.vrijeme)
                                      .ToList();

                if (!izvedbe.Any())
                {
                    return Json("Nema izvedbi");
                }

                var najranijaIzvedba = izvedbe.Min();
                return Json(najranijaIzvedba.ToString("dd.MM.yyyy HH:mm")); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
