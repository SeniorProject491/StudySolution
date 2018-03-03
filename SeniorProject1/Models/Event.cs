using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amazon.DynamoDBv2.DataModel;


namespace SeniorProject1.Models
{
    [DynamoDBTable("Event")]
    public class Event
    {
        [DynamoDBHashKey]
        public int EventID { set; get; }

        [DynamoDBHashKey]
        public int UserID { set; get; }

        [DynamoDBProperty]
        public string EventType { set; get; }

        public string EventName { set; get; }

        public string Location { set; get; }

        public string Occurrence { set; get; }

        public DateTime EventDateTime { set; get; }

        public List<int> Alert { set; get; }

        //true = available
        public bool Status { set; get; }

        public string Notes { set; get; }


    }
}