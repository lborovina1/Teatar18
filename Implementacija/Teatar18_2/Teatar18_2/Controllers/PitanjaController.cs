using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Teatar18_2.Data;
using Teatar18_2.Models;

namespace Teatar18_2.Controllers
{
    public class PitanjaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PitanjaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pitanja
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pitanje.ToListAsync());
        }

        // GET: Pitanja/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pitanje = await _context.Pitanje
                .FirstOrDefaultAsync(m => m.ID == id);
            if (pitanje == null)
            {
                return NotFound();
            }

            return View(pitanje);
        }

        // GET: Pitanja/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pitanja/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,predmet,sadrzaj,datumPostavljanja,odgovoreno,datumOdgovora")] Pitanje pitanje)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pitanje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pitanje);
        }

        // GET: Pitanja/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pitanje = await _context.Pitanje.FindAsync(id);
            if (pitanje == null)
            {
                return NotFound();
            }
            return View(pitanje);
        }

        // POST: Pitanja/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,predmet,sadrzaj,datumPostavljanja,odgovoreno,datumOdgovora")] Pitanje pitanje)
        {
            if (id != pitanje.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pitanje);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PitanjeExists(pitanje.ID))
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
            return View(pitanje);
        }

        // GET: Pitanja/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pitanje = await _context.Pitanje
                .FirstOrDefaultAsync(m => m.ID == id);
            if (pitanje == null)
            {
                return NotFound();
            }

            return View(pitanje);
        }

        // POST: Pitanja/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pitanje = await _context.Pitanje.FindAsync(id);
            if (pitanje != null)
            {
                _context.Pitanje.Remove(pitanje);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PitanjeExists(int id)
        {
            return _context.Pitanje.Any(e => e.ID == id);
        }
    }
}
