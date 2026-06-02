using System.ComponentModel.DataAnnotations;

namespace Projekt_WEB.Models
{
    public class Discipline
    {
        public int DisciplineId { get; set; }

        [Required(ErrorMessage = "Podaj nazwę dyscypliny.")]
        [Display(Name = "Nazwa dyscypliny")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Opis")]
        public string Description { get; set; } = string.Empty;

        public List<Athlete> Athletes { get; set; } = new List<Athlete>();

        public List<CompetitionEvent> CompetitionEvents { get; set; } = new List<CompetitionEvent>();
    }
}