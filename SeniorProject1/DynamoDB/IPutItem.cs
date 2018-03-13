using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject1.DynamoDB
{
    public interface IPutItem
    {
        Task AddNewEvent(int id, int userId, string eventType, string eventName, string location, string occurance, string startTime,
            string endTime, List<int> alerts, string notes, bool status);

        Task AddNotification(int id, int sender, int receiver, string message, bool status);

        Task AddNewUser(int id, string username, string email, string name, string password);
    }
}         
