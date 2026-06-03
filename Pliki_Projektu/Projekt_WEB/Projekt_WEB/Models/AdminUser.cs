using System.ComponentModel.DataAnnotations;

namespace Projekt_WEB.Models
{
    public class AdminUser
    {
        public int AdminUserId { get; set; }

        [Required(ErrorMessage = "Podaj adres e-mail.")]
        [EmailAddress(ErrorMessage = "Podaj poprawny adres e-mail.")]
        [StringLength(120, ErrorMessage = "Adres e-mail może mieć maksymalnie 120 znaków.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Brakuje hasha hasła.")]
        [StringLength(500, ErrorMessage = "Hash hasła może mieć maksymalnie 500 znaków.")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj rolę użytkownika.")]
        [StringLength(50, ErrorMessage = "Rola może mieć maksymalnie 50 znaków.")]
        [Display(Name = "Rola")]
        public string Role { get; set; } = "Admin";
    }
}