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

        public async Task AddNewEvent(int id, int userId, string eventType, string eventName, string location, string occurance, string startTime,
            string endTime, List<int> alerts, string notes, bool status)
        {
            var queryRequest = RequestBuilder(id, userId, eventType, eventName, location, occurance, startTime, endTime, alerts, notes, status);

            await PutItemAsync(queryRequest);
        }

        private PutItemRequest RequestBuilder(int id, int userId, string eventType, string eventName, string location, string occurance, string startTime,
            string endTime, List<int> alerts, string notes, bool status)
        {
            var userEvent = new Dictionary<string, AttributeValue>
            {
                {"EventId", new AttributeValue {N = id.ToString()}},
                {"UserId", new AttributeValue {N = userId.ToString()}},
                {"EventType", new AttributeValue {N = eventType}},
                {"EventName", new AttributeValue {N = eventName}},
                {"EventLocation", new AttributeValue {N = location}},
                {"Occurance", new AttributeValue {N = occurance}},
                {"EventStartTime", new AttributeValue {N = startTime}},
                {"EventEndTime", new AttributeValue {N = endTime}},
                {"Alert", new AttributeValue {N = alerts.ToString()}},
                {"Notes", new AttributeValue {N = notes}},
                {"EventStatus", new AttributeValue {N = status.ToString()}}
            };

            return new PutItemRequest
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
            var userNotification = new Dictionary<string, AttributeValue>
            {
                {"NotificationID", new AttributeValue {N = id.ToString()}},
                {"ReceiverID", new AttributeValue {N = receiver.ToString()}},
                {"NotificationMsg", new AttributeValue {N = message}},
                {"NotificationStatus", new AttributeValue {N = status.ToString()}},
                {"SenderID", new AttributeValue {N = sender.ToString()}}
            };

            return new PutItemRequest
            {
                TableName = "Notification",
                Item = userNotification
            };
        }

        public async Task AddNewUser(int id, string username, string email, string name, string password)
        {
            var queryRequest = RequestBuilder(id, username, email, name, password);

            await PutItemAsync(queryRequest);
        }

        private PutItemRequest RequestBuilder(int id, string username, string email, string name, string password)
        {
            var newUser = new Dictionary<string, AttributeValue>
            {
                {"UserID", new AttributeValue {N = id.ToString()}},
                {"UserName", new AttributeValue {N = username}},
                {"Email", new AttributeValue {N = email}},
                {"FullName", new AttributeValue {N = name}},
                {"Password", new AttributeValue {N = password}}
            };

            return new PutItemRequest
            {
                TableName = "Users",
                Item = newUser
            };
        }


        private async Task PutItemAsync(PutItemRequest request)
        {
            await _dynamoDbClient.PutItemAsync(request);
        }


    }
}
