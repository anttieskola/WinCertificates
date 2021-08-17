using System;
using WinCertificates.Models;

namespace WinCertificates.Finder
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Searches using given thumbprint all certificates found for current user");
                Console.WriteLine("Usage: [thumbprint]");
                return;
            }

            Console.WriteLine("Finding...");

            var results = Finder.Find(args[0]);
            Console.WriteLine("Results...");
            
            if (results.Count == 0)
            {
                Console.WriteLine("no certificate found with given thumbprint");
            }
            else
            {
                Console.WriteLine(CertificateModel.Headers());
                foreach (var cert in results)
                {
                    Console.WriteLine(cert.ToString());
                }
            }
        }
    }
}
