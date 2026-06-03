using System.ComponentModel.DataAnnotations;

namespace Projekt_WEB.Models
{
    public class Discipline
    {
        public int DisciplineId { get; set; }

        [Required(ErrorMessage = "Podaj nazwę dyscypliny.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Nazwa dyscypliny powinna mieć od 2 do 100 znaków.")]
        [Display(Name = "Nazwa dyscypliny")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Opis może mieć maksymalnie 500 znaków.")]
        [Display(Name = "Opis")]
        public string Description { get; set; } = string.Empty;

        public List<Athlete> Athletes { get; set; } = new List<Athlete>();

        public List<CompetitionEvent> CompetitionEvents { get; set; } = new List<CompetitionEvent>();
    }
}