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

        public async Task AddNewEvent(int id, int userId, string eventType, string eventName, string location, string occurance, string startTime,
            string endTime, string notes, bool status)
        {
            var queryRequest = RequestBuilder(id, userId, eventType, eventName, location, occurance, startTime, endTime, notes, status);
            await PutItemAsync(queryRequest);
        }

        private PutItemRequest RequestBuilder(int id, int userId, string eventType, string eventName, string location, string occurrance, string startTime,
            string endTime, string notes, bool status)
        {
            var userEvent = new Dictionary<string, AttributeValue>()
            {
                {"EventID", new AttributeValue {N = id.ToString()}},
                {"UserID", new AttributeValue {N = userId.ToString()}},
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


        public async Task AddNotification(int id, int sender, int receiver, string message, bool status)
        {
            var queryRequest = RequestBuilder(id, sender, receiver, message, status);

            await PutItemAsync(queryRequest);
        }

        private PutItemRequest RequestBuilder(int id, int sender, int receiver, string message, bool status)
        {
            var userNotification = new Dictionary<string, AttributeValue>()
            {
                {"NotificationID", new AttributeValue {N = id.ToString()}},
                {"ReceiverID", new AttributeValue {N = receiver.ToString()}},
                {"NotificationMsg", new AttributeValue {S = message}},
                {"NotificationStatus", new AttributeValue {BOOL = status}},
                {"SenderID", new AttributeValue {N = sender.ToString()}}
            };

            return new PutItemRequest
            {
                TableName = "Notification",
                Item = userNotification
            };
        }

        public async Task AddNewUser(int id, string username, string email, string password)
        {
            var queryRequest = RequestBuilder(id, username, email, password);
            await PutItemAsync(queryRequest);
        }

        private PutItemRequest RequestBuilder(int id, string username, string email, string password)
        {
            var newUser = new Dictionary<string, AttributeValue>()
            {
                {"UserID", new AttributeValue {N = id.ToString()}},
                {"UserName", new AttributeValue {S = username}},
                {"Email", new AttributeValue {S = email}},
                {"Password", new AttributeValue{S = password} }
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
