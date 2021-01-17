using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CryptoLicense.NET.Helpers
{
    internal static class StringHelper
    {
        internal static string UniqString(int string_length)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[(((string_length * 6) + 7) / 8)];
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
