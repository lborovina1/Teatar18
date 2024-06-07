using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Teatar18_2.Data;
using Teatar18_2.Models;
using Teatar18_2.Services;

namespace Teatar18_2.Areas.Identity.Pages.Account.Manage
{
    public class IstekleRezervacijeModel : PageModel
    {
        private readonly UserManager<Korisnik> _userManager;
        private readonly SignInManager<Korisnik> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly RezervacijaService _rezervacijaService;

        public IstekleRezervacijeModel(
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
        public List<Rezervacija> Rezervacije { get; set; }
        private async Task LoadAsync(Korisnik user)
        {
            var rezervacije = await _context.Rezervacija
                .Where(r => r.IDKorisnika == user && r.aktivna == false)
                .Include(r => r.Izvedba)
                .ThenInclude(i => i.Predstava)
                .ToListAsync();

            Rezervacije = rezervacije;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Unable to load user");
            }

            await LoadAsync(user);
            return Page();
        }

        public IActionResult OnPostOcijeni(int id)
        {
            var rezervacija = _context.Rezervacija
                    .Include(r => r.IDKorisnika)
                    .FirstOrDefault(r => r.ID == id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            //==postavljanje rezervacije na ocijenjena==

            rezervacija.ocijenjena = true;
            _context.Update(rezervacija);
            _context.SaveChanges();

            var izvedba = _context.Izvedba
                    .Where(i => i.ID == rezervacija.IDIzvedbe)
                    .FirstOrDefault();

            if (izvedba == null)
            {
                return NotFound();
            }

            return RedirectToPage("OcjenaPredstave", new { id = izvedba.IDPredstave });
        }
    }
}
