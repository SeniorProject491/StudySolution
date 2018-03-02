using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject1.DynamoDB
{
    public interface IPutItem
    {
        Task AddNewEntry(int id, string replyDateTime, double price);
    }
}
