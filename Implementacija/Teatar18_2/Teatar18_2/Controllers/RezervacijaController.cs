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

namespace Teatar18_2.Controllers
{
    public class RezervacijaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public RezervacijaController(
            ApplicationDbContext context,
            UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Rezervacija
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Rezervacija.Include(r => r.Izvedba);
            return View(await applicationDbContext.ToListAsync());
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
        public IActionResult Create()       //treba da prima int IDIzvedbe
        {
            //var rezervacija = new Rezervacija { IDIzvedbe = IDIzvedbe };
            //return View(rezervacija);

            //probno
            ViewData["IDIzvedbe"] = new SelectList(_context.Izvedba, "ID", "ID");
            return View();
        }

        // POST: Rezervacija/Create ---------------------> kreirajRezervaciju()
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,IDIzvedbe,kupovina,brojKarata")] Rezervacija rezervacija)
        {
            //==korisnik koji je kreirao rezervaciju==

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Unable to load user.");
            }

            rezervacija.IDKorisnika = user;

            //========================================

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
                return RedirectToAction(nameof(Index));
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

            //=======================================================

            //========obracun popusta========

            if ((user.brojKupljenihKarata + rezervacija.brojKarata) % 5 == 0)     //povecati kod kupovine!!
            {
                rezervacija.popust = 0.2;
            }

            //===============================

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

                //========================================

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //==Errori ako ModelState nije valid==
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage); 
            }
            //====================================

            return View(rezervacija);
        }

        // GET: Rezervacija/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

            //ViewData["IDIzvedbe"] = new SelectList(_context.Izvedba, "ID", "ID", rezervacija.IDIzvedbe);
            //return View(rezervacija);
            return View("Kupovina", rezervacija);
        }

        // POST: Rezervacija/Edit/5 ---------------------> platiRezervaciju()
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    rezervacija.kupovina = true;
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

                //==postavi karte na placene==

                var rezervisaneKarte = await _context.Karta
                    .Where(k => k.IDRezervacije == rezervacija.ID)
                    .ToListAsync();

                if (rezervisaneKarte == null)
                {
                    return NotFound("Nisu pronadjene karte.");
                }

                foreach (var karta in rezervisaneKarte)
                {
                    karta.placena = true;
                }

                await _context.SaveChangesAsync();

                //============================

                return RedirectToAction(nameof(Index));
            }
            //ViewData["IDIzvedbe"] = new SelectList(_context.Izvedba, "ID", "ID", rezervacija.IDIzvedbe);
            //return View(rezervacija);
            return View("Kupovina", rezervacija);
        }

        // GET: Rezervacija/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Rezervacija/Delete/5 ---------------------> otkaziRezervaciju()
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacija = await _context.Rezervacija.FindAsync(id);
            if (rezervacija != null)
            {
                //==postavljanje karata na slobodne i edit izvedbe (brojSlobodnihKarata)==

                var izvedba = await _context.Izvedba
                    .Where(i => i.ID == rezervacija.IDIzvedbe)
                    .FirstOrDefaultAsync();

                if (izvedba == null)
                {
                    return NotFound("Nije pronadjena izvedba.");
                }

                var rezervisaneKarte = await _context.Karta
                    .Where(k => k.IDRezervacije == rezervacija.ID)
                    .ToListAsync();

                if (rezervisaneKarte == null)
                {
                    return NotFound("Nisu pronadjene karte.");
                }

                foreach (var karta in rezervisaneKarte)
                {
                    karta.IDRezervacije = null;
                }

                izvedba.brojSlobodnihKarata += rezervacija.brojKarata;

                await _context.SaveChangesAsync();

                //=======================================================

                _context.Rezervacija.Remove(rezervacija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RezervacijaExists(int id)
        {
            return _context.Rezervacija.Any(e => e.ID == id);
        }
    }
}
