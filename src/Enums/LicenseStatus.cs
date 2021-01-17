using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoLicense.NET.Enums
{
    public enum LicenseStatus
    {
        NoFound,
        InvalidHwid,
        bannedLicense,
        UnexpectedError,
        Valid
    }
}
