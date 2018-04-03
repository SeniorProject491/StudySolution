using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using SeniorProject1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject1.DynamoDB
{
    public class UpdateItem : IUpdateItem
    {
        private static string _tableName;
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private IGetItem _getItem;
        private IPutItem _putItem;
        private IDeleteItem _deleteItem;

        public UpdateItem(IAmazonDynamoDB dynamoDbClient, IGetItem getItem, IPutItem putItem, IDeleteItem deleteItem)
        {
            _dynamoDbClient = dynamoDbClient;
            _getItem = getItem;
            _putItem = putItem;
            _deleteItem = deleteItem;
        }

        public async Task<Item> Update (string tableName, int id, double price)
        {
            var response = await _getItem.GetItems(tableName, id);

            var currentPrice = response.Items.Select(p => p.Price).FirstOrDefault();

            var replyDateTime = response.Items.Select(p => p.ReplyDateTime).FirstOrDefault();

            var request = RequestBuilder(id, price, currentPrice, replyDateTime);

            var result = await UpdatItemAsync(request);

            return new Item
            {
                Id = Convert.ToInt32(result.Attributes["Id"].N),
                ReplyDateTime = result.Attributes["ReplyDateTime"].N,
                Price = Convert.ToDouble(result.Attributes["Price"].N)
            };
        }

        public async Task UpdateUser(int id, string userName , string Email, string password)
        {
            _tableName = "User";
            //get the current object with the id
            var currentUser = await _getItem.GetItems(_tableName, id);

            //get the sort keys of the previous one
            var currentUserName = currentUser.User.Select(p => p.UserName).FirstOrDefault();

            //delete the current event with id
            var response = await _deleteItem.Delete(_tableName, id);

            //create a new object with the id and sort key
            await _putItem.AddNewUser(id, currentUserName, Email, password);

        }

        public async Task UpdateEvent(int id, string eventType, string eventName, string location, string occurrance, string startTime, string endTime, string notes, bool status)
        {
            _tableName = "Event";
            //get the current object with the id
            var currentEvent = await _getItem.GetItems(_tableName, id);

            //get the sort keys of the previous one
            var userID = currentEvent.Event.Select(p => p.UserID).FirstOrDefault();

            //delete the current event with id
            var response = await _deleteItem.Delete(_tableName, id);

            //create a new object with the id and sort key
            await _putItem.AddNewEvent(id, userID, eventType, eventName, location, occurrance, startTime, endTime, notes, status);      
            
           
        }

        public async Task UpdateNotification(int id, int senderID, string notificationMsg, bool status)
        {
            _tableName = "Notification";
            //get the current object with the id
            var currentNotification = await _getItem.GetItems(_tableName, id);

            //get the sort keys of the previous one
            var receiverID = currentNotification.Notification.Select(p => p.ReceiverID).FirstOrDefault();

            //delete the current event with id
            var response = await _deleteItem.Delete(_tableName, id);

            //create a new object with the id and sort key
            await _putItem.AddNotification(id, senderID, receiverID, notificationMsg, status);         
        }


        private UpdateItemRequest RequestBuilder(int id, double price, double currentPrice, string replyDateTime)
        {
            var request = new UpdateItemRequest
            {
                Key = new Dictionary<string, AttributeValue>
                {
                    {"Id", new AttributeValue
                    {
                        N = id.ToString()
                    } },
                    {"ReplyDateTime", new AttributeValue
                    {
                        N = replyDateTime
                    } }
                },
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    {"#P", "Price" }
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":newprice", new AttributeValue
                    {
                        N = price.ToString()
                    } },
                    {":currprice", new AttributeValue
                    {
                        N = currentPrice.ToString()
                    } }
                },
                UpdateExpression = "SET #P = :newprice",
                ConditionExpression = "#P = :currprice",

                TableName = _tableName,
                ReturnValues = "ALL_NEW"
            };

            return request;
        }


        private async Task<UpdateItemResponse> UpdatItemAsync(UpdateItemRequest request)
        {
            var response = await _dynamoDbClient.UpdateItemAsync(request);

            return response;
        }
    }
}
