using Amazon.CloudFront;
using Amazon.CloudFront.Model;
using Amazon.Runtime;
using System.Configuration;

namespace CloudFrontConsole
{
    public class CloudFrontOperations
    {
        public BasicAWSCredentials credentials =
        new BasicAWSCredentials(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secertKey"]);
        IAmazonCloudFront client;
        public CloudFrontOperations()
        {
            client = new AmazonCloudFrontClient(credentials, Amazon.RegionEndpoint.USWest1);
            S3Origin origin = new S3Origin { }
            client.CreateDistribution(new CreateDistributionRequest { DistributionConfig = new DistributionConfig {Origins = new Origins { } } })
        }
    }
}
