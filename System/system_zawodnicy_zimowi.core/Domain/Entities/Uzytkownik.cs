using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using system_zawodnicy_zimowi.core.Domain.Exceptions;
using system_zawodnicy_zimowi.core.Services;

namespace system_zawodnicy_zimowi.core.Domain.Entities
{
    public class Uzytkownik
    {
        protected Uzytkownik() { }
        public Uzytkownik(string login, string haslo)
        {
            Id = Guid.NewGuid();
            SetLogin(login);
            SetHaslo(haslo);
        }

        public Guid Id { get; private set; }

        public string Login { get; private set; } = "";

        public string HasloHash { get; private set; } = "";

        public void SetLogin(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new DomainValidationException("Login nie może być pusty.");

            if (login.Length < 3)
                throw new DomainValidationException("Login musi mieć co najmniej 3 znaki.");

            Login = login.Trim();
        }

        public void SetHaslo(string haslo)
        {
            if (string.IsNullOrWhiteSpace(haslo))
                throw new DomainValidationException("Hasło nie może być puste.");

            if (haslo.Length < 7)
                throw new DomainValidationException("Hasło musi mieć co najmniej 7 znaków.");
            var regex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$");
            if (!regex.IsMatch(haslo))
            {
                throw new DomainValidationException("Hasło musi zawierać: 1 dużą literę, 1 cyfrę i znak specjalny.");
            }
            HasloHash = PomocnikHasel.HashPassword(haslo);
        }
       
        

        public bool SprawdzHaslo(string podaneHaslo)
        {
            return PomocnikHasel.VerifyPassword(podaneHaslo, HasloHash);
        }
    }
}