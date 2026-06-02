using System.ComponentModel.DataAnnotations;

namespace Projekt_WEB.Models
{
    public class CompetitionEvent
    {
        public int CompetitionEventId { get; set; }

        [Required(ErrorMessage = "Podaj nazwę zawodów.")]
        [Display(Name = "Nazwa zawodów")]
        public string Name { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Data zawodów")]
        public DateTime EventDate { get; set; }

        [Display(Name = "Miejsce")]
        public string VenueName { get; set; } = string.Empty;

        [Display(Name = "Miasto")]
        public string City { get; set; } = string.Empty;

        [Display(Name = "Kraj")]
        public string Country { get; set; } = string.Empty;

        [Display(Name = "Szerokość geograficzna")]
        public double? Latitude { get; set; }

        [Display(Name = "Długość geograficzna")]
        public double? Longitude { get; set; }

        [Display(Name = "Dyscyplina")]
        public int DisciplineId { get; set; }

        public Discipline? Discipline { get; set; }

        public List<Result> Results { get; set; } = new List<Result>();
    }
}