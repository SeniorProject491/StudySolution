using SeniorProject1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject1.DynamoDB
{
    public interface IGetItem
    {
        Task<DynamoTableItems> GetItems(string tableName, int? id);
    }
}
