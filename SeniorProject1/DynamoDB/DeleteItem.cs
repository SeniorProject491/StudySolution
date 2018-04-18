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

        //delete item with primary key "id"
        public async Task<DeleteItemResponse> Delete(String tableName, string id)
        {
            _tableName = tableName;

            //get the item by id
            //var deleteItem = await _getItem.GetItems(_tableName, id);
            //var currentID = deleteItem.Items.Select(p => p.Id);

            if (_tableName == "User")
            {
                var deleteItem = await _getItem.GetUserByName(id);

                _hashKeyName = "UserName";
                _hashKey = id;

                return await DeleteUserRequest();
            }
            else
            {
                if (_tableName == "Event")
                {
                    _hashKeyName = "EventID";
                    _hashKey = id;

                    int eventID = Convert.ToInt32(id);
                    var deleteItem = await _getItem.GetEventByID(eventID);

                    _sortKeyName = "UserName";
                    _sortKey = deleteItem.UserName;
                }
                else if (_tableName == "Notification") //notification
                {
                    _hashKeyName = "NotificationID";
                    _hashKey = id; 

                    int notificationID = Convert.ToInt32(id);
                    var deleteItem = await _getItem.GetNotificationByID(notificationID);

                    _sortKeyName = "SenderName";
                    _sortKey = deleteItem.ReceiverName;
                }
                else
                {
                    return null;
                }


                var request = new DeleteItemRequest
                {
                    TableName = _tableName,
                    Key = new Dictionary<string, AttributeValue>
                {
                    {_hashKeyName, new AttributeValue{ N = _hashKey} },
                    {_sortKeyName, new AttributeValue { S = _sortKey} }                }
                };

                var response = await _dynamoDbClient.DeleteItemAsync(request);
                return response;

            }

        }
        
        public async Task<DeleteItemResponse> DeleteUserRequest()
        {
            var request = new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {_hashKey, new AttributeValue { S = _hashKey} }
                }
            };
            var response = await _dynamoDbClient.DeleteItemAsync(request);

            return response;
        }
    }
}
