using System.ComponentModel.DataAnnotations;

namespace Projekt_WEB.Models
{
    public class CompetitionEvent
    {
        public int CompetitionEventId { get; set; }

        [Required(ErrorMessage = "Podaj nazwę zawodów.")]
        [StringLength(120, MinimumLength = 2, ErrorMessage = "Nazwa zawodów powinna mieć od 2 do 120 znaków.")]
        [Display(Name = "Nazwa zawodów")]
        public string Name { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Data zawodów")]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Podaj nazwę miejsca zawodów.")]
        [StringLength(120, MinimumLength = 2, ErrorMessage = "Miejsce powinno mieć od 2 do 120 znaków.")]
        [Display(Name = "Miejsce")]
        public string VenueName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj miasto zawodów.")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "Miasto powinno mieć od 2 do 80 znaków.")]
        [Display(Name = "Miasto")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj kraj zawodów.")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "Kraj powinien mieć od 2 do 80 znaków.")]
        [Display(Name = "Kraj")]
        public string Country { get; set; } = string.Empty;

        [Range(-90, 90, ErrorMessage = "Szerokość geograficzna musi być w zakresie od -90 do 90.")]
        [Display(Name = "Szerokość geograficzna")]
        public double? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Długość geograficzna musi być w zakresie od -180 do 180.")]
        [Display(Name = "Długość geograficzna")]
        public double? Longitude { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Wybierz dyscyplinę.")]
        [Display(Name = "Dyscyplina")]
        public int DisciplineId { get; set; }

        public Discipline? Discipline { get; set; }

        public List<Result> Results { get; set; } = new List<Result>();
    }
}