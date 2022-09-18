using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace TheStory.Utilities
{
    public class PasswordHasher
    {
        public static string Hash(string rawPassword)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(salt);
            }

            var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: rawPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32));

            return hashedPassword;
        }
    }
}
