using System.ComponentModel.DataAnnotations;

namespace Projekt_WEB.Models
{
    public class Result
    {
        public int ResultId { get; set; }

        [Display(Name = "Zawodnik")]
        public int AthleteId { get; set; }

        public Athlete? Athlete { get; set; }

        [Display(Name = "Zawody")]
        public int CompetitionEventId { get; set; }

        public CompetitionEvent? CompetitionEvent { get; set; }

        [Range(1, 300, ErrorMessage = "Miejsce powinno być większe od 0.")]
        [Display(Name = "Miejsce")]
        public int Place { get; set; }

        [Range(0, 10000, ErrorMessage = "Punkty nie mogą być ujemne.")]
        [Display(Name = "Punkty")]
        public int Points { get; set; }

        [Display(Name = "Czas / wynik")]
        public string ScoreText { get; set; } = string.Empty;
    }
}