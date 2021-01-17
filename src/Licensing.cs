using CryptoLicense.NET.Enums;
using CryptoLicense.NET.Helpers;
using CryptoLicense.NET.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CryptoLicense.NET
{
    public class Licensing
    {
        private static CryptoLicenseConfiguration _CryptoLicenseConfiguration { get; set; }

        /// <summary>
        /// Save your cryptoLicense settings
        /// </summary>
        /// <param name="cryptoLicenseConfiguration">Your cryptoLicense configuration</param>
        public Licensing(CryptoLicenseConfiguration cryptoLicenseConfiguration)
        {
            _CryptoLicenseConfiguration = cryptoLicenseConfiguration;            
        }

        /// <summary>
        /// Initialize the library for use
        /// </summary>
        /// <returns></returns>
        public Licensing Initialize()
        {
            //_CryptoLicenseConfiguration.httpClient.DefaultRequestHeaders.Add("Crypto-Application-ID", _CryptoLicenseConfiguration.ApplicationID);
            // This will be done for a soon update            

            Globals.securityAccessKey = StringHelper.UniqString(25);
            _CryptoLicenseConfiguration.httpClient.DefaultRequestHeaders.Add("Crypto-Access-Key", Globals.securityAccessKey); // Security algorithm

            if (string.IsNullOrEmpty(_CryptoLicenseConfiguration.EncryptionKey) ||
                string.IsNullOrEmpty(_CryptoLicenseConfiguration.CommunicationKey))
                throw new Exception("You must include the communication & encryption key!");

            Cryptography.KEY = _CryptoLicenseConfiguration.EncryptionKey; // Security algorithm
            Cryptography.IV = _CryptoLicenseConfiguration.CommunicationKey; // Security algorithm

            return this;
        }

        /// <summary>
        /// Login using a license key
        /// </summary>
        /// <param name="licenseKey">The client license key</param>
        /// <returns></returns>
        public async Task<(LicenseStatus, LoggedInfo)> validateLicense(string licenseKey)
        {
            string Stamp = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();

            var sessionInfo = new Dictionary<string, string>()
            {
                { "License", licenseKey },
                { "Stamp", Stamp }
            };

            var _loginSession = await CryptoRequestHelper.PostAsync(_CryptoLicenseConfiguration.httpClient, "?session", sessionInfo); // Opening session

            if (_loginSession.IsSuccessStatusCode)
            {
                string responseBody = await _loginSession.Content.ReadAsStringAsync();
                if (responseBody.Contains("session") && 
                    responseBody.Contains("token") &&
                    responseBody.Contains("active"))
                {
                    Session deserialized = Json.Deserialize<Session>(responseBody);
                    string[] Token = deserialized.active.token.Split('?');

                    string accessKeyEncrypted = Cryptography.Decrypt(Token[0]);
                    string stampEncrypted = Cryptography.Decrypt(Token[1]); 
                    string licenseAccessKey = Cryptography.Decrypt(Token[2]); // With this key can access to take the information of the license

                    if (Stamp != stampEncrypted ||
                        Globals.securityAccessKey != accessKeyEncrypted) // Security algorithm
                        return (LicenseStatus.UnexpectedError, new LoggedInfo() { errorMessage = "An unexpected error occurred, if this continues, please contact support." });

                    _CryptoLicenseConfiguration.httpClient.DefaultRequestHeaders.Add("Crypto-License-Access-Key", licenseAccessKey);

                    var _licenseLogin = new Dictionary<string, string>()
                    {
                        { "HWID", Cryptography.createMD5(new UIDHelper().Generate()) }
                    };

                    var licenseLogin = await CryptoRequestHelper.PostAsync(_CryptoLicenseConfiguration.httpClient, $"?validate={deserialized.active.session}", _licenseLogin);
                    // Validating license on the server side

                    switch (licenseLogin.StatusCode)
                    {
                        default: return (LicenseStatus.UnexpectedError, new LoggedInfo() { errorMessage = "An unexpected error occurred, if this continues, please contact support." });
                        case HttpStatusCode.Unauthorized: return (LicenseStatus.InvalidHwid, new LoggedInfo() { errorMessage = "Your HWID must be the same registered to this license, if you think this is a mistake or you want to reset the registered HWID, please contact support." });
                        case HttpStatusCode.Forbidden: return (LicenseStatus.bannedLicense, new LoggedInfo() { errorMessage = "Your license has been permanently banned." });

                        case HttpStatusCode.OK:

                            string licenseBody = await licenseLogin.Content.ReadAsStringAsync();

                            if (licenseBody.Contains("licenseInfo") &&
                                licenseBody.Contains("verifyStamp") &&
                                licenseBody.Contains("verifyAccessKey") &&
                                licenseBody.Contains("Rank"))
                            {
                                ValidatedSession validatedSession = Json.Deserialize<ValidatedSession>(licenseBody);

                                string checksumStamp = Cryptography.Decrypt(validatedSession.licenseInfo.verifyStamp);
                                string checksumLicenseAccessKey = Cryptography.Decrypt(validatedSession.licenseInfo.verifyAccessKey);

                                if (checksumStamp == Stamp &&
                                    checksumLicenseAccessKey == licenseAccessKey) // Security algorithm
                                {
                                    return (LicenseStatus.Valid, new LoggedInfo()
                                    {
                                        HWID = new UIDHelper().Generate(),
                                        LicenseKey = licenseKey,
                                        SessionID = deserialized.active.session,
                                        Rank = int.Parse(Cryptography.Decrypt(validatedSession.licenseInfo.Rank))
                                    });
                                }
                                else
                                {
                                    return (LicenseStatus.UnexpectedError, new LoggedInfo() { errorMessage = "An unexpected error occurred, if this continues, please contact support." });
                                }
                            }
                            else
                            {
                                return (LicenseStatus.UnexpectedError, new LoggedInfo() { errorMessage = "An unexpected error occurred, if this continues, please contact support." });
                            }
                    }
                }
                else
                {
                    return (LicenseStatus.UnexpectedError, new LoggedInfo() { errorMessage = "An unexpected error occurred, if this continues, please contact support." });
                }
            }
            else
            {
                if (_loginSession.StatusCode == HttpStatusCode.NotFound)
                    return (LicenseStatus.NoFound, new LoggedInfo() { errorMessage = "The license entered has not been found." });
                else
                    return (LicenseStatus.UnexpectedError, new LoggedInfo() { errorMessage = "An unexpected error occurred, if this continues, please contact support." });
            }
        }
    }
}
