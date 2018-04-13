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

        public async Task<User> UpdateUser(string userName, string email, string password)
        {
            var response = await _getItem.GetUserByName(userName);

            var currentEmail = response.Email;

            var currentPassword = response.Password;

            var request = UserRequestBuilder(userName, email, currentEmail, password, currentPassword);

            var result = await UpdatItemAsync(request);

            return new User
            {
                UserName = result.Attributes["UserName"].S,
                Email = result.Attributes["Email"].S,
                Password = result.Attributes["Password"].S,
                FriendList = result.Attributes["FriendList"].L.Select(sf => sf.S).ToList()
            };
        }

        public async Task<User> UpdateFriendList(string userName, string friendName)
        {
            var response = await _getItem.GetUserByName(userName);

            var friendList = response.FriendList;

            var request = FriendsRequestBuilder(userName, friendList, friendName);

            var result = await UpdatItemAsync(request);

            return new User
            {
                UserName = result.Attributes["UserName"].S,
                Email = result.Attributes["Email"].S,
                Password = result.Attributes["Password"].S,
                FriendList = result.Attributes["FriendList"].L.Select(sf => sf.S).ToList()
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
            var currentEvent = await _getItem.GetEventByID(id);

            //get the sort keys of the previous one
            var userName = currentEvent.UserName;

            //delete the current event with id
            var response = await _deleteItem.Delete(_tableName, id.ToString());

            //create a new object with the id and sort key
            await _putItem.AddNewEvent(id, userName, eventType, eventName, location, occurrance, startTime, endTime, notes, status);      
            
           
        }

        public async Task UpdateNotification(int id, string senderName, string notificationMsg, bool status)
        {
            _tableName = "Notification";
            //get the current object with the id
            var currentNotification = await _getItem.GetNotificationByID(id);

            //get the sort keys of the previous one
            var receiverName = currentNotification.ReceiverName;

            //delete the current event with id
            var response = await _deleteItem.Delete(_tableName, id.ToString());

            //create a new object with the id and sort key
            await _putItem.AddNotification(id, senderName, receiverName, notificationMsg, status);         
        }

        private UpdateItemRequest FriendsRequestBuilder(string userName, List<string> friendList, string friendName)
        {
            var request = new UpdateItemRequest
            {
                Key = new Dictionary<string, AttributeValue>()
                {
                    {"UserName", new AttributeValue
                    {
                        S = userName
                    } }
                },
                ExpressionAttributeNames = new Dictionary<string, string>()
                {
                    {"#F", "FriendList" }
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                {

                    {":updateList", new AttributeValue
                        {
                            L = new List<AttributeValue>
                            {
                                new AttributeValue { S = friendName }
                            }
                        }
                        
                    }
                },
                UpdateExpression = "SET #F = list_append(#F, :updateList)",
                //ConditionExpression = "#E = :currEmail AND #PW = :currPW",

                TableName = "User",
                ReturnValues = "ALL_NEW"
            };

            return request;
        }


            //id, userName, currentEmail, currentPassword
            private UpdateItemRequest UserRequestBuilder(string userName, string email, string currentEmail, string password, string currentPassword)
        {
            var request = new UpdateItemRequest
            {
                Key = new Dictionary<string, AttributeValue>
                {
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
