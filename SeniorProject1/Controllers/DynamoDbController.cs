using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeniorProject1.DynamoDB;
using System.Web.Http.Cors;
using SeniorProject1.Models;

namespace SeniorProject1.Controllers
{
    [Route("api/DynamoDb")]
    public class DynamoDbController : Controller
    {
        private readonly ICreateTable _createTable;
        private readonly IPutItem _putItem;
        private readonly IGetItem _getItem;
        private readonly IUpdateItem _updateItem;
        private readonly IDeleteItem _deleteItem;
        private readonly ILoadTables _loadTables;

        public DynamoDbController(ICreateTable createTable, IPutItem putItem, IGetItem getItem, IUpdateItem updateItem, IDeleteItem deleteItem, ILoadTables loadTables)
        {
            _createTable = createTable;
            _putItem = putItem;
            _getItem = getItem;
            _updateItem = updateItem;
            _deleteItem = deleteItem;
            _loadTables = loadTables;
        }

        [Route("createtable")]
        public IActionResult CreateDynamoDbTable()
        {
            _createTable.CreateDynamoDbTables();

            return Ok();
        }

        [Route("loaddata")]
        public IActionResult LoadDynamoDBTable()
        {
            _loadTables.LoadDynamDBTables();

            return Ok();
        }

        [Route("getusers")]
        public async Task<IActionResult> GetUserList()
        {
            var response = await _getItem.GetUserList();

            return Ok(response);
        }


        [Route("getuserbyname/{UserName}")]
        public async Task<IActionResult> GetUserByName(string UserName)
        {
            var response = await _getItem.GetUserByName(UserName);

            return Ok(response);
        }

        [Route("geteventbyname/{UserName}")]
        public async Task<IActionResult> GetEventByName(string UserName)
        {
            var response = await _getItem.GetEventByName(UserName);

            return Ok(response);
        }

        [Route("getnotificationbyname/{UserName}")]
        public async Task<IActionResult> GetNotificationByName(string UserName)
        {
            var response = await _getItem.GetNotificationByName(UserName);

            return Ok(response);
        }

        [Route("getnotificationbyid/{id}")]
        public async Task<IActionResult> GetNotificationByID(int id)
        {
            var response = await _getItem.GetNotificationByID(id);

            return Ok(response);
        }

        [Route("geteventbyid/{id}")]
        public async Task<IActionResult> GetEventByID(int id)
        {
            var response = await _getItem.GetEventByID(id);

            return Ok(response);
        }

        //get the item by the objects primary id
        //[Route("getitems")]
        //public async Task<IActionResult> GetItems([FromQuery] string tableName, int id)
        //{
        //    var response = await _getItem.GetItems(tableName, id);

        //    return Ok(response);
        //}

        ////get the item by the user's ID
        //[Route("getuseritems")]
        //public async Task<IActionResult> GetUserItems([FromQuery] string tableName, int? id)
        //{
        //    var response = await _getItem.GetEventByName(tableName, id);

        //    return Ok(response);
        //}


        [HttpPut]
        [Route("updateuser")]
        public async Task<IActionResult >UpdateUser([FromBody] User user)
        {
            var response = await _updateItem.UpdateUser(user.UserName, user.Email, user.Password);

            return Ok(response);
        }

        [HttpPut]
        [Route("updatefriendlist/{userName}/{friendName}")]
        public async Task<IActionResult> UpdateFriendList (string userName, string friendName)
        {

            var response = await _updateItem.UpdateFriendList(userName, friendName);

            return Ok(response);
        }

        [HttpPut]
        [Route("updateevent")]
        ///{id}/{eventType}/{eventName}/{location}/{occurrance}
        public async Task<IActionResult> UpdateEvent([FromBody] Event evnt)
        {
            await _updateItem.UpdateEvent(evnt.EventID, evnt.EventType, evnt.EventName, evnt.Location, evnt.Occurrance, evnt.EventStartTime.ToString(), evnt.EventEndTime.ToString(), evnt.Notes, evnt.Status);
            return Ok();
        }


        [HttpPut]
        [Route("updatenotification")]
        public async Task<IActionResult> UpdateNotification([FromBody] Notification notification)//int id, string senderName, string notificationMsg, bool status)
        {
            await _updateItem.UpdateNotification(notification.NotificationID, notification.SenderName, notification.NotificationMsg, notification.Status);
            return Ok();
        }

        //[HttpPut]
        //[Route("updateuser")]
        //public IActionResult UpdateUser([FromQuery] int id, string userName, string email, string password)
        //{
        //    _updateItem.UpdateUser(id, userName, email, password);
        //    return Ok();
        //}

        [HttpPost]
        [Route("putevent")]
        //[Route("putevent/{eventId}/{userName}/{eventType}/{eventName}/{location}/{occurrance}/{startTime}/{endTime}/{notes}/{status}")]
        //int eventId, string userName, string eventType, string eventName, string location, string occurrance, string startTime, string endTime, string notes, bool status
        public IActionResult PutEvent([FromBody] Event evnt)
        {
            _putItem.AddNewEvent(evnt.EventID, evnt.UserName, evnt.EventType, evnt.EventName, evnt.Location, evnt.Occurrance, evnt.EventStartTime.ToString(), evnt.EventEndTime.ToString(), evnt.Notes, evnt.Status);
            return Ok();
        }

        [HttpPost]
        [Route("putnotification")]
        public IActionResult PutNotification([FromBody] Notification notification)
        {
            _putItem.AddNewNotification(notification.NotificationID, notification.SenderName, notification.ReceiverName, notification.NotificationMsg, notification.Status);
            return Ok();
        }

        [HttpPost]
        [Route("putuser")]
        public IActionResult PutUser([FromBody] User user)
        {
            _putItem.AddNewUser(user.UserName, user.Email, user.Password);
            return Ok();
        }

        //[HttpPut]
        //[Route("postuser")]
        //public IActionResult PostUser([FromBody] string username, string email, string password)
        //{
        //    _putItem.AddNewUser(username, email, password);
        //    return Ok();
        //}

        [HttpDelete]
        [Route("deleteitem/{tableName}/{id}")]
        public async Task<IActionResult> DeleteItem([FromQuery]string tableName, string id)
        {
            var response = await _deleteItem.Delete(tableName, id);

            return Ok(response);
        }
    }
}