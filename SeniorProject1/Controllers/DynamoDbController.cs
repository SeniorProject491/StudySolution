using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeniorProject1.DynamoDB;
using System.Web.Http.Cors;

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


        [Route("getuserbyname")]
        public async Task<IActionResult> GetUserByName(string UserName)
        {
            var response = await _getItem.GetUserByName(UserName);

            return Ok(response);
        }

        [Route("geteventbyname")]
        public async Task<IActionResult> GetEventByName(string userName)
        {
            var response = await _getItem.GetEventByName(userName);

            return Ok(response);
        }

        [Route("getnotificationbyname")]
        public async Task<IActionResult> GetNotificationByName(string userName)
        {
            var response = await _getItem.GetNotificationByName(userName);

            return Ok(response);
        }

        [Route("getnotificationbyid")]
        public async Task<IActionResult> GetNotificationByID(int id)
        {
            var response = await _getItem.GetNotificationByID(id);

            return Ok(response);
        }

        [Route("geteventbyid")]
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
        public async Task<IActionResult >UpdateUser([FromQuery] string userName, string email, string password)
        {
            var response = await _updateItem.UpdateUser(userName, email, password);

            return Ok(response);
        }

        [HttpPut]
        [Route("updatefriendlist")]
        public async Task<IActionResult> UpdateFriendList ([FromQuery] string userName, string friendName)
        {

            var response = await _updateItem.UpdateFriendList(userName, friendName);

            return Ok(response);
        }

        [HttpPut]
        [Route("updateevent")]
        public async Task<IActionResult> UpdateEvent([FromQuery] int id, string eventType, string eventName, string location, string occurrance, 
            string startTime, string endTime,string notes, bool status)
        {
            await _updateItem.UpdateEvent(id, eventType, eventName, location, occurrance, startTime, endTime, notes, status);
            return Ok();
        }


        [HttpPut]
        [Route("updatenotification")]
        public async Task<IActionResult> UpdateNotification([FromQuery] int id, string senderName, string notificationMsg, bool status)
        {
            await _updateItem.UpdateNotification(id, senderName, notificationMsg, status);
            return Ok();
        }

        //[HttpPut]
        //[Route("updateuser")]
        //public IActionResult UpdateUser([FromQuery] int id, string userName, string email, string password)
        //{
        //    _updateItem.UpdateUser(id, userName, email, password);
        //    return Ok();
        //}

        [HttpPut]
        [Route("putevent")]
        public IActionResult PutEvent([FromQuery] int eventId, string userName, string eventType, string eventName, string location, string occurrance, string startTime,
           string endTime, string notes, bool status)
        {
            _putItem.AddNewEvent(eventId, userName, eventType, eventName, location, occurrance, startTime, endTime, notes, status);
            return Ok();
        }

        [HttpPut]
        [Route("putnotification")]
        public IActionResult PutNotification([FromQuery] int id, string sender, string receiver, string message, bool status)
        {
            _putItem.AddNotification(id, sender, receiver, message, status);
            return Ok();
        }

        [HttpPut]
        [Route("putuser")]
        public IActionResult PutUser([FromQuery] string username, string email, string password)
        {
            _putItem.AddNewUser(username, email, password);
            return Ok();
        }

        [HttpDelete]
        [Route("deleteitem")]
        public async Task<IActionResult> DeleteItem([FromQuery]string tableName, string id)
        {
            var response = await _deleteItem.Delete(tableName, id);

            return Ok(response);
        }
    }
}