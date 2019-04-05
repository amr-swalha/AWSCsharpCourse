using System;

namespace S3Console
{
    class Program
    {
        static void Main(string[] args)
        {
           S3BucketOperations s3BucketOperations = new S3BucketOperations();
            s3BucketOperations.GeneratePreSignedUrl();
            Console.ReadLine();
        }
    }
}
