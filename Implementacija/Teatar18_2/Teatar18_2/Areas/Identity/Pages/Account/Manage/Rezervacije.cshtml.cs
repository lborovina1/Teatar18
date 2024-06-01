#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Teatar18_2.Data;
using Teatar18_2.Models;

namespace Teatar18_2.Areas.Identity.Pages.Account.Manage
{
    public class RezervacijeModel: PageModel
    {
        private readonly UserManager<Korisnik> _userManager;
        private readonly SignInManager<Korisnik> _signInManager;
        private readonly ApplicationDbContext _context;

        public RezervacijeModel(
            UserManager<Korisnik> userManager,
            SignInManager<Korisnik> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }
        public List<Rezervacija> Rezervacije { get; set; }

        private async Task LoadAsync(Korisnik user)
        {
            var rezervacije = await _context.Rezervacija
                .Where(r => r.IDKorisnika == user)
                .ToListAsync();

            Rezervacije = rezervacije;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }
    }
}