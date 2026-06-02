using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace system_zawodnicy_zimowi.core.Services
{
    public static class PomocnikHasel
    {
    
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (var sha256 = SHA256.Create())
            {
                
                var bytes = Encoding.UTF8.GetBytes(password);
                
                var hash = sha256.ComputeHash(bytes);
                
                return Convert.ToBase64String(hash);
            }
        }

        
        public static bool VerifyPassword(string inputPassword, string storedHash)
        {
            string inputHash = HashPassword(inputPassword);
        
            return inputHash == storedHash;
        }
    }
}
