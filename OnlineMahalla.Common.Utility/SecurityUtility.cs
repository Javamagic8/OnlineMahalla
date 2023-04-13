using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Utility
{
    public static class HashHelper
    {

        public static string CreateRandomSalt()
        {
            Byte[] saltBytes = new Byte[4];
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);

            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// Compute salted password hash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string ComputeSaltedHash(string password, string salt)
        {
            // Create Byte array of password string
            UnicodeEncoding encoder = new UnicodeEncoding();
            Byte[] secretBytes = encoder.GetBytes(password);

            // Create a new salt
            Byte[] saltBytes = Convert.FromBase64String(salt);

            // append the two arrays
            Byte[] toHash = new Byte[secretBytes.Length + saltBytes.Length];
            Array.Copy(secretBytes, 0, toHash, 0, secretBytes.Length);
            Array.Copy(saltBytes, 0, toHash, secretBytes.Length, saltBytes.Length);

            SHA512 sha512 = SHA512.Create();
            Byte[] computedHash = sha512.ComputeHash(toHash);

            return Convert.ToBase64String(computedHash);
        }

    }
    public class CustomPaswordHasher
    {
        public string HashPassword(string password, string salt)
        {
            return HashHelper.ComputeSaltedHash(password, salt);
        }

        public bool VerifyHashedPassword(string hashedPassword, string providedPassword, string salt)
        {
            if (string.Equals(hashedPassword, HashHelper.ComputeSaltedHash(providedPassword, salt), StringComparison.Ordinal))
                return true;
            else
                return false;
        }
    }
}