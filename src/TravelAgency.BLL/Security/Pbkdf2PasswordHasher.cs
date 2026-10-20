using System;
using System.Security.Cryptography;

namespace TravelAgency.BLL.Security
{
    /// <summary>
    /// Хешування паролів PBKDF2-SHA256 з випадковою сіллю.
    /// Формат: PBKDF2-SHA256$ітерації$сіль(base64)$хеш(base64).
    /// </summary>
    public class Pbkdf2PasswordHasher : IPasswordHasher
    {
        private const string Prefix = "PBKDF2-SHA256";
        private const char Separator = '$';
        private const int Iterations = 100000;
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int PartsCount = 4;

        public string Hash(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }
            byte[] hash = Derive(password, salt, Iterations);
            return string.Join(Separator.ToString(), Prefix, Iterations.ToString(),
                Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public bool Verify(string password, string hash)
        {
            string[] parts = (hash ?? string.Empty).Split(Separator);
            int iterations;
            if (parts.Length != PartsCount || parts[0] != Prefix || !int.TryParse(parts[1], out iterations))
            {
                return false;
            }
            byte[] salt = Convert.FromBase64String(parts[2]);
            byte[] expected = Convert.FromBase64String(parts[3]);
            byte[] actual = Derive(password ?? string.Empty, salt, iterations);
            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }

        private static byte[] Derive(string password, byte[] salt, int iterations)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(HashSize);
            }
        }
    }
}
