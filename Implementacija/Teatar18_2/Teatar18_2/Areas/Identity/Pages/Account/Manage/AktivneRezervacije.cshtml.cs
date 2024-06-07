#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Teatar18_2.Controllers;
using Teatar18_2.Data;
using Teatar18_2.Models;
using Teatar18_2.Services;

namespace Teatar18_2.Areas.Identity.Pages.Account.Manage
{
    public class AktivneRezervacijeModel: PageModel
    {
        private readonly UserManager<Korisnik> _userManager;
        private readonly SignInManager<Korisnik> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly RezervacijaService _rezervacijaService;

        public AktivneRezervacijeModel(
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
        public List<(Rezervacija, List<Karta>)> KarteRezervacije { get; set; } = new List<(Rezervacija, List<Karta>)>();
        
        private async Task LoadAsync(Korisnik user)
        {
            var rezervacije = await _context.Rezervacija
                .Where(r => r.IDKorisnika == user && r.aktivna == true)
                .Include(r => r.Izvedba)
                .ThenInclude(i => i.Predstava)
                .ToListAsync();

            //==provjeri koje rezervacije su istekle / trebaju se automatski otkazati==

            var rezervacijeZaOtkazivanje = new List<Rezervacija>();
            
            foreach (var rezervacija in rezervacije)
            {
                if (rezervacija.kupovina == false && rezervacija.Izvedba.vrijeme <= DateTime.Now.AddDays(2))
                {
                    rezervacijeZaOtkazivanje.Add(rezervacija);
                }

                else if (rezervacija.Izvedba.vrijeme < DateTime.Now)
                {
                    rezervacija.aktivna = false;
                    _context.Update(rezervacija);
                }  
            }
            await _context.SaveChangesAsync();

            //==filtriranje po aktivnim/validnim==

            rezervacije.RemoveAll(r => r.aktivna == false);     
            
            foreach(var rezervacija in rezervacijeZaOtkazivanje)    
            {
                rezervacije.Remove(rezervacija);
                await _rezervacijaService.OtkaziRezervaciju(rezervacija.ID);
            }

            Rezervacije = rezervacije;

            //==vezi karte uz rezervacije==
            
            foreach(var rezervacija in rezervacije)
            {
                KarteRezervacije.Add((rezervacija, await _context.Karta
                    .Where(k => k.IDRezervacije == rezervacija.ID)
                    .Include(k => k.Rezervacija)
                    .ToListAsync()));
            }
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

        public IActionResult OnPostPlati(int id)
        {
            return RedirectToPage("Kupovina", new { id });
        }

        public IActionResult OnPostOtkazi(int id)
        {
            return RedirectToPage("OtkazivanjeRezervacije", new { id });
        }
    }
}