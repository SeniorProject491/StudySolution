using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeniorProject1.DynamoDB;

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

        //get the item by the objects primary id

        [Route("getitems")]
        public async Task<IActionResult> GetItems([FromQuery] string tableName, int id)
        {
            var response = await _getItem.GetItems(tableName, id);

            return Ok(response);
        }

        //get the item by the user's ID
        [Route("getuseritems")]
        public async Task<IActionResult> GetUserItems([FromQuery] string tableName, int? id)
        {
            var response = await _getItem.GetUserItems(tableName, id);

            return Ok(response);
        }

        
        [HttpPut]
        [Route("updateitem")]
        public async Task<IActionResult> UpdateItem([FromQuery] string tableName, int id, double price)
        {
            var response = await _updateItem.Update(tableName, id, price);

            return Ok(response);
        }


        [HttpPut]
        [Route("putevent")]
        public IActionResult PutEvent([FromQuery] int eventId, int userId, string eventType, string eventName, string location, string occurrance, string startTime,
            string endTime, string notes, bool status)
        {
            _putItem.AddNewEvent(eventId, userId, eventType, eventName, location, occurrance, startTime, endTime, notes, status);
            return Ok();
        }

        [HttpPut]
        [Route("putnotification")]
        public IActionResult PutNotification([FromQuery] int id, int sender, int receiver, string message, bool status)
        {
            _putItem.AddNotification(id, sender, receiver, message, status);
            return Ok();
        }

        [HttpPut]
        [Route("putuser")]
        public IActionResult PutUser([FromQuery] int id, string username, string email, string password)
        {
            _putItem.AddNewUser(id, username, email, password);
            return Ok();
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
        public async Task<IActionResult> UpdateNotification([FromQuery] int id, int senderID, string notificationMsg, bool status)
        {
            await _updateItem.UpdateNotification(id, senderID, notificationMsg, status);
            return Ok();
        }

        [HttpPut]
        [Route("updateuser")]
        public IActionResult UpdateUser([FromQuery] int id, string userName, string email, string password)
        {
            _updateItem.UpdateUser(id, userName, email, password);
            return Ok();
        }

        [HttpDelete]
        [Route("deleteitem")]
        public async Task<IActionResult> DeleteItem([FromQuery]string tableName, int id)
        {
            var response = await _deleteItem.Delete(tableName, id);

            return Ok(response);
        }
    }
}