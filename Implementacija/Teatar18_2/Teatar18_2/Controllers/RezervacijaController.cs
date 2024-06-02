using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Teatar18_2.Data;
using Teatar18_2.Models;
using Teatar18_2.Services;

namespace Teatar18_2.Controllers
{
    public class RezervacijaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;
        private readonly RezervacijaService _rezervacijaService;

        public RezervacijaController(
            ApplicationDbContext context,
            UserManager<Korisnik> userManager,
            RezervacijaService rezervacijaService)
        {
            _context = context;
            _userManager = userManager;
            _rezervacijaService = rezervacijaService;
        }

        // GET: Rezervacija
        public async Task<IActionResult> Index()
        {
            var rezervacije = await _context.Rezervacija
                .Include(r => r.Izvedba)
                .ToListAsync();

            return View(rezervacije);
        }

        // GET: Rezervacija/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija
                .Include(r => r.Izvedba)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        // GET: Rezervacija/Create
        public IActionResult KreirajRezervaciju(int IDIzvedbe)       
        {
            var rezervacija = new Rezervacija { IDIzvedbe = IDIzvedbe };
            return View("Create", rezervacija);
        }

        // POST: Rezervacija/Create 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KreirajRezervaciju([Bind("ID,IDIzvedbe,kupovina,brojKarata")] Rezervacija rezervacija)
        {
            //==korisnik koji je kreirao rezervaciju==

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Nije pronadjen korisnik.");
            }

            rezervacija.IDKorisnika = user;

            //==provjera i zauzimanje karata (+ izmjena u izvedbi)==

            var izvedba = await _context.Izvedba
                .Where(i => i.ID == rezervacija.IDIzvedbe)
                .FirstOrDefaultAsync();

            if(izvedba == null)
            {
                return NotFound("Nije pronadjena izvedba.");
            }

            if(izvedba.brojSlobodnihKarata < rezervacija.brojKarata)
            {
                //implementirati logiku za to (view s greskom i sl)
                //privremeno rjesenje
                return View("NedovoljnoKarata");
            }

            var slobodneKarte = await _context.Karta
                .Where(k => k.IDIzvedbe == izvedba.ID && k.IDRezervacije == null)
                .OrderBy(k => k.ID)
                .Take(rezervacija.brojKarata)
                .ToListAsync();

            if (slobodneKarte == null)
            {
                return NotFound("Nisu pronadjene karte.");
            }

            //========obracun popusta========

            if(Enumerable.Range(user.brojKupljenihKarata + 1, rezervacija.brojKarata).Any(x => x % 5 == 0)){
                rezervacija.popust = 0.2;
            }

            if (ModelState.IsValid)
            {
                _context.Add(rezervacija);
                await _context.SaveChangesAsync();

                //==zauzmi karte i smanji broj slobodnih==

                foreach (var karta in slobodneKarte)
                {
                    karta.IDRezervacije = rezervacija.ID;
                }

                izvedba.brojSlobodnihKarata -= rezervacija.brojKarata;

                await _context.SaveChangesAsync();

                //==kupovina - preusmjeri==
                if (rezervacija.kupovina)
                {
                    return RedirectToAction(nameof(PlatiRezervaciju), new { id = rezervacija.ID });
                }

                return RedirectToAction(nameof(Index));     //vidjeti sta da vraca
            }

            return View(rezervacija);
        }

        // GET: Rezervacija/Edit/5 
        public async Task<IActionResult> Edit(int? id)      //ne koristi je korisnik
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija.FindAsync(id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            ViewData["IDIzvedbe"] = new SelectList(_context.Izvedba, "ID", "ID", rezervacija.IDIzvedbe);
            return View(rezervacija);
        }

        // POST: Rezervacija/Edit/5 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,IDIzvedbe,kupovina,brojKarata,popust,aktivna")] Rezervacija rezervacija)
        {
            if (id != rezervacija.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervacija);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervacijaExists(rezervacija.ID))
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
            ViewData["IDIzvedbe"] = new SelectList(_context.Izvedba, "ID", "ID", rezervacija.IDIzvedbe);
            return View(rezervacija);
        }

        // GET: Rezervacija/Delete/5
        public async Task<IActionResult> OtkaziRezervaciju(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija
                .Include(r => r.Izvedba)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            return View("Delete", rezervacija);
        }

        // POST: Rezervacija/Delete/5 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OtkaziRezervacijuConfirmed(int id)
        {
            if (await _rezervacijaService.OtkaziRezervaciju(id))
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        private bool RezervacijaExists(int id)
        {
            return _context.Rezervacija.Any(e => e.ID == id);
        }
        public async Task<IActionResult> PlatiRezervaciju(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija.FindAsync(id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View("Kupovina", rezervacija);
        }

        [HttpPost]
        public async Task<IActionResult> PlatiRezervaciju(int id, [Bind("ID,IDIzvedbe,kupovina,brojKarata,popust,aktivna")] Rezervacija rezervacija)
        {
            if (id != rezervacija.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (await _rezervacijaService.PlatiRezervaciju(id) == false)
                {
                    return View();
                }

                return RedirectToAction(nameof(Index));     //vidjeti sta da vraca
            }
            
            return View("Kupovina", rezervacija);
        }
    }
}
