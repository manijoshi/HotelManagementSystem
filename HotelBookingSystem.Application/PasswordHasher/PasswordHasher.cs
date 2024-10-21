
using System.Security.Cryptography;
using System.Text;

namespace HotelBookingSystem.Application.PasswordHasher
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password, byte[] salt)
        {
            using (var hmac = new HMACSHA256(salt))
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashedPasswordBytes = hmac.ComputeHash(passwordBytes);
                var saltedHashedPassword = new byte[salt.Length + hashedPasswordBytes.Length];

                Buffer.BlockCopy(salt, 0, saltedHashedPassword, 0, salt.Length);
                Buffer.BlockCopy(hashedPasswordBytes, 0, saltedHashedPassword, salt.Length, hashedPasswordBytes.Length);

                return Convert.ToBase64String(saltedHashedPassword);
            }
        }

        public bool VerifyPassword(string password, string storedHashedPassword)
        {
            var saltedHashedPasswordBytes = Convert.FromBase64String(storedHashedPassword);
            var salt = new byte[32];
            var storedHash = new byte[saltedHashedPasswordBytes.Length - 32];

            Buffer.BlockCopy(saltedHashedPasswordBytes, 0, salt, 0, 32);
            Buffer.BlockCopy(saltedHashedPasswordBytes, 32, storedHash, 0, storedHash.Length);

            var computedHashBytes = HashPasswordBytes(password, salt);

            return CompareByteArrays(computedHashBytes, storedHash);
        }

        private byte[] HashPasswordBytes(string password, byte[] salt)
        {
            using (var hmac = new HMACSHA256(salt))
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                return hmac.ComputeHash(passwordBytes);
            }
        }

        public byte[] GenerateSalt(int length = 32)
        {
            var salt = new byte[length];
            RandomNumberGenerator.Fill(salt);
            return salt;
        }

        private bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                    return false;
            }
            return true;
        }
    }
}
