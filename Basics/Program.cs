using Amazon.Runtime;
using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basics
{
    class Program
    {
        static void Main(string[] args)
        {
            var credentials =
                new BasicAWSCredentials(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secertKey"]);
            using (AmazonS3Client client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USWest1))
            {
                foreach (var bucket in client.ListBuckets().Buckets)
                {
                    Console.WriteLine(bucket.BucketName + "   " + bucket.CreationDate.ToShortDateString());
                }
            }
            Console.ReadLine();
        }
    }
}
