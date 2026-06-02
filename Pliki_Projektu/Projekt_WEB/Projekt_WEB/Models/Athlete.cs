using System.ComponentModel.DataAnnotations;

namespace Projekt_WEB.Models
{
    public class Athlete
    {
        public int AthleteId { get; set; }

        [Required(ErrorMessage = "Podaj imię zawodnika.")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj nazwisko zawodnika.")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; } = string.Empty;

        [Range(6, 70, ErrorMessage = "Wiek zawodnika powinien być w zakresie od 6 do 70 lat.")]
        [Display(Name = "Wiek")]
        public int Age { get; set; }

        [Display(Name = "Kraj")]
        public string Country { get; set; } = string.Empty;

        [Display(Name = "Klub")]
        public string Club { get; set; } = string.Empty;

        [Display(Name = "Punkty")]
        public int Points { get; set; }

        [Display(Name = "Status")]
        public AthleteStatus Status { get; set; } = AthleteStatus.Active;

        [Display(Name = "Zdjęcie")]
        public string? PhotoPath { get; set; }

        [Display(Name = "Dyscyplina")]
        public int DisciplineId { get; set; }

        public Discipline? Discipline { get; set; }

        public List<Result> Results { get; set; } = new List<Result>();
    }

    public enum AthleteStatus
    {
        Active,
        Injured,
        Retired
    }
}