using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Teatar18_2.Data;
using Teatar18_2.Models;
using Teatar18_2.Services;

namespace Teatar18_2.Areas.Identity.Pages.Account.Manage
{
    public class OtkazivanjeRezervacijeModel : PageModel
    {
        private readonly UserManager<Korisnik> _userManager;
        private readonly SignInManager<Korisnik> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly RezervacijaService _rezervacijaService;

        public OtkazivanjeRezervacijeModel(
            UserManager<Korisnik> userManager,
            SignInManager<Korisnik> signInManager,
            ApplicationDbContext context,
            RezervacijaService rezervacijaService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _rezervacijaService = rezervacijaService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public Rezervacija rezervacija { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            rezervacija = await _context.Rezervacija
                .Include(r => r.IDKorisnika)
                .FirstOrDefaultAsync(r => r.ID == id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (await _rezervacijaService.OtkaziRezervaciju(rezervacija.ID) == false)
            {
                return Page();
            }

            return RedirectToPage("AktivneRezervacije");
        }
    }
}
