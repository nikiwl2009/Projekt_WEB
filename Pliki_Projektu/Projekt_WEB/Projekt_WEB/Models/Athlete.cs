using System.ComponentModel.DataAnnotations;

namespace Projekt_WEB.Models
{
    public class Athlete
    {
        public int AthleteId { get; set; }

        [Required(ErrorMessage = "Podaj imię zawodnika.")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Imię powinno mieć od 2 do 60 znaków.")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj nazwisko zawodnika.")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "Nazwisko powinno mieć od 2 do 80 znaków.")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; } = string.Empty;

        [Range(6, 70, ErrorMessage = "Wiek zawodnika powinien być w zakresie od 6 do 70 lat.")]
        [Display(Name = "Wiek")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Podaj kraj zawodnika.")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "Kraj powinien mieć od 2 do 80 znaków.")]
        [Display(Name = "Kraj")]
        public string Country { get; set; } = string.Empty;

        [Range(0, 100000, ErrorMessage = "Liczba punktów nie może być ujemna.")]
        [Display(Name = "Punkty")]
        public int Points { get; set; }

        [Display(Name = "Status")]
        public AthleteStatus Status { get; set; } = AthleteStatus.Active;

        [StringLength(250, ErrorMessage = "Ścieżka zdjęcia może mieć maksymalnie 250 znaków.")]
        [Display(Name = "Zdjęcie")]
        public string? PhotoPath { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Wybierz dyscyplinę.")]
        [Display(Name = "Dyscyplina")]
        public int DisciplineId { get; set; }

        public Discipline? Discipline { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Wybierz klub.")]
        [Display(Name = "Klub")]
        public int ClubId { get; set; }

        public Club? Club { get; set; }

        public List<Result> Results { get; set; } = new List<Result>();
    }

    public enum AthleteStatus
    {
        [Display(Name = "Aktywny")]
        Active,

        [Display(Name = "Kontuzjowany")]
        Injured,

        [Display(Name = "Zakończył karierę")]
        Retired
    }
}