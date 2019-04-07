using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace S3Console
{
    public class S3BucketOperations : IDisposable
    {
        #region Fields
        AmazonS3Client client;
        const string bucketName = "myapp564";
        const string objectName = "test.txt";
        public BasicAWSCredentials credentials =
              new BasicAWSCredentials(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secertKey"]);
        TransferUtility transferUtil;
        #endregion

        public S3BucketOperations()
        {
            client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USWest1);
            transferUtil = new TransferUtility(client);
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
                if (bucketResponsoe.HttpStatusCode.IsSuccess())
                {
                    Console.WriteLine("Bucket Created Successfully");
                }
            }
        }
        public void UploadFile()
        {
            //transfareUtil.UploadDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\files",bucketName);
            //transfareUtil.Upload(AppDomain.CurrentDomain.BaseDirectory + "\\test.txt",bucketName);
            var fileTransferRequest = new TransferUtilityUploadRequest
            {
                FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\test.txt",
                CannedACL = S3CannedACL.PublicRead,
                BucketName = bucketName
            };
            transferUtil.Upload(fileTransferRequest);
            Console.WriteLine("File Uploaded Successfully");
        }
        public async Task DownloadFileAsync()
        {
            string content = string.Empty;
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = "test.txt"
            };
            using (GetObjectResponse response = await client.GetObjectAsync(request))
            {
                using (Stream responseStream = response.ResponseStream)
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string contenType = response.Headers["Content-Type"];
                        content = reader.ReadToEnd();
                        Console.WriteLine("File Content:");
                        Console.WriteLine(content);
                        Console.WriteLine("File Content Type: " + contenType);
                    }
                }
            }
            Console.WriteLine("File Download Successfully");
        }
        public void GeneratePreSignedUrl()
        {
            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = "test.txt",
                Expires = DateTime.Now.AddHours(1)
            };
            var url = client.GetPreSignedURL(request);
            Console.WriteLine("Sharing URL");
            Console.WriteLine(url);
        }
        public void GetObjectTagging()
        {
            GetObjectTaggingRequest tagRequest = new GetObjectTaggingRequest
            {

                BucketName = bucketName,
                Key = objectName
            };
            GetObjectTaggingResponse objectTags = client.GetObjectTagging(tagRequest);
            if (objectTags.Tagging.Count == 0)
            {
                Console.WriteLine("No Tags Found");
            }
            foreach (var tag in objectTags.Tagging)
            {
                Console.WriteLine($"Key: {tag.Key}, Value: {tag.Value}");
            }
        }
        public void UpdateObjectTagging()
        {
            GetObjectTagging();
            Tagging tags = new Tagging();
            tags.TagSet = new List<Tag>
            {
                new Tag{Key="Key1",Value="Val1"},
                new Tag{Key="Key2",Value="Val2"}
            };
            PutObjectTaggingRequest request = new PutObjectTaggingRequest
            {
                BucketName = bucketName,
                Key = objectName,
                Tagging = tags
            };
            PutObjectTaggingResponse response = client.PutObjectTagging(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Object Tags updated successfully");
            }
            GetObjectTagging();
        }
        public void UpdateObjectACL()
        {
            PutACLRequest request = new PutACLRequest { BucketName = bucketName, Key = objectName, CannedACL = S3CannedACL.PublicReadWrite };
            var response = client.PutACL(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Object ACL Updated Successfully");
            }

        }

        public void BucketVersioning()
        {
            PutBucketVersioningRequest request = new PutBucketVersioningRequest
            {
                BucketName = bucketName,
                VersioningConfig = new S3BucketVersioningConfig { EnableMfaDelete = false, Status = VersionStatus.Enabled }
            };
            var response = client.PutBucketVersioning(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Bucket Versioning successful");
            }
        }
        public void Dispose()
        {
            Console.WriteLine("Dispose");
            client.Dispose();
        }
    }
}
