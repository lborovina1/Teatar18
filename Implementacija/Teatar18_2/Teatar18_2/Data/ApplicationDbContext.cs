using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Teatar18_2.Models;

namespace Teatar18_2.Data
{
    public class ApplicationDbContext : IdentityDbContext<Korisnik>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Newsletter> Newsletter { get; set; }
        public DbSet<Korisnik> Korisnik { get; set; }
        public DbSet<Pitanje> Pitanje { get; set; }
        public DbSet<Rezervacija> Rezervacija { get; set; }
        public DbSet<Predstava> Predstava { get; set; }
        public DbSet<Izvedba> Izvedba { get; set; }
        public DbSet<Ocjena> Ocjena { get; set; }
        public DbSet<Karta> Karta { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Newsletter>().ToTable("Newsletter");
            builder.Entity<Korisnik>(b =>
            {
                b.Property(u => u.ime);
                b.Property(u => u.prezime);
                b.Property(u => u.brojKupljenihKarata);
                b.Property(u => u.newsletter);
            });
            builder.Entity<Pitanje>().ToTable("Pitanje");
            builder.Entity<Rezervacija>().ToTable("Rezervacija");
            builder.Entity<Predstava>().ToTable("Predstava");
            builder.Entity<Izvedba>().ToTable("Izvedba");
            builder.Entity<Ocjena>().ToTable("Ocjena");
            builder.Entity<Karta>().ToTable("Karta");

            base.OnModelCreating(builder);
        }
    }
}
