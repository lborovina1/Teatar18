#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.CompilerServices;
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
    public class KupovinaModel : PageModel
    {
        private readonly UserManager<Korisnik> _userManager;
        private readonly SignInManager<Korisnik> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly RezervacijaService _rezervacijaService;

        public KupovinaModel(
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

        public class InputModel
        {
            [Required]
            [RegularExpression(@"^\d{16}$", ErrorMessage = "Broj kartice nije validan")]
            [Display(Name = "Broj kartice")]
            public string CreditCardNo { get; set; }

            [Required]
            [RegularExpression(@"^\d{3}$", ErrorMessage = "CVC nije validan")]
            [Display(Name = "CVC")]
            public string CVC { get; set; }

            [Required]
            [RegularExpression(@"^(0[1-9]|1[0-2])\/(\d{2})$", ErrorMessage = "Datum isteka nije validan")]
            [ValidateExpirationDate]            
            [Display(Name = "Datum isteka")]
            public string ExpirationDate { get; set; }

            [Required]
            [Display(Name = "Vlasnik kartice")]
            [RegularExpression(@"[a-zA-Z\s]*", ErrorMessage = "Unos nije validan")]
            [StringLength(50)]
            public string CardholderName { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            rezervacija = await _context.Rezervacija
                .Include(r => r.IDKorisnika)
                .FirstOrDefaultAsync(r => r.ID == id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            Input = new InputModel();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(await _rezervacijaService.PlatiRezervaciju(rezervacija.ID) == false)
            {
                return Page();
            }

            return RedirectToPage("AktivneRezervacije");
        }

        public class ValidateExpirationDate : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (DateTime.TryParseExact(value.ToString(), "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                {
                    return (dt > DateTime.Now)
                        ? ValidationResult.Success
                        : new ValidationResult("Datum isteka nije validan");
                }
                else
                {
                    return new ValidationResult("Invalid date format");
                }
            }
        }
    }
}
