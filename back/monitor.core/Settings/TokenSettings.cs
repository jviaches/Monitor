using System;
using System.Collections.Generic;
using System.Text;

namespace monitor_core.Settings
{
    public class TokenSettings
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int ExpirationInMinutes { get; set; }
        public string SigningKey { get; set; }

        public byte[] GetSecurityKey()
        {
            return Encoding.ASCII.GetBytes(SigningKey);
        }
    }
}
