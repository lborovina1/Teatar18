using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Teatar18_2.Models
{
    public class Korisnik: IdentityUser
    {
        public Korisnik(){}
        [Key] public int ID { get; set; }
        public string imePrezime { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public int brojKupljenihKarata { get; set; } = 0;
        public bool newsletter { get; set; } = false;
    }
}
