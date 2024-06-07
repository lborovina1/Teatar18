using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Teatar18_2.Data;
using Teatar18_2.Models;

namespace Teatar18_2.Services
{
    public class OcjenaPredstaveService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public OcjenaPredstaveService(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> OcijeniPredstavu(Ocjena ocjena)
        {
            try
            {
                _context.Add(ocjena);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }
    }
}
