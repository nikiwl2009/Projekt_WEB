using System.ComponentModel.DataAnnotations;

namespace Projekt_WEB.Models
{
    public class Result
    {
        public int ResultId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Wybierz zawodnika.")]
        [Display(Name = "Zawodnik")]
        public int AthleteId { get; set; }

        public Athlete? Athlete { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Wybierz zawody.")]
        [Display(Name = "Zawody")]
        public int CompetitionEventId { get; set; }

        public CompetitionEvent? CompetitionEvent { get; set; }

        [Range(1, 300, ErrorMessage = "Miejsce powinno być w zakresie od 1 do 300.")]
        [Display(Name = "Miejsce")]
        public int Place { get; set; }

        [Range(0, 100000, ErrorMessage = "Punkty nie mogą być ujemne.")]
        [Display(Name = "Punkty")]
        public int Points { get; set; }

        [Required(ErrorMessage = "Podaj wynik lub opis rezultatu.")]
        [StringLength(100, ErrorMessage = "Wynik może mieć maksymalnie 100 znaków.")]
        [Display(Name = "Czas / wynik")]
        public string ScoreText { get; set; } = string.Empty;
    }
}