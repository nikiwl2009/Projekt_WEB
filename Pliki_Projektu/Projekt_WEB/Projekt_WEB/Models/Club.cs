using System.ComponentModel.DataAnnotations;

namespace Projekt_WEB.Models
{
    public class Club
    {
        public int ClubId { get; set; }

        [Required(ErrorMessage = "Podaj nazwę klubu.")]
        [Display(Name = "Nazwa klubu")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Miasto")]
        public string City { get; set; } = string.Empty;

        [Display(Name = "Kraj")]
        public string Country { get; set; } = string.Empty;

        [Display(Name = "Opis")]
        public string Description { get; set; } = string.Empty;

        public List<Athlete> Athletes { get; set; } = new List<Athlete>();
    }
}