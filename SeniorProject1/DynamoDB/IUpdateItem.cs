using SeniorProject1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject1.DynamoDB
{
    public interface IUpdateItem
    {
        Task<User> UpdateUser(string userName, string Email, string password);
        Task UpdateEvent(int id, string eventType, string eventName, string location, string occurance, string startTime, string endTime, string notes,  bool status);
        Task UpdateNotification(int id, string senderName, string notificationMsg, bool status);
        Task<User> UpdateFriendList(string userName, string friendName);

    }
}
