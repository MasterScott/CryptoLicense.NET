# CryptoLicense.NET
Powerful, open source licensing system, written asynchronous in .NET Core.

### Features:
- Use of Advanced encryption standard (AES)
- HWID Lock configurable 
- <A>synchronous code written in a library for .NET Core
- Possibility of ban licenses
- Customizable license rank
- Advanced HWID Grip
- WEB-API Based

### Operation:
![Pictogram](https://i.imgur.com/MYyUn49.png)

### Usage:

If you don't have encryption keys, I made a small program that you only have to open and it will always give you different KEY / IV keys, you have to grab them and place them on the client side and in the server's PHP file.

```csharp
using System;
using System.Threading.Tasks;
using CryptoLicense.NET;
using CryptoLicense.NET.Enums;
using CryptoLicense.NET.Models;

namespace CryptoLicense.Test
{
    class Program
    {
        static async Task Main()
        {
            var CryptoConfiguration = new CryptoLicenseConfiguration()
            {
                EncryptionKey = "Your encryption key AES-256",
                CommunicationKey = "Your IV Key"
            };

            var cryptoInit = new Licensing(CryptoConfiguration).Initialize();

            (LicenseStatus licenseStatus, LoggedInfo loggedInfo) = await cryptoInit.validateLicense("test");

            if (licenseStatus == LicenseStatus.Valid)
            {
                Console.WriteLine("Logged!");
                Console.WriteLine(loggedInfo.SessionID);
                Console.WriteLine(loggedInfo.Rank);
                Console.WriteLine(loggedInfo.LicenseKey);
                Console.WriteLine(loggedInfo.HWID);
            }
            else
            {
                Console.WriteLine(loggedInfo.errorMessage);
            }      

            Console.ReadLine();
        }
    }
}
```

If you want to generate licenses, you must do it manually or create an API, I am lazy to do it, probably in some update I will integrate it

### Read:

- This license system is open source & maintained by only me in my spare time, if you get code from here or copy it for a license system of yours, please I expect to receive my credit for developing it, any bug, error you find, suggestion or similar, report it and I will help you.
