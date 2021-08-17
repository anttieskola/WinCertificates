using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using WinCertificates.Models;

namespace WinCertificates.Catalog
{
    public class CatalogCreator
    {
        private readonly List<CertificateModel> _certificates;

        public CatalogCreator()
        {
            _certificates = new List<CertificateModel>();
        }

        public List<CertificateModel> Create()
        {
            Iterate();
            Analyze();
            return Order();
        }

        private void Iterate()
        {
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
                        _certificates.Add(new CertificateModel
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

                    store.Close();
                }
            }
        }

        private void Analyze()
        {
            // detect duplicates
            foreach (var certificate in _certificates)
            {
                certificate.IsDuplicate = _certificates.Any(c => certificate.Thumbprint == c.Thumbprint && certificate.Id != c.Id);
            }
        }

        private List<CertificateModel> Order()
        {
            return _certificates
                .OrderBy(c => c.StoreLocation)
                .ThenBy(c => c.StoreName)
                .ThenBy(c => c.SimpleName)
                .ToList();
        }
    }
}
