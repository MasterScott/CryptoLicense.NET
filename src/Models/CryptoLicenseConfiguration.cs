using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CryptoLicense.NET.Models
{
    public class CryptoLicenseConfiguration
    {
        /// <summary>
        /// Your own configured httpClient (to use proxies or similar) (optional)
        /// </summary>
        public HttpClient httpClient { get; set; } = new HttpClient();

        /// <summary>
        /// Your private encryption key (required)
        /// </summary>
        public string EncryptionKey { get; set; }

        /// <summary>
        /// Your communication key (required)
        /// </summary>
        public string CommunicationKey { get; set; }
    }
}
