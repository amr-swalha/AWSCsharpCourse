using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
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
        const string BackupArn = "arn:aws:dynamodb:us-west-1:993110140744:table/MyAppsTable/backup/01556559330663-cc29d80b";
        const string BackupTableName = "MyAppsTableBk";
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
        public void InsertItem()
        {
            PutItemRequest request = new PutItemRequest
            {
                TableName = TableName,
                Item = new Dictionary<string, AttributeValue>
               {
                   {"Id", new AttributeValue{N = "7"} },
                   {"Username", new AttributeValue{S = "user"} },
                   {"CreatedAt", new AttributeValue{S = "27-4-2019"} }
               }
            };
            var response = client.PutItem(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Item added successfully");
            }
        }

        public void GetItem()
        {
            GetItemRequest request = new GetItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"Id", new AttributeValue{N = "7"} },
                    {"Username", new AttributeValue{S = "user"} }
                }
            };
            var response = client.GetItem(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                if (response.Item.Count > 0)
                {
                    Console.WriteLine("Items(s) retrived successfully");
                    foreach (var item in response.Item)
                    {
                        Console.WriteLine($"Key: {item.Key}, Value:{item.Value.S}{item.Value.N}");
                    }
                }
                else
                {
                    Console.WriteLine("No Items(s) found");
                }
            }
        }

        public void DeleteItem()
        {
            DeleteItemRequest request = new DeleteItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"Id", new AttributeValue{N = "7"} },
                    {"Username", new AttributeValue{S = "user"} }
                }
            };
            var response = client.DeleteItem(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Item has been deleted successfully");
                GetItem();
            }
        }

        public void DescribeTable()
        {
            DescribeTableRequest request = new DescribeTableRequest { TableName = TableName };
            var response = client.DescribeTable(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine($"TableArn: {response.Table.TableArn}");
            }
        }

        public void DeleteTable()
        {
            var response = client.DeleteTable(BackupTableName);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Table has been deleted successfully");
                Console.WriteLine($"Table status: {response.TableDescription.TableStatus.Value}");
            }
        }
        public void BackupTable()
        {
            CreateBackupRequest request = new CreateBackupRequest { BackupName = "BKP002", TableName = TableName };
            var response = client.CreateBackup(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Backup created successfully");
                Console.WriteLine($"Backup BackupArn:{response.BackupDetails.BackupArn}");
                Console.WriteLine($"Backup BackupCreationDateTime:{response.BackupDetails.BackupCreationDateTime}");
                Console.WriteLine($"Backup BackupStatus:{response.BackupDetails.BackupStatus}");
                Console.WriteLine($"Backup BackupSizeBytes:{response.BackupDetails.BackupSizeBytes}");
            }
        }
        public void RestoreBackup()
        {
            RestoreTableFromBackupRequest request = new RestoreTableFromBackupRequest { BackupArn = BackupArn, TargetTableName = BackupTableName };
            var response = client.RestoreTableFromBackup(request);
            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Backup restored successfully");
                Console.WriteLine($"Backup Table Arn: {response.TableDescription.TableArn}");
                Console.WriteLine($"Backup Table Status: {response.TableDescription.TableStatus}");
            }
        }
    }
}
