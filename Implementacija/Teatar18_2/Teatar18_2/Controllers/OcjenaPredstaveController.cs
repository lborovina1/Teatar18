using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Teatar18_2.Data;

namespace Teatar18_2.Controllers
{
    public class OcjenaPredstaveController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OcjenaPredstaveController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OcjenaPredstave
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Ocjena.Include(o => o.Predstava);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OcjenaPredstave/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ocjena = await _context.Ocjena
                .Include(o => o.Predstava)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (ocjena == null)
            {
                return NotFound();
            }

            return View(ocjena);
        }

        // GET: OcjenaPredstave/Create
        public IActionResult Create()
        {
            ViewData["IDPredstave"] = new SelectList(_context.Predstava, "ID", "ID");
            return View();
        }

        // POST: OcjenaPredstave/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,IDPredstave,ocjena")] Ocjena ocj)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ocj);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IDPredstave"] = new SelectList(_context.Predstava, "ID", "ID", ocj.IDPredstave);
            return View(ocj);
        }

        // GET: OcjenaPredstave/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ocjena = await _context.Ocjena.FindAsync(id);
            if (ocjena == null)
            {
                return NotFound();
            }
            ViewData["IDPredstave"] = new SelectList(_context.Predstava, "ID", "ID", ocjena.IDPredstave);
            return View(ocjena);
        }

        // POST: OcjenaPredstave/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,IDPredstave,ocjena")] Ocjena ocj)
        {
            if (id != ocj.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ocj);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OcjenaExists(ocj.ID))
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
            ViewData["IDPredstave"] = new SelectList(_context.Predstava, "ID", "ID", ocj.IDPredstave);
            return View(ocj);
        }

        // GET: OcjenaPredstave/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ocjena = await _context.Ocjena
                .Include(o => o.Predstava)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (ocjena == null)
            {
                return NotFound();
            }

            return View(ocjena);
        }

        // POST: OcjenaPredstave/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ocjena = await _context.Ocjena.FindAsync(id);
            if (ocjena != null)
            {
                _context.Ocjena.Remove(ocjena);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OcjenaExists(int id)
        {
            return _context.Ocjena.Any(e => e.ID == id);
        }
    }
}
