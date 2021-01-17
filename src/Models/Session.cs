using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoLicense.NET.Models
{
    public class Active
    {
        public string session { get; set; }
        public string token { get; set; }
    }

    public class Session
    {
        public Active active { get; set; }
    }
}
