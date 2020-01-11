using System;
using System.Text;

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

        public static bool VerifyPassword(string password, byte[] userSalt, byte[] userHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256(userSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != userHash[i]) return false;
                }
            }

            return true;
        }
    }
}
