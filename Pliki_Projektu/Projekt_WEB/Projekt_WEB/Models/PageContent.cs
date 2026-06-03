using System.ComponentModel.DataAnnotations;

namespace Projekt_WEB.Models
{
    public class PageContent
    {
        public int PageContentId { get; set; }

        [Required(ErrorMessage = "Podaj klucz treści.")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "Klucz treści powinien mieć od 2 do 80 znaków.")]
        [Display(Name = "Klucz treści")]
        public string Key { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj tytuł.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Tytuł powinien mieć od 2 do 150 znaków.")]
        [Display(Name = "Tytuł")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Treść może mieć maksymalnie 1000 znaków.")]
        [Display(Name = "Treść")]
        public string Body { get; set; } = string.Empty;
    }
}