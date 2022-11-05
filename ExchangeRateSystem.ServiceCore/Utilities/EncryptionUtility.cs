using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.ServiceCore.Utilities
{
    public static class EncryptionUtility
    {
        public static string Encrypt(string plain)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var hash = BCrypt.Net.BCrypt.HashPassword(plain, salt);

            return hash;
        }

        public static bool Verify(string plain, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(plain, hash);
        }
    }
}
