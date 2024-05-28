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
    public class PredstavaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PredstavaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Predstava
        public async Task<IActionResult> Index()
        {
            return View(await _context.Predstava.ToListAsync());
        }

        // GET: Predstava/Details/5
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
    }
}
