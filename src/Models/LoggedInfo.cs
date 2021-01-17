using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoLicense.NET.Models
{
    public class LoggedInfo
    {
        public string LicenseKey { get; set; }
        public string SessionID { get; set; }
        public string HWID { get; set; }
        public string errorMessage { get; set; }
        public int Rank { get; set; }
        public Dictionary<string, string> Variables { get; set; }
    }

    public class LicenseInfo
    {
        public string Rank { get; set; }
        public string verifyAccessKey { get; set; }
        public string verifyStamp { get; set; }
    }

    public class ValidatedSession
    {
        public LicenseInfo licenseInfo { get; set; }
    }
}
