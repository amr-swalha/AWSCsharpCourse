using System;
using System.Configuration;
using Amazon.Glacier;
using Amazon.Glacier.Model;
using Amazon.Runtime;

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
    }
}
