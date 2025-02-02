using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace INNO.Domain.Helpers
{
    public static class PasswordHelper
    {
        private static int iterationCount = 100000;
        private static readonly string key = "ASDfasdfiasjdifjAdsfaSdfsda84898";

        public static bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (hashedPassword == null)
            {
                return false;
            }
            if (providedPassword == null)
            {
                return false;
            }

            byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);

            if (decodedHashedPassword.Length == 0)
            {
                return false;
            }

            return VerifyHashedPassword(decodedHashedPassword, providedPassword);
        }

        private static bool VerifyHashedPassword(byte[] hashedPassword, string providedPassword)
        {
            try
            {
                var prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashedPassword, 1);
                var iterCount = (int)ReadNetworkByteOrder(hashedPassword, 5);
                var saltLength = (int)ReadNetworkByteOrder(hashedPassword, 9);

                if (saltLength < 128 / 8)
                {
                    return false;
                }

                byte[] salt = new byte[saltLength];
                Buffer.BlockCopy(hashedPassword, 13, salt, 0, salt.Length);

                int subkeyLength = hashedPassword.Length - 13 - salt.Length;

                if (subkeyLength < 128 / 8)
                {
                    return false;
                }

                byte[] expectedSubkey = new byte[subkeyLength];

                Buffer.BlockCopy(hashedPassword, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

                byte[] actualSubkey = KeyDerivation.Pbkdf2(providedPassword, salt, prf, iterCount, subkeyLength);

                return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
            }
            catch
            {
                return false;
            }
        }

        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return ((uint)(buffer[offset + 0]) << 24)
                | ((uint)(buffer[offset + 1]) << 16)
                | ((uint)(buffer[offset + 2]) << 8)
                | ((uint)(buffer[offset + 3]));
        }

        public static string GetHash(
            string password,
            KeyDerivationPrf method = KeyDerivationPrf.HMACSHA512
        )
        {
            var salt = GetSalt();
            var saltSize = 128 / 8;

            var result = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: method,
                iterationCount: iterationCount,
                numBytesRequested: 256 / 8
            );

            var outputBytes = new byte[13 + salt.Length + result.Length];
            outputBytes[0] = 0x01;

            WriteNetworkByteOrder(outputBytes, 1, (uint)method);
            WriteNetworkByteOrder(outputBytes, 5, (uint)iterationCount);
            WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);

            Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
            Buffer.BlockCopy(result, 0, outputBytes, 13 + saltSize, result.Length);

            return Convert.ToBase64String(outputBytes);
        }

        private static byte[] GetSalt()
        {
            byte[] salt = new byte[128 / 8];

            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            return salt;
        }

        private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);
        }
    }
}
