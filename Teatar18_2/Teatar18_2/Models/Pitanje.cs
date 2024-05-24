using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teatar18_2.Models
{
    public class Pitanje
    {
        public Pitanje(){}
        [Key] public int ID { get; set; }
        [ForeignKey(nameof(Korisnik))] public int IDKorisnika { get; set; }
        public string predmet { get; set; }
        public string sadrzaj { get; set; }
        public DateTime datumPostavljanja { get; set; }
        public bool odgovoreno { get; set; }
        public DateTime? datumOdgovora { get; set; }
        [ForeignKey(nameof(Zaposlenik))] public int? IDZaposlenika { get; set; }
        public Korisnik? Korisnik { get; set; }
        public Korisnik? Zaposlenik { get; set; }
    }
}
