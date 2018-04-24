using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject1.DynamoDB
{
    public interface IPutItem
    {
        Task AddNewEvent(int id, string userName, string eventType, string eventName, string location, string occurance, string startTime,
            string endTime, string notes, bool status);

        Task AddNewNotification(int id, string sender, string receiver, string message, bool status);

        Task AddNewUser(string username, string email, string password);
    }
}         
