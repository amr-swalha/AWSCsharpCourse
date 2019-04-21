using System;
using System.Configuration;
using Amazon.Glacier;
using Amazon.Glacier.Model;
using Amazon.Runtime;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Amazon.Glacier.Transfer;
using SharedCode;

namespace S3Console
{
    public class S3GlacierOperations
    {
        AmazonGlacierClient client;
        public BasicAWSCredentials credentials =
             new BasicAWSCredentials(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secertKey"]);
        const string vaultName = "myapp564";
        public S3GlacierOperations()
        {
            client = new AmazonGlacierClient(credentials, Amazon.RegionEndpoint.USWest1);

        }
        public void CreateVault()
        {
            CreateVaultRequest request = new CreateVaultRequest
            {
                AccountId = "-",
                VaultName = vaultName
            };
            var response = client.CreateVault(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Vault Created Successfully");
            }
        }
        public void UploadVaultArchive()
        {
            var stream = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "\\test.txt");
            UploadArchiveRequest request = new UploadArchiveRequest
            {
                VaultName = vaultName,
                AccountId = "-",
                ArchiveDescription = "test desc",
                Checksum = TreeHashGenerator.CalculateTreeHash(stream),
                Body = stream
            };
            request.StreamTransferProgress += OnUploadProgress;
            var response = client.UploadArchive(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Archive Uploaded successfully");
                Console.WriteLine($"RequestId: {response.ResponseMetadata.RequestId}");
                foreach (var item in response.ResponseMetadata.Metadata)
                {
                    Console.WriteLine($"{item.Key}:{item.Value}");
                }
            }
        }
        public void OnUploadProgress(object sender, StreamTransferProgressArgs args)
        {
            Console.WriteLine($"PercentDone: {args.PercentDone}");
            Console.WriteLine($"Total Tansfer: {args.TransferredBytes}/{args.TotalBytes}");
            Console.WriteLine($"IncrementTransferred: {args.IncrementTransferred}");
        }
        public void DownloadArchive()
        {
            var manager = new ArchiveTransferManager(credentials, Amazon.RegionEndpoint.USWest1);
            manager.Download(vaultName,
                "bIN_WObb6J9b_EkKN-G0nOXn1bf6J0JJM573ZbFg8K5KuuWsZDgmGBylHg_MiIaB3My-d1n5qtW_AsMgZgVg6FOzPjgeKbu1RX2jrkUi2kpH0SJ5xhkgjXz-XgWYS3OVqq5hxN_Ftw",
                AppDomain.CurrentDomain.BaseDirectory + "\\test-galcier.txt");
            Console.WriteLine("File downloaded successfully");
        }

    }
}
