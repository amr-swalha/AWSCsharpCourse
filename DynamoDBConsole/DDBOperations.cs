using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using SharedCode;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace DynamoDBConsole
{
    public class DDBOperations
    {
        AmazonDynamoDBClient client;
        public BasicAWSCredentials credentials =
             new BasicAWSCredentials(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secertKey"]);
        const string TableName = "MyAppsTable";
        public DDBOperations()
        {
            client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USWest1);
        }
        public void CreateTable()
        {
            CreateTableRequest request = new CreateTableRequest
            {
                TableName = TableName,
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Id",
                        AttributeType = "N"
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "Username",
                        AttributeType = "S"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Id",
                        KeyType = "HASH"
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "Username",
                        KeyType = "RANGE"
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 20,
                    WriteCapacityUnits = 10
                }
            };
            var response = client.CreateTable(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Table created successfully");
            }
        }
    }
}
