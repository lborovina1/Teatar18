using System.ComponentModel.DataAnnotations;

namespace Teatar18_2.Models
{
    public class Newsletter
    {
        public Newsletter() { }
        [Key] public int ID { get; set; }
        public string informacija { get; set; }
        public DateTime? datumSlanja { get; set; }
    }
}
