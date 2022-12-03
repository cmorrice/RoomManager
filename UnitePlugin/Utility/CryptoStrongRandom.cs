using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UnitePlugin.Utility
{
    public class CryptoStrongRandom
    {
        private static readonly RNGCryptoServiceProvider RngCsp = new RNGCryptoServiceProvider();
        public int Next(byte maxValue)
        {
            // Create a byte array to hold the random value.
            var randomBytes = new byte[1];
            RngCsp.GetBytes(randomBytes);
            return randomBytes[0] % (maxValue + 1);
        }
    }
}
