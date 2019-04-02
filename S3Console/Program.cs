using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using System;
using System.Configuration;

namespace S3Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var credentials =
               new BasicAWSCredentials(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secertKey"]);
            using (AmazonS3Client client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USWest1))
            {
                if (AmazonS3Util.DoesS3BucketExist(client, "myapp564"))
                {
                    Console.WriteLine("Bucket already exists");
                }
                else
                {
                    var bucket = new PutBucketRequest { BucketName = "myapp564", UseClientRegion = true };
                    var bucketResponsoe = client.PutBucket(bucket);
                    if (bucketResponsoe.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Console.WriteLine("Bucket Created Successfully");
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
