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

        [DynamoDBHashKey]
        public int ReceiverID { set; get; }

        [DynamoDBProperty]
        public int SenderID { set; get; }

        [DynamoDBProperty]
        public string NotificationMsg { set; get; }

        // true = accepted
        public bool Status { set; get; }
    }
}