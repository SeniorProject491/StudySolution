using SeniorProject1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject1.DynamoDB
{
    public interface IUpdateItem
    {
        Task<Item> Update(string tableName, int id, double price);
        Task UpdateNotification(int id, int senderID, string notificationMsg, bool status);
        Task UpdateEvent(int id, string eventType, string eventName, string location, string occurance, string startTime, string endTime, string notes,  bool status);
        Task UpdateUser(int id, string userName, string Email, string password);
    }
}
