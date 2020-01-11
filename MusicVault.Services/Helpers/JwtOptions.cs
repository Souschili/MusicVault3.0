using System;
using System.Collections.Generic;
using System.Text;

namespace MusicVault.Services.Helpers
{
    public class JwtOptions
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpireTime { get; set; }
    }
}
