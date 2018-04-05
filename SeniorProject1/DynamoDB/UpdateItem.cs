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

        public async Task<User> UpdateUser(int id, string userName, string email, string password)
        {
            var response = await _getItem.GetUserItems("User", id);

            var currentEmail = response.User.Select(p => p.Email).FirstOrDefault();

            var currentPassword = response.User.Select(p => p.Password).FirstOrDefault();

            var currentUserName = response.User.Select(p => p.UserName).FirstOrDefault();

            var request = RequestBuilder(id, userName, email, currentEmail, password, currentPassword);

            var result = await UpdatItemAsync(request);

            return new User
            {
                UserID = Convert.ToInt32(result.Attributes["UserID"].N),
                UserName = result.Attributes["UserName"].S,
                Email = result.Attributes["Email"].S,
                Password = result.Attributes["Password"].S
            };
        }

        //public async Task UpdateUser(int id, string userName , string Email, string password)
        //{
        //    _tableName = "User";
        //    //get the current object with the id
        //    var currentUser = await _getItem.GetUserItems(_tableName, id);

        //    //get the sort keys of the previous one
        //    var currentUserName = currentUser.User.Select(p => p.UserName).FirstOrDefault();

        //    //delete the current event with id
        //    var response = await _deleteItem.Delete(_tableName, id);

        //    //create a new object with the id and sort key
        //    await _putItem.AddNewUser(id, currentUserName, Email, password);

        //}

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

        //id, userName, currentEmail, currentPassword
        private UpdateItemRequest RequestBuilder(int id, string userName, string email, string currentEmail, string password, string currentPassword)
        {
            var request = new UpdateItemRequest
            {
                Key = new Dictionary<string, AttributeValue>
                {
                    {"UserID", new AttributeValue
                    {
                        N = id.ToString()
                    } },
                    {"UserName", new AttributeValue
                    {
                        S = userName
                    } }
                },
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    {"#E", "Email" },
                    {"#PW", "Password" }
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    
                    {":newPW", new AttributeValue
                    {
                        S = password
                    } },
                    {":currPW", new AttributeValue
                    {
                        S = currentPassword
                    } },
                    {":newEmail", new AttributeValue
                    {
                        S = email
                    } },
                    {":currEmail", new AttributeValue
                    {
                        S = currentEmail
                    } }
                },
                UpdateExpression = "SET #E = :newEmail, #PW = :newPW",
                ConditionExpression = "#E = :currEmail AND #PW = :currPW",

                TableName = "User",
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
