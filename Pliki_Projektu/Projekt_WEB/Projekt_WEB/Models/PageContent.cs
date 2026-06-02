using System.ComponentModel.DataAnnotations;

namespace Projekt_WEB.Models
{
    public class PageContent
    {
        public int PageContentId { get; set; }

        [Required]
        [Display(Name = "Klucz treści")]
        public string Key { get; set; } = string.Empty;

        [Display(Name = "Tytuł")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Treść")]
        public string Body { get; set; } = string.Empty;
    }
}