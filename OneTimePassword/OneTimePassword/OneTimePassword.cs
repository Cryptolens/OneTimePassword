using System;

using System.Security.Cryptography;

namespace Cryptolens.OneTimePassword
{
    public class OneTimePassword
    {
        /// <summary>
        /// The period of time used for TOTP.
        /// </summary>
        public readonly static int PERIOD = 30;

        /// <summary>
        /// Creates a time-based one-time password based on rfc6238.
        /// </summary>
        /// <param name="secret">The shared secret created using
        /// <see cref="CreateSharedSecret"/>.</param>
        public static string TimeBasedPassword(byte[] secret)
        {
            // see https://tools.ietf.org/html/rfc6238

            return CounterBasedPassword(secret, DateTimeOffset.UtcNow.ToUnixTimeSeconds() / PERIOD);
        }


        /// <summary>
        ///  Creates a counter-based one-time password based on rfc4226.
        /// </summary>
        /// <param name="secret">The shared secret created using
        /// <see cref="CreateSharedSecret"/>.</param>
        /// <param name="counter">The counter</param>
        public static string CounterBasedPassword(byte[] secret, long counter)
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

            return (bin_code % 0xf4240).ToString().PadLeft(6,'0');
        }

        /// <summary>
        /// Creates a random shared secret.
        /// </summary>
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

        /// <summary>
        /// Converts the random shared secret from <see cref="CreateSharedSecret"/>
        /// to base32.
        /// </summary>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string SharedSecretToString(byte[] secret)
        {
            return Helpers.BytesToBase32(secret);
        }

        /// <summary>
        /// Creates an url which can be used to create a QR-code that users can scan in their Authenticator app.
        /// </summary>
        /// <param name="secret">The shared secret obtained from <see cref="SharedSecretToString(byte[])"/>.</param>
        /// <param name="username">The username that this two-factor code will be associated with. </param>
        /// <param name="companyName">Company name and/or your app name.</param>
        /// <returns></returns>
        public static string GetAuthenticatorAppUrl(string secret, string username, string companyName)
        {
            return $"otpauth://totp/${username}?secret=${secret}&issuer=${companyName}";
        }
    }
}
