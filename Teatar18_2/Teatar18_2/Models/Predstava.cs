using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace Teatar18_2.Models
{
    public class Predstava
    {
        public Predstava() { }

        [Key] public int ID { get; set; }
        public String Naziv { get; set; }
        public List<String> glumci { get; set; }
        public List<String> scenristi { get; set; }
        public List<String> reziseri { get; set; }
        public List<String> scenografi { get; set; }
        public Zanr zanr { get; set; }
        public String opis { get; set; }
        public Uri poster { get; set; }
        public int trajanje { get; set; }
        public List<double> ocjene { get; set; }
        public List<int> slobodneKarte { get; set; }
        public bool uRepertoaru { get; set; }

        public void ocijeniPredstavu(){ }
        public double izracunajProsjecnuOcjenu(){ return ocjene.Average(); }
    }
}
