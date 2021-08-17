using System;
using System.IO;
using System.Threading.Tasks;
using WinCertificates.Models;

namespace WinCertificates.Catalog
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Catalogs all certificates for current user into output file");
                Console.WriteLine("Usage: [outputfilename.csv]");
                return;
            }
            var outputFileName = args[0];

            if (File.Exists(outputFileName))
            {
                Console.WriteLine($"{outputFileName} already exists");
                return;
            }

            var creator = new CatalogCreator();
            var catalog = creator.Create();

            using var sw = File.AppendText(outputFileName);
            await sw.WriteLineAsync(CertificateModel.Headers());
            foreach (var cert in catalog)
            {
                await sw.WriteLineAsync(cert.ToString());
            }
        }
    }
}
