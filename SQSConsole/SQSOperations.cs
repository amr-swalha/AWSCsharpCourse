using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Configuration;
using SharedCode;
using System.Collections.Generic;

namespace SQSConsole
{
    public class SQSOperations
    {
        const string QueueName = "myappqueue";
        const string ReceiptHandle = "AQEBzDmr97WjpftKzvm5GzvcrEZl3fsK8SmjGVhVV1O759HYC4V8Q9PDfU6CbsleCOufBV9JLbPYpW5JphEdrkT0IIO5+3s6uJe7yzwo6YUgXo8dhDa28Gzoe+P3Dg707ex/h5+GP0AG4O1kZmjmiKev1ooRnLr7d5dS0aUq5bq5hzcdMZrU51mhsEX325BOszXB8xNPCOZBkzJ55ABg3eS6NIg9vjv1fTYC1qba0q7Ak1B1CaXGBmBypaBnynnw6lA4B5edntAuhL6PAtjFgt/5iol/p54oOmARQ3lQJNUBUfnM59xpceAdP4Etr60J9ISaUCID/m7V52X9s7Y2LbuYP08dmXraPvxpdyVtHfPpCG8VqvoAGtyxIJ5weR20bUWX";
        const string QueueUrl = "https://sqs.us-east-1.amazonaws.com/993110140744/myappqueue";
        const string DeleteQueue = "https://sqs.us-east-1.amazonaws.com/993110140744/newq";
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
                    Console.WriteLine($"Message Content: {message.ReceiptHandle}");
                }
            }
        }
        public void SendBatchMessages()
        {
            SendMessageBatchRequest request = new SendMessageBatchRequest();
            request.QueueUrl = QueueUrl;
            request.Entries = new List<SendMessageBatchRequestEntry>
            {
                new SendMessageBatchRequestEntry {Id = Guid.NewGuid().ToString(),MessageBody = "First Message"},
                new SendMessageBatchRequestEntry {Id = Guid.NewGuid().ToString(),MessageBody = "Second Message"}
            };
            var response = client.SendMessageBatch(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Messages queued successfully");
                foreach (var item in response.Successful)
                {
                    Console.WriteLine($"ID: {item.Id}, SequenceNumber: {item.SequenceNumber}, MessageId: {item.MessageId}");
                }
                
            }
        }
        public void DeleteMessage()
        {
            DeleteMessageRequest request = new DeleteMessageRequest { QueueUrl = QueueUrl, ReceiptHandle = ReceiptHandle };
            var response = client.DeleteMessage(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Message Deleted Succsessfully");
            }
        }
        public void PurgeMessages()
        {
            PurgeQueueRequest request = new PurgeQueueRequest { QueueUrl = QueueUrl };
            var response = client.PurgeQueue(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Message(s) Deleted Succsessfully");
            }
        }
        public void ListQueues()
        {
            ListQueuesRequest request = new ListQueuesRequest { };
            var respone = client.ListQueues(request);
            if (respone.HttpStatusCode.IsSuccess())
            {
                foreach (var queue in respone.QueueUrls)
                {
                    Console.WriteLine(queue);
                }
            }
        }
        public void DeleteQueues()
        {
            DeleteQueueRequest request = new DeleteQueueRequest { QueueUrl = DeleteQueue };
            var response = client.DeleteQueue(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Queue has been deleted successfully");
                Console.WriteLine("Current Queues");
                ListQueues();
            }
        }

        public void AddTags()
        {
            TagQueueRequest request = new TagQueueRequest();
            request.QueueUrl = QueueUrl;
            request.Tags.Add("env", "staging");
            var response = client.TagQueue(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Tags added successfully");
                ListTags();
            }
        }
        public void ListTags()
        {
            var response =  client.ListQueueTags(new ListQueueTagsRequest { QueueUrl = QueueUrl });
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Tags List");
                foreach (var tag in response.Tags)
                {
                    Console.WriteLine($"Key: {tag.Key}, Value: {tag.Value}");
                }
            }
        }
    }
}
