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

        public GetItem (IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        /**
         * Get items based on UserID
         */
        public async Task<DynamoTableItems> GetUserItems(string tableName, int? id)
        {
            _tableName = tableName;

            ScanRequest queryRequest; ;

            ScanResponse result;

            if (_tableName == "User")
            {
                _projectionExpression = "UserID, UserName, Email";
                _filterExpression = "UserID = :v_Id";

                queryRequest = RequestBuilder(id);
                result = await ScanAsync(queryRequest);
                return new DynamoTableItems
                {
                    User = result.Items.Select(MapUser).ToList()
                };
            } else if (_tableName == "Event")
            {                
                _projectionExpression = "EventID, UserID, EventName, EventType, Alert, EventStartTime, EventEndTime, EventLocation, Notes, Occurrence, EventStatus";
                _filterExpression = "UserID = :v_Id";
                queryRequest = RequestBuilder(id);
                result = await ScanAsync(queryRequest);
                return new DynamoTableItems
                {
                    Event = result.Items.Select(MapEvent).ToList()
                };
            } else
            {
                _projectionExpression = "NotificationID, ReceiverID, SenderID, NotificationMsg, NotificationStatus";
                _filterExpression = "ReceiverID = :v_Id";
                queryRequest = RequestBuilder(id);
                result = await ScanAsync(queryRequest);
                return new DynamoTableItems
                {
                    Notification = result.Items.Select(MapNotification).ToList()
                };
            }
        }

        /**
         *  Get item based on the item's primary key
         */
        public async Task<DynamoTableItems> GetItems(string tableName, int id)
        {
            _tableName = tableName;

            ScanRequest queryRequest; ;

            ScanResponse result;

           if (_tableName == "Event")
            {
                _projectionExpression = "EventID, UserID, EventName, EventType, Alert, EventStartTime, EventEndTime, EventLocation, Notes, Occurrence, EventStatus";
                _filterExpression = "EventID = :v_Id";
                queryRequest = RequestBuilder(id);
                result = await ScanAsync(queryRequest);
                return new DynamoTableItems
                {
                    Event = result.Items.Select(MapEvent).ToList()
                };
            }
            else if (_tableName == "Notification")
            {
                _projectionExpression = "NotificationID, ReceiverID, SenderID, NotificationMsg, NotificationStatus";
                _filterExpression = "NotificationID = :v_Id";
                queryRequest = RequestBuilder(id);
                result = await ScanAsync(queryRequest);
                return new DynamoTableItems
                {
                    Notification = result.Items.Select(MapNotification).ToList()
                };
            }

            return null;
        }

        // map the attributes of notification
        private Notification MapNotification(Dictionary<string, AttributeValue> result)
        {
            return new Notification
            {
                NotificationID = Convert.ToInt32(result["NotificationID"].N),
                ReceiverID = Convert.ToInt32(result["ReceiverID"].N),
                SenderID = Convert.ToInt32(result["SenderID"].N),
                NotificationMsg = result["NotificationMsg"].S,
                Status = Convert.ToBoolean(result["NotificationStatus"].BOOL)
            };
        }

        // map the attributes of user
        private User MapUser(Dictionary<string, AttributeValue> result)
        {
            return new User
            {
                UserID = Convert.ToInt32(result["UserID"].N),
                UserName = result["UserName"].S,
                Email = result["Email"].S
            };
        }

        // map the attributes of an event
        private Event MapEvent(Dictionary<string, AttributeValue> result)
        {
            return new Event
            {
                EventID = Convert.ToInt32(result["EventID"].N),
                UserID = Convert.ToInt32(result["UserID"].N),
                EventType = result["EventType"].S,
                EventName = result["EventName"].S,
                Location = result["EventLocation"].S,
                Occurrence = result["Occurrence"].S,
                EventStartTime = Convert.ToDateTime(result["EventStartTime"].S),
                EventEndTime = Convert.ToDateTime(result["EventEndTime"].S),
                Alert = result["Alert"].NS.Select(int.Parse).ToList(),
                Status = result["EventStatus"].BOOL,
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
    }
}
