using System.ComponentModel.DataAnnotations;

namespace Teatar18_2.Models
{
    public enum Zanr
    {
        [Display(Name="Drama")]
        Drama, 
        [Display(Name="Komedija")]
        Komedija, 
        [Display(Name="Opera")]
        Opera, 
        [Display(Name="Balet")]
        Balet
    }
}
