using System.ComponentModel.DataAnnotations;

namespace Projekt_WEB.ViewModels
{
    public class SkiChallengeWinnerViewModel
    {
        [Required(ErrorMessage = "Podaj imię.")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Imię powinno mieć od 2 do 60 znaków.")]
        [RegularExpression(@"^[A-Za-zĄĆĘŁŃÓŚŹŻąćęłńóśźż\s\-]+$", ErrorMessage = "Imię może zawierać tylko litery, spacje i myślnik.")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj nazwisko.")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "Nazwisko powinno mieć od 2 do 80 znaków.")]
        [RegularExpression(@"^[A-Za-zĄĆĘŁŃÓŚŹŻąćęłńóśźż\s\-]+$", ErrorMessage = "Nazwisko może zawierać tylko litery, spacje i myślnik.")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; } = string.Empty;

        [Range(0, 20000, ErrorMessage = "Wynik gry ma niepoprawną wartość.")]
        public int DistanceMeters { get; set; }

        public int TargetDistanceMeters { get; set; } = 16000;
    }
}