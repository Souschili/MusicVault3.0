using System;

namespace MusicVault.Services.Helpers
{
    static public class PassCryptHelper
    {
        public static void CreatePassword(string pass, out byte[] passHash, out byte[] passSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256())
            {
                passSalt = hmac.Key;
                passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
            }
        }
    }
}
