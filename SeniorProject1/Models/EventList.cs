using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amazon.DynamoDBv2.DataModel;


namespace SeniorProject1.Models
{
    public class EventList
    {
        public IEnumerable<Event> Events { get; set; }
    }
}