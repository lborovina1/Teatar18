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
    public class NewsletterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewsletterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Newsletter
        public async Task<IActionResult> Index()
        {
            return View(await _context.Newsletter.ToListAsync());
        }

        // GET: Newsletter/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsletter = await _context.Newsletter
                .FirstOrDefaultAsync(m => m.ID == id);
            if (newsletter == null)
            {
                return NotFound();
            }

            return View(newsletter);
        }

        // GET: Newsletter/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Newsletter/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,informacija,datumSlanja")] Newsletter newsletter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(newsletter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newsletter);
        }

        // GET: Newsletter/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsletter = await _context.Newsletter.FindAsync(id);
            if (newsletter == null)
            {
                return NotFound();
            }
            return View(newsletter);
        }

        // POST: Newsletter/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,informacija,datumSlanja")] Newsletter newsletter)
        {
            if (id != newsletter.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newsletter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsletterExists(newsletter.ID))
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
            return View(newsletter);
        }

        // GET: Newsletter/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsletter = await _context.Newsletter
                .FirstOrDefaultAsync(m => m.ID == id);
            if (newsletter == null)
            {
                return NotFound();
            }

            return View(newsletter);
        }

        // POST: Newsletter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newsletter = await _context.Newsletter.FindAsync(id);
            if (newsletter != null)
            {
                _context.Newsletter.Remove(newsletter);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsletterExists(int id)
        {
            return _context.Newsletter.Any(e => e.ID == id);
        }
    }
}
