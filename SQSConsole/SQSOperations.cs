using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Configuration;
using SharedCode;
namespace SQSConsole
{
    public class SQSOperations
    {
        const string QueueName = "myappqueue";
        const string QueueUrl = "https://sqs.us-east-1.amazonaws.com/993110140744/myappqueue";
        public BasicAWSCredentials credentials =
          new BasicAWSCredentials(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secertKey"]);
        AmazonSQSClient client;
        public SQSOperations()
        {
            client = new AmazonSQSClient(credentials, Amazon.RegionEndpoint.USEast1);
        }
        public void CreateSQSQueue()
        {
            CreateQueueRequest request = new CreateQueueRequest { QueueName = QueueName };
            var response = client.CreateQueue(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("SQS Queue Created Successfully");
                Console.WriteLine($"SQS Queue Url: {response.QueueUrl}");
            }
        }
        public void SendMessage()
        {
            SendMessageRequest request = new SendMessageRequest { QueueUrl = QueueUrl, MessageBody = "My Queue Message" };
            var response = client.SendMessage(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Message Sent Successfully");
                Console.WriteLine($"Message Id: {response.MessageId}, Sequence Id: {response.SequenceNumber}");
            }
        }
        public void ReceiveMessage()
        {
            ReceiveMessageRequest request = new ReceiveMessageRequest { QueueUrl = QueueUrl };
            var response = client.ReceiveMessage(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Message(s) Received Successfully");
                foreach (var message in response.Messages)
                {
                    Console.WriteLine($"Message Content: {message.Body}");
                    Console.WriteLine($"Message Content: {message.MessageId}");
                }
            }
        }
    }
}
