using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace TheStory.Utilities
{
    public static class PasswordHasher
    {
        public static string GetSalt()
        {
            byte[] salt = new byte[16];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        public static string Hash(string salt,string rawPassword)
        {
            var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: rawPassword,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32));

            return hashedPassword;
        }

        public static bool CheckHash(string inputPassword,string salt, string hashedPassword)
        {
            return Hash(salt, inputPassword) == hashedPassword;
        }
    }
}
