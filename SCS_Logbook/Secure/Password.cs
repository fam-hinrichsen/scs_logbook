using System;
using System.Text;

namespace SCS_Logbook.Secure
{
    public class Password
    {

        public string Salt { get; private set; }

        public string PasswordHash { get; private set;  }

        public Password(string salt, string passwordHash)
        {
            Salt = salt;
            PasswordHash = passwordHash;
        }

        public static Password HashPassword(string password)
        {
            //generate random salt;
            byte[] saltBytes = new byte[32];
            System.Security.Cryptography.RandomNumberGenerator.Create().GetBytes(saltBytes);

            byte[] hash = generateHash(password, saltBytes);

            return new Password(getHexString(saltBytes), getHexString(hash));
        }

        public bool VerifyPassword(string password)
        {
            byte[] saltBytes = StringToByteArray(Salt);
            byte[] hash = generateHash(password, saltBytes);
            return PasswordHash.Equals(getHexString(hash));
        }

        private static byte[] generateHash(string input, byte[] salt)
        {                        
            // Convert the plain string pwd into bytes
            byte[] plainTextBytes = Encoding.Unicode.GetBytes(input);

            // Append salt to pwd before hashing
            byte[] combinedBytes = new byte[plainTextBytes.Length + salt.Length];
            Buffer.BlockCopy(plainTextBytes, 0, combinedBytes, 0, plainTextBytes.Length);
            Buffer.BlockCopy(salt, 0, combinedBytes, plainTextBytes.Length, salt.Length);

            // Create hash for the pwd+salt
            System.Security.Cryptography.HashAlgorithm hashAlgo = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = hashAlgo.ComputeHash(combinedBytes);
            return hash;
        }

        private static string getHexString(byte[] hash) { 
            
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i<hash.Length; i++)
            {
                sBuilder.Append(hash[i].ToString("X2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        private static byte[] StringToByteArray(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] bytes = new byte[hex.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }

            return bytes;
        }
    }
}
