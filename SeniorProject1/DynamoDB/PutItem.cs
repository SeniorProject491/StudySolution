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
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public PutItem(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task AddNewUser(string username, string email, string password)
        {
            var queryRequest = UserRequestBuilder(username, email, password);
            await PutItemAsync(queryRequest);
        }

        public async Task AddNewEvent(int id, string userName, string eventType, string eventName, string location, string occurance, string startTime,
            string endTime, string notes, bool status)
        {
            var queryRequest = EventRequestBuilder(id, userName, eventType, eventName, location, occurance, startTime, endTime, notes, status);
            await PutItemAsync(queryRequest);
        }

        public async Task AddNewNotification(int id, string sender, string receiver, string message, bool status)
        {
            var queryRequest = NotificationRequestBuilder(id, sender, receiver, message, status);
            //Console.WriteLine(id.ToString(), " ", sender, " ", receiver, " ", message, " ", status);
            await PutItemAsync(queryRequest);
        }

        private PutItemRequest EventRequestBuilder(int id, string userName, string eventType, string eventName, string location, string occurrance, string startTime,
            string endTime, string notes, bool status)
        {
            var userEvent = new Dictionary<string, AttributeValue>()
            {
                {"EventID", new AttributeValue {N = id.ToString()}},
                {"UserName", new AttributeValue {S = userName.ToString()}},
                {"EventType", new AttributeValue {S = eventType}},
                {"EventName", new AttributeValue {S = eventName}},
                {"EventLocation", new AttributeValue {S = location}},
                {"Occurrance", new AttributeValue {S = occurrance}},
                {"EventStartTime", new AttributeValue {S = startTime}},
                {"EventEndTime", new AttributeValue {S = endTime}},
                //{"Alerts", new AttributeValue {NS = alerts.ConvertAll(delegate(int i) { return i.ToString(); })} },
                {"Notes", new AttributeValue {S = notes}},
                {"EventStatus", new AttributeValue {BOOL = status}}
            };

            return new PutItemRequest()
            {
                TableName = "Event",
                Item = userEvent
            };
        }

        private PutItemRequest NotificationRequestBuilder(int id, string sender, string receiver, string message, bool status)
        {
            var userNotification = new Dictionary<string, AttributeValue>()
            {
                {"NotificationID", new AttributeValue {N = id.ToString()}},
                {"ReceiverName", new AttributeValue {S = receiver}},
                {"NotificationMsg", new AttributeValue {S = message}},
                {"NotificationStatus", new AttributeValue {BOOL = status}},
                {"SenderName", new AttributeValue {S = sender}}
            };

            return new PutItemRequest
            {
                TableName = "Notification",
                Item = userNotification
            };
        }

      

        private PutItemRequest UserRequestBuilder(string username, string email, string password)
        {
            var newUser = new Dictionary<string, AttributeValue>()
            {
                {"UserName", new AttributeValue {S = username}},
                {"Email", new AttributeValue {S = email}},
                {"Password", new AttributeValue{S = password} },
                {"FriendList", new AttributeValue
                    {
                        L = new List<AttributeValue>
                        {
                            new AttributeValue { S = "Empty" }
                        }
                    }
                }

            };

            return new PutItemRequest
            {
                TableName = "User",
                Item = newUser
            };
        }

        private async Task PutItemAsync(PutItemRequest request)
        {
            await _dynamoDbClient.PutItemAsync(request);
        }
    }
}
