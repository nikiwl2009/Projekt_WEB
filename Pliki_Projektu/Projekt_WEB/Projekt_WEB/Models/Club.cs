using System.ComponentModel.DataAnnotations;

namespace Projekt_WEB.Models
{
    public class Club
    {
        public int ClubId { get; set; }

        [Required(ErrorMessage = "Podaj nazwę klubu.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Nazwa klubu powinna mieć od 2 do 100 znaków.")]
        [Display(Name = "Nazwa klubu")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj miasto klubu.")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "Miasto powinno mieć od 2 do 80 znaków.")]
        [Display(Name = "Miasto")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj kraj klubu.")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "Kraj powinien mieć od 2 do 80 znaków.")]
        [Display(Name = "Kraj")]
        public string Country { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Opis może mieć maksymalnie 500 znaków.")]
        [Display(Name = "Opis")]
        public string Description { get; set; } = string.Empty;

        public List<Athlete> Athletes { get; set; } = new List<Athlete>();
    }
}