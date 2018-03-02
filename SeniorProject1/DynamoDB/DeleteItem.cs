using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject1.DynamoDB
{
    public class DeleteItem : IDeleteItem
    {
        private static readonly string tableName = "TempDynamoDbTable";
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private IGetItem _getItem;


        public DeleteItem(IAmazonDynamoDB dynamoDbClient, IGetItem getItem)
        {
            _dynamoDbClient = dynamoDbClient;
            _getItem = getItem;
        }

        public async Task<DeleteItemResponse> Delete(int id)
        {
            var deleteItem = await _getItem.GetItems(id);

            //var currentID = deleteItem.Items.Select(p => p.Id);

            var replyDateTime = deleteItem.Items.Select(p => p.ReplyDateTime).FirstOrDefault();

            var request = new DeleteItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"Id", new AttributeValue{ N = id.ToString()} },
                    {"ReplyDateTime", new AttributeValue { N = replyDateTime.ToString()} }                }
            };

            var response = await _dynamoDbClient.DeleteItemAsync(request);

            return response;
        }
    }
}
