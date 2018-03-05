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
        private static string _tableName;
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private IGetItem _getItem;
        private static string _hashKey;
        private static string _hashKeyName;
        private static string _sortKey;
        private static string _sortKeyName;




        public DeleteItem(IAmazonDynamoDB dynamoDbClient, IGetItem getItem)
        {
            _dynamoDbClient = dynamoDbClient;
            _getItem = getItem;
        }

        public async Task<DeleteItemResponse> Delete(String tableName, int id)
        {
            _tableName = tableName;

            var deleteItem = await _getItem.GetItems(tableName, id);

            //var currentID = deleteItem.Items.Select(p => p.Id);

            if (_tableName == "User")
            {
                _hashKeyName = "UserID";
                _hashKey = id.ToString();
                _sortKeyName = "UserName";
                _sortKey = deleteItem.User.Select(p => p.UserName).FirstOrDefault();
            }
            else if (_tableName == "Event")
            {
                _hashKeyName = "EventID";
                _hashKey = deleteItem.Event.Select(p => p.EventID).ToString();
                _sortKeyName = "UserID";
                _sortKey = deleteItem.User.Select(p => p.UserID).FirstOrDefault().ToString();
            }
            else //notification
            {
                _hashKeyName = "NotificationID";
                _hashKey = deleteItem.Event.Select(p => p.EventID).ToString();
                _sortKeyName = "UserID";
                _sortKey = deleteItem.User.Select(p => p.UserID).FirstOrDefault().ToString();
            }


            var request = new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {_hashKeyName, new AttributeValue{ N = _hashKey} },
                    {_sortKeyName, new AttributeValue { N = _sortKey} }                }
            };

            var response = await _dynamoDbClient.DeleteItemAsync(request);

            return response;
        }
    }
}
