using System;

using System.Security.Cryptography;

namespace OneTimePassword
{
    public class HOTP
    {
        public static int GetToken(byte[] secret, int counter)
        {
            // see https://tools.ietf.org/html/rfc4226#section-5.3

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
    }
}
