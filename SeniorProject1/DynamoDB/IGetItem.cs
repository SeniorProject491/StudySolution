using SeniorProject1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject1.DynamoDB
{
    public interface IGetItem
    {
        //Task<DynamoTableItems> GetItems(string tableName, int id);
        Task<User> GetUserByName(string UserName);
        Task<Event> GetEventByID(int id);
        Task<EventList> GetEventByName(string userName);
        Task<NotificationList> GetNotificationByName(string userName);
        Task<Notification> GetNotificationByID(int id);
        Task<UserList> GetUserList();
        Task<EventList> GetAllEvents();
    }
}
