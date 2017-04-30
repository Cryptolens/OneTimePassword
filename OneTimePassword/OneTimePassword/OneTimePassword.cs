using System;

using System.Security.Cryptography;

namespace SerialKeyManager.HelperMethods
{
    public class OneTimePassword
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static int TimeBasedPassword(byte[] secret)
        {
            // see https://tools.ietf.org/html/rfc6238

            return CounterBasedPassword(secret, DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 30);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="secret"></param>
        /// <param name="counter"></param>
        /// <returns></returns>
        public static int CounterBasedPassword(byte[] secret, long counter)
        {
            // see https://tools.ietf.org/html/rfc4226

            var hmac = new HMACSHA1(secret);

            var counterBytes = BitConverter.GetBytes(counter);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(counterBytes);

            var hash = hmac.ComputeHash(counterBytes);

            int offset = hash[19] & 0xf;
            int bin_code = (hash[offset] & 0x7f) << 24
               | (hash[offset + 1] & 0xff) << 16
               | (hash[offset + 2] & 0xff) << 8
               | (hash[offset + 3] & 0xff);

            // bincode % 10^6

            return bin_code % 0xf4240;
        }

        public static byte[] CreateSharedSecret()
        {
            // recommend length is 160 bits (120 bits is the minimum)
            // 160 bits = 20 bytes
            // https://tools.ietf.org/html/rfc4226#page-14

            var buffer = new byte[20];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
            }

            return buffer;
        }

        public static string SharedSecretToString(byte[] secret)
        {
            return Helpers.BytesToBase32(secret);
        }
    }
}
