using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System;
using System.Configuration;

namespace SNSConsole
{
    public class SNSOperations
    {
        const string TopicName = "MyAppTopic";
        const string TopicARN = "arn:aws:sns:us-west-1:993110140744:MyAppTopic";
        AmazonSimpleNotificationServiceClient client;
        public BasicAWSCredentials credentials =
             new BasicAWSCredentials(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secertKey"]);
        public SNSOperations()
        {
            client = new AmazonSimpleNotificationServiceClient(credentials, Amazon.RegionEndpoint.USWest1);
        }
        public void CreateTopic()
        {
            CreateTopicRequest request = new CreateTopicRequest
            {
                Name = TopicName
            };
            var response = client.CreateTopic(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Topic Created Successfully");
                Console.WriteLine($"Topic ARN: {response.TopicArn}");
            }
        }
        public void SubscribeToTopic()
        {
            SubscribeRequest request = new SubscribeRequest
            {
                TopicArn = TopicARN,
                Protocol = "email",
                Endpoint = ConfigurationManager.AppSettings["testEmail"]
            };
            var response = client.Subscribe(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Subscribe Created Successfully");
                Console.WriteLine(response.SubscriptionArn);
                Console.WriteLine(request.ReturnSubscriptionArn);
            }
        }

        public void PublishToTopic()
        {
            PublishRequest request = new PublishRequest {
                TopicArn = TopicARN,
                Subject = "New Topic Message",
                Message = "New Message"
            };
            var response = client.Publish(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Message sent successfully");
                Console.WriteLine($"MessageId: {response.MessageId}");
            }
        }

        public void ListTopics()
        {
            ListTopicsRequest request = new ListTopicsRequest
            {

            };
            var response = client.ListTopics(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                foreach (var topic in response.Topics)
                {
                    Console.WriteLine(topic.TopicArn);
                }
            }
        }

        public void ListSubscriptions()
        {
            ListSubscriptionsByTopicRequest request = new ListSubscriptionsByTopicRequest { TopicArn = TopicARN };
            var response = client.ListSubscriptionsByTopic(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                foreach (var subscription in response.Subscriptions)
                {
                    Console.WriteLine(subscription.Owner);
                    Console.WriteLine(subscription.Protocol);
                    Console.WriteLine(subscription.SubscriptionArn);
                    Console.WriteLine(subscription.Endpoint);
                }
            }
        }
        public void Unsubscribe()
        {
            var subscribtion = "Tour sub arn";
            UnsubscribeRequest request = new UnsubscribeRequest { SubscriptionArn = subscribtion };
            var response = client.Unsubscribe(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Unsubscribe successfully");
            }
        }
    }
}
