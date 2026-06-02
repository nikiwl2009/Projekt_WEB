using System.ComponentModel.DataAnnotations;

namespace Projekt_WEB.Models
{
    public class AdminUser
    {
        public int AdminUserId { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Display(Name = "Rola")]
        public string Role { get; set; } = "Admin";
    }
}