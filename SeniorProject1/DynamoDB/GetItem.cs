using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SeniorProject1.Models;

namespace SeniorProject1.DynamoDB
{
    public class GetItem : IGetItem
    {
        private static readonly string tableName = "User";
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public GetItem (IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task<DynamoTableItems> GetItems(int? id)
        {
            var queryRequest = RequestBuilder(id);

            var result = await ScanAsync(queryRequest);

            return new DynamoTableItems
            {
                User = result.Items.Select(Map).ToList()
            };
        }

        private User Map(Dictionary<string, AttributeValue> result)
        {
            return new User
            {
                UserID = Convert.ToInt32(result["UserID"].N),
                UserName = result["UserName"].S,
                Email = result["Email"].S
            };
        }

        private async Task<ScanResponse> ScanAsync(ScanRequest request)
        {
            var response = await _dynamoDbClient.ScanAsync(request);

            return response;
        }

        private ScanRequest RequestBuilder(int? id)
        {
            if (id.HasValue == false)
            {
                return new ScanRequest
                {
                    TableName = tableName

                };
            }

            return new ScanRequest
            {
                TableName = tableName,
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":v_Id", new AttributeValue{N = id.ToString()} }
                },
                FilterExpression = "UserID = :v_Id",
                ProjectionExpression = "UserID, UserName, Email"
            };
        }
    }
}
