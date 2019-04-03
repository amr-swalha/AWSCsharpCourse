using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using System;
using System.Configuration;
using System.Linq;

namespace S3Console
{
    public class S3BucketOperations : IDisposable
    {
        AmazonS3Client client;
        const string bucketName = "myapp564";
        public BasicAWSCredentials credentials =
              new BasicAWSCredentials(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secertKey"]);
        public S3BucketOperations()
        {
            client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USWest1);
        }

        public void CreateBucket()
        {

            if (AmazonS3Util.DoesS3BucketExist(client, bucketName))
            {
                Console.WriteLine("Bucket already exists");
            }
            else
            {
                var bucket = new PutBucketRequest { BucketName = bucketName, UseClientRegion = true };
                var bucketResponsoe = client.PutBucket(bucket);
                if (bucketResponsoe.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("Bucket Created Successfully");
                }
            }
        }
        public void UploadFile()
        {
            var transfareUtil = new TransferUtility(client);
            //transfareUtil.UploadDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\files",bucketName);
            //transfareUtil.Upload(AppDomain.CurrentDomain.BaseDirectory + "\\test.txt",bucketName);
            var fileTransferRequest = new TransferUtilityUploadRequest
            {
                FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\test.txt",
                CannedACL = S3CannedACL.PublicRead,
                 BucketName = bucketName
            };
            transfareUtil.Upload(fileTransferRequest);
            Console.WriteLine("File Uploaded Successfully");
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
