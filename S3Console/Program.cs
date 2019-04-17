using System;

namespace S3Console
{
    class Program
    {
        static void Main(string[] args)
        {
            S3GlacierOperations s3GlacierOperations = new S3GlacierOperations();
            //s3GlacierOperations.UploadVaultArchive();
            s3GlacierOperations.DownloadArchive();
            Console.ReadLine();
        }
    }
}
