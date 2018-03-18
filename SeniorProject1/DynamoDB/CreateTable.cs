using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SeniorProject1.DynamoDB
{
    public class CreateTable : ICreateTable
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private static string tableName;

        public CreateTable(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public void CreateDynamoDbTables()
        {
            try
            {
                CreateUserTable();
                CreateEventTable();
                CreateNotificationTable();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void CreateUserTable()
        {
            Console.WriteLine("Creating Table");
            tableName = "User";

            var request = new CreateTableRequest
            {
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "UserID",
                        AttributeType = "N"
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "UserName",
                        AttributeType = "S"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "UserID",
                        KeyType = "HASH" // Partition Key
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "UserName",
                        KeyType = "Range" // Sort Key
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                TableName = tableName
            };

            var response = _dynamoDbClient.CreateTableAsync(request);

            WaitUntilTableReady(tableName);
        }

        private void CreateEventTable()
        {
            Console.WriteLine("Creating Table");
            tableName = "Event";

            var request = new CreateTableRequest
            {
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "EventID",
                        AttributeType = "N"
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "UserID",
                        AttributeType = "N"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "EventID",
                        KeyType = "HASH" // Partition Key
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "UserID",
                        KeyType = "Range" // Sort Key
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                TableName = tableName
            };

            var response = _dynamoDbClient.CreateTableAsync(request);

            WaitUntilTableReady(tableName);
        }

        private void CreateNotificationTable()
        {
            Console.WriteLine("Creating Table");
            tableName = "Notification";

            var request = new CreateTableRequest
            {
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "NotificationID",
                        AttributeType = "N"
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "ReceiverID",
                        AttributeType = "N"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "NotificationID",
                        KeyType = "HASH" // Partition Key
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "ReceiverID",
                        KeyType = "Range" // Sort Key
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                TableName = tableName
            };

            var response = _dynamoDbClient.CreateTableAsync(request);

            WaitUntilTableReady(tableName);
        }

        public void WaitUntilTableReady(string tableName)
        {
            string status = null;

            do
            {
                Thread.Sleep(5000);
                try
                {
                    var res = _dynamoDbClient.DescribeTableAsync(new DescribeTableRequest
                    {
                        TableName = tableName
                    });

                    status = res.Result.Table.TableStatus;
                }
                catch (ResourceNotFoundException)
                {

                }

            } while (status != "ACTIVE");
            {
                Console.WriteLine("Table Created Successfully");
            }
        }
    }
}
