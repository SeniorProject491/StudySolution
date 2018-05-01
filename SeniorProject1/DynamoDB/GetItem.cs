using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SeniorProject1.Models;
using System.Globalization;

namespace SeniorProject1.DynamoDB
{
    public class GetItem : IGetItem
    {
        private static string _tableName;
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private static string _projectionExpression;
        private static string _filterExpression;

        ScanRequest queryRequest;

        ScanResponse result;

        public GetItem (IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task<UserList> GetUserList()
        {
            _tableName = "User";
            queryRequest = RequestBuilder(null);
            result = await ScanAsync(queryRequest);
            return new UserList
            {
                Users = result.Items.Select(MapUser).ToList()
            };

        }

        public async Task<User> GetUserByName(string UserName)
        {
            _projectionExpression = "UserName, Email, Password, FriendList";
            _filterExpression = "UserName = :v_Name";

            queryRequest = UserRequestBuilder("User",   UserName);
            result = await ScanAsync(queryRequest);

            return result.Items.Select(MapUser).FirstOrDefault();
        }

        /**
         * Get items based on UserID
         */
        public async Task<EventList> GetEventByName(string userName)
        {
            _tableName = "Event";
            _projectionExpression = "EventID, UserName, EventName, EventType, Alert, EventStartTime, EventEndTime, EventLocation, Notes, Occurrance, EventStatus";
            _filterExpression = "UserName = :v_Name";
            queryRequest = UserRequestBuilder(_tableName, userName);
            result = await ScanAsync(queryRequest);
            return new EventList
            {
                Events = result.Items.Select(MapEvent).ToList()
            };
        }

        public async Task<EventList> GetAllEvents()
        {
            _tableName = "Event";
            _projectionExpression = "EventID, UserName, EventName, EventType, Alert, EventStartTime, EventEndTime, EventLocation, Notes, Occurrance, EventStatus";
            _filterExpression = "UserName = :v_Name";
            queryRequest = RequestBuilder(null);
            result = await ScanAsync(queryRequest);
            return new EventList
            {
                Events = result.Items.Select(MapEvent).ToList()
            };
        }

        public async Task<NotificationList> GetNotificationByName(string userName)
        {
            _tableName = "Notification";
            _projectionExpression = "NotificationID, ReceiverName, SenderName, NotificationMsg, NotificationStatus";
            _filterExpression = "ReceiverName = :v_Name";
            queryRequest = UserRequestBuilder(_tableName, userName);
            result = await ScanAsync(queryRequest);
            return new NotificationList
            {
                Notifications = result.Items.Select(MapNotification).ToList()
            };
        }


        /**
         *  Get item based on the item's primary key
         */
        public async Task<Event> GetEventByID(int id)
        {
            _tableName = "Event";
            _projectionExpression = "EventID, UserName, EventName, EventType, Alerts, EventStartTime, EventEndTime, EventLocation, Notes, Occurrance, EventStatus";
            _filterExpression = "EventID = :v_Id";
            queryRequest = RequestBuilder(id);
            result = await ScanAsync(queryRequest);
            return result.Items.Select(MapEvent).FirstOrDefault();
        }

        public async Task<Notification> GetNotificationByID(int id)
        { 
            _tableName = "Notification";
            _projectionExpression = "NotificationID, ReceiverName, SenderName, NotificationMsg, NotificationStatus";
            _filterExpression = "NotificationID = :v_Id";
            queryRequest = RequestBuilder(id);
            result = await ScanAsync(queryRequest);
            return result.Items.Select(MapNotification).FirstOrDefault();
        }
        

        // map the attributes of notification
        private Notification MapNotification(Dictionary<string, AttributeValue> result)
        {
            return new Notification
            {
                NotificationID = Convert.ToInt32(result["NotificationID"].N),
                ReceiverName = result["ReceiverName"].S,
                SenderName = result["SenderName"].S,
                NotificationMsg = result["NotificationMsg"].S,
                Status = Convert.ToBoolean(result["NotificationStatus"].BOOL)
            };
        }

        // map the attributes of user
        private User MapUser(Dictionary<string, AttributeValue> result)
        {
            return new User
            {
                UserName = result["UserName"].S,
                Email = result["Email"].S,
                Password = result["Password"].S,
                FriendList = result["FriendList"].L.Select(sf => sf.S).ToList()
            };
        }

        // map the attributes of an event
        private Event MapEvent(Dictionary<string, AttributeValue> result)
        {
            return new Event
            {
                EventID = Convert.ToInt32(result["EventID"].N),
                UserName = result["UserName"].S,
                EventType = result["EventType"].S,
                EventName = result["EventName"].S,
                Location = result["EventLocation"].S,
                Occurrance = result["Occurrance"].S,
                EventStartTime = Convert.ToDateTime(result["EventStartTime"].S),
                EventEndTime = Convert.ToDateTime(result["EventEndTime"].S),
                Notes = result["Notes"].S
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
                    TableName = _tableName

                };
            }

            return new ScanRequest
            {
                TableName = _tableName,
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":v_Id", new AttributeValue{N = id.ToString()} }
                },
                FilterExpression = _filterExpression,
                ProjectionExpression = _projectionExpression
            };
        }

        private ScanRequest UserRequestBuilder(string tableName,string UserName)
        {
            _tableName = tableName;

            return new ScanRequest
            {
                TableName = _tableName,
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":v_Name", new AttributeValue{S = UserName} }
                },
                FilterExpression = _filterExpression,
                ProjectionExpression = _projectionExpression
            };
        }
    }
}
