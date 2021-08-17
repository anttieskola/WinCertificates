using System;
using System.Security.Cryptography.X509Certificates;

namespace WinCertificates.Models
{
    public class CertificateModel
    {
        public Guid Id { get; set; }
        public StoreLocation StoreLocation { get; set; }
        public StoreName StoreName { get; set; }
        public string Thumbprint { get; set; }
        public string SimpleName { get; set; }
        public bool HasPublicKey { get; set; }
        public bool IsDuplicate { get; set; }
        public bool IsValid { get; set; }
        public bool Verified { get; set; }


        public static string Headers()
        {
            return "StoreLocation,StoreName,Thumbprint,SimpleName,HasPublicKey,IsDuplicate,IsValid,Verified";
        }

        public override string ToString()
        {
            return $"{StoreLocation},{StoreName},{Thumbprint},\"{SimpleName}\",{HasPublicKey},{IsDuplicate},{IsValid},{Verified}";
        }
    }
}
