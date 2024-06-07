#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Teatar18_2.Data;
using Teatar18_2.Models;
using Teatar18_2.Services;

namespace Teatar18_2.Areas.Identity.Pages.Account.Manage
{
    public class OcjenaPredstaveModel : PageModel
    {
        private readonly UserManager<Korisnik> _userManager;
        private readonly SignInManager<Korisnik> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly OcjenaPredstaveService _ocjenaPredstaveService;

        public OcjenaPredstaveModel(
            UserManager<Korisnik> userManager,
            SignInManager<Korisnik> signInManager,
            ApplicationDbContext context,
            OcjenaPredstaveService ocjenaPredstaveService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _ocjenaPredstaveService = ocjenaPredstaveService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public Ocjena ocjenaPredstave { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ocjenaPredstave = new Ocjena();
            ocjenaPredstave.IDPredstave = id;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (await _ocjenaPredstaveService.OcijeniPredstavu(ocjenaPredstave) == false)
            {
                return Page();
            }

            return RedirectToPage("IstekleRezervacije");
        }
    }
}
