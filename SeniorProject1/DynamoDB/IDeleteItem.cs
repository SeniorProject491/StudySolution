using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject1.DynamoDB
{
    public interface IDeleteItem
    {
        Task<DeleteItemResponse> Delete(string tableName, string id);
    }
}
