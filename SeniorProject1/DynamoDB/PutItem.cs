using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject1.DynamoDB
{
    public class PutItem : IPutItem
    {
        private static readonly string tableName = "TempDynamoDbTable";
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public PutItem(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task AddNewEntry(int id, string replyDateTime, double price)
        {
            var queryRequest = RequestBuilder(id, replyDateTime, price);

            await PutItemAsync(queryRequest);
        }

        private async Task PutItemAsync(PutItemRequest request)
        {
            await _dynamoDbClient.PutItemAsync(request);
        }

        private PutItemRequest RequestBuilder(int id, string replyDateTime, double price)
        {
            var item = new Dictionary<string, AttributeValue>
            {
                {"Id", new AttributeValue{N = id.ToString()} },
                {"ReplyDateTime", new AttributeValue {N = replyDateTime} },
                {"Price", new AttributeValue{N = price.ToString(CultureInfo.InvariantCulture)} }
            };

            return new PutItemRequest
            {
                TableName = tableName,
                Item = item
            };
        }
    }
}
