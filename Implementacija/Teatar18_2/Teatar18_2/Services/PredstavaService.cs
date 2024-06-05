using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Teatar18_2.Data;
using Teatar18_2.Models;

namespace Teatar18_2.Services
{
    public class PredstavaService
    {
        private readonly ApplicationDbContext _context;

        public PredstavaService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<double> izracunajProsjecnuOcjenu(int predstavaID)
        {
            var ocjene = await _context.Ocjena
                                       .Where(e => e.IDPredstave == predstavaID)
                                       .Select(e => e.ocjena)
                                       .ToListAsync();

            if (ocjene.Count == 0)
            {
                return 0;
            }

            double prosjecnaOcjena = ocjene.Average();
            return prosjecnaOcjena;
        }

        public async Task<List<Predstava>> DajPreporuke()
        {
            var predstave = await _context.Predstava.ToListAsync();
            var predstaveOcjene = new List<(Predstava predstava, double prosjecnaOcjena)>();

            foreach (var predstava in predstave)
            {
                double prosjecnaOcjena = await izracunajProsjecnuOcjenu(predstava.ID);
                predstaveOcjene.Add((predstava, prosjecnaOcjena));
            }

            var najbolje = predstaveOcjene
                .OrderByDescending(e => e.prosjecnaOcjena)
                .Take(3)
                .Select(e => e.predstava)
                .ToList();

            return najbolje;
        }
    }
}
