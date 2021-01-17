using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLicense.NET.Helpers
{
    /// <summary>
    /// From this method all the requests of this project are made
    /// </summary>
    internal static class CryptoRequestHelper
    {        
        internal static async Task<HttpResponseMessage> PostAsync(HttpClient httpClient, string EndPoint, Dictionary<string, string> formUrlEncodedData = null)
            => await httpClient.PostAsync($"{Globals.EndPoint}{EndPoint}", formUrlEncodedData != null ? new FormUrlEncodedContent(formUrlEncodedData) : null);
    }
}
