using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptolens.OneTimePassword
{
    sealed class Helpers
    {
        /// <summary>
        /// Converts a byte array into base 32 (see http://stackoverflow.com/a/42231034/1275924)
        /// Author: www.jensolsson.se
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToBase32(byte[] bytes)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            string output = "";
            for (int bitIndex = 0; bitIndex < bytes.Length * 8; bitIndex += 5)
            {
                int dualbyte = bytes[bitIndex / 8] << 8;
                if (bitIndex / 8 + 1 < bytes.Length)
                    dualbyte |= bytes[bitIndex / 8 + 1];
                dualbyte = 0x1f & (dualbyte >> (16 - bitIndex % 8 - 5));
                output += alphabet[dualbyte];
            }

            return output;
        }
    }
}
