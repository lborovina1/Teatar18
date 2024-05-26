using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Teatar18_2.Models
{
    public class Korisnik: IdentityUser
    {
        public Korisnik(){}
        public string ime { get; set; }
        public string prezime { get; set; }
        public int brojKupljenihKarata { get; set; } = 0;
        public bool newsletter { get; set; } = false;
    }
}
