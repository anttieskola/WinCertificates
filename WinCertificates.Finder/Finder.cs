using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using WinCertificates.Models;

namespace WinCertificates.Finder
{
    public class Finder
    {
        public static List<CertificateModel> Find(string thumbprint)
        {
            var certificates = new List<CertificateModel>();

            var locationCount = Enum.GetValues(typeof(StoreLocation)).Length;
            var nameCount = Enum.GetValues(typeof(StoreName)).Length;

            var locationIndex = 0;
            foreach (var location in Enum.GetValues(typeof(StoreLocation)))
            {
                locationIndex++;
                var storeLocation = (StoreLocation)location;

                var nameIndex = 0;
                foreach (var name in Enum.GetValues(typeof(StoreName)))
                {
                    nameIndex++;

                    var storeName = (StoreName)name;
                    Console.WriteLine($"Location {locationIndex}/{locationCount} - Name {nameIndex}/{nameCount} ({storeLocation}.{storeName})");

                    var store = new X509Store(storeName, storeLocation);
                    store.Open(OpenFlags.ReadOnly);

                    foreach (var cert in store.Certificates)
                    {
                        if (cert.Thumbprint == thumbprint)
                        {
                            certificates.Add(new CertificateModel
                            {
                                Id = Guid.NewGuid(),
                                StoreLocation = storeLocation,
                                StoreName = storeName,
                                Thumbprint = cert.Thumbprint,
                                SimpleName = cert.GetNameInfo(X509NameType.SimpleName, true),
                                HasPublicKey = !string.IsNullOrEmpty(cert.GetPublicKeyString()),
                                Verified = cert.Verify(),
                            });
                        }
                    }

                    store.Close();
                }
            }

            return certificates;
        }
    }
}
