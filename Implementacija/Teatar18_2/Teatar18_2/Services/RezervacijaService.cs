using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Teatar18_2.Data;
using Teatar18_2.Models;

namespace Teatar18_2.Services
{
    public class RezervacijaService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public RezervacijaService(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> PlatiRezervaciju(int id)
        {
            var rezervacija = await _context.Rezervacija
                .Include(r => r.IDKorisnika)
                .FirstOrDefaultAsync(r => r.ID == id);

            if (rezervacija == null)
            {
                return false;
            }

            try
            {
                rezervacija.kupovina = true;
                _context.Update(rezervacija);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            //==postavi karte na placene==

            var rezervisaneKarte = await _context.Karta
                .Where(k => k.IDRezervacije == rezervacija.ID)
                .ToListAsync();

            if (rezervisaneKarte == null)
            {
                return false;
            }

            foreach (var karta in rezervisaneKarte)
            {
                karta.placena = true;
            }

            //==promijeni brojKupljenihKarata korisnika==

            var user = rezervacija.IDKorisnika;
            if (user == null)
            {
                return false;
            }

            user.brojKupljenihKarata += rezervacija.brojKarata;
            _context.Update(user);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> OtkaziRezervaciju(int id)
        {
            var rezervacija = await _context.Rezervacija
                .Include(r => r.IDKorisnika)
                .FirstOrDefaultAsync(r => r.ID == id);

            if (rezervacija == null)
            {
                return false;
            }

            //==postavi karte na slobodne==

            var rezervisaneKarte = await _context.Karta
                .Where(k => k.IDRezervacije == rezervacija.ID)
                .ToListAsync();

            if (rezervisaneKarte == null)
            {
                return false;
            }

            foreach (var karta in rezervisaneKarte)
            {
                karta.IDRezervacije = null;
                if (karta.placena)
                    karta.placena = false;
            }

            //==edit izvedbe (broj slobodnih karata)==

            var izvedba = await _context.Izvedba
                    .Where(i => i.ID == rezervacija.IDIzvedbe)
                    .FirstOrDefaultAsync();

            if (izvedba == null)
            {
                return false;
            }

            izvedba.brojSlobodnihKarata += rezervacija.brojKarata;

            //==edit korisnika==
            if (rezervacija.kupovina)
            {
                var user = rezervacija.IDKorisnika;
                if (user == null)
                {
                    return false;
                }

                user.brojKupljenihKarata -= rezervacija.brojKarata;
                _context.Update(user);
            }

            await _context.SaveChangesAsync();

            _context.Rezervacija.Remove(rezervacija);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> OtkaziNeplaceneRezervacije()
        {
            var rezervacije = await _context.Rezervacija
                .Where(r => r.aktivna == true)
                .Include(r => r.Izvedba)
                .ToListAsync();

            if (rezervacije == null)
                return false;

            foreach(var rezervacija in rezervacije)
            {
                if (rezervacija.kupovina == false && rezervacija.Izvedba.vrijeme <= DateTime.Now.AddDays(2))
                    await OtkaziRezervaciju(rezervacija.ID);
            }
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
