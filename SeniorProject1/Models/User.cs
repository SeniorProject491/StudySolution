using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amazon.DynamoDBv2.DataModel;

namespace SeniorProject1.Models
{
    [DynamoDBTable("User")]
    public class User
    {
        [DynamoDBHashKey]
        public int UserID { get; set; }
        [DynamoDBRangeKey]
        public string UserName { get; set; }

        [DynamoDBProperty]
        public string Email { get; set; }


    }
}