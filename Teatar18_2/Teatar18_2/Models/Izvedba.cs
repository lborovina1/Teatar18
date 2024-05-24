using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Teatar18_2.Models
{
    public class Izvedba
    {
        public Izvedba () { }
        [Key] public int ID { get; set; }
        [ForeignKey("Predstava")] public int IDPredstave { get; set; }
        public DateTime vrijeme { get; set; }
        public int brojSlobodnihKarata { get; set; }
    }
}
