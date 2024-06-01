using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace Teatar18_2.Models
{
    public class Predstava
    {
        public Predstava() {}
        [Key] public int ID { get; set; }
        public string naziv { get; set; }
        public string glumci { get; set; }
        public string? scenaristi { get; set; }
        public string? reziseri { get; set; }
        public string? scenografi { get; set; }
        [EnumDataType(typeof(Zanr))] public Zanr zanr { get; set; }
        public string opis { get; set; }
        public string poster { get; set; }
        public int trajanje { get; set; }
        public bool uRepertoaru { get; set; } = false;
        public bool preporucena { get; set; } = false;
    }
}
