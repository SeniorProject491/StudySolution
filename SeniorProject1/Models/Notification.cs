using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amazon.DynamoDBv2.DataModel;


namespace SeniorProject1.Models
{
    public class Notification
    {
        [DynamoDBHashKey]
        public int NotificationID { set; get; }

        [DynamoDBRangeKey]
        public string ReceiverName { set; get; }

        [DynamoDBProperty]
        public string SenderName { set; get; }

        [DynamoDBProperty]
        public string NotificationMsg { set; get; }

        // true = accepted
        public bool Status { set; get; }
    }
}