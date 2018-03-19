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

<<<<<<< HEAD
=======
        [Route("putitem")]
        public IActionResult PutItem([FromQuery] int id, string replyDateTime, double price)
        {
            _putItem.AddNewEntry(id, replyDateTime, price);

            return Ok();
        }

        //get the item by the objects primary id
>>>>>>> testbranch
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
<<<<<<< HEAD
        [Route("putEvent")]
        public IActionResult PutItem([FromQuery] int id, int userId, string eventType, string eventName, string location, string occurance, string startTime,
            string endTime, List<int> alerts, string notes, bool status)
        {
            _putItem.AddNewEvent(id, userId, eventType, eventName, location, occurance, startTime, endTime, alerts, notes, status);
            return Ok();
        }

        [HttpPut]
        [Route("putNotification")]
        public IActionResult putNotification([FromQuery] int id, int sender, int receiver, string message, bool status)
        {
            _putItem.AddNotification(id, sender, receiver, message, status);
=======
        [Route("updateevent")]
        public async Task<IActionResult> UpdateEvent([FromQuery] int id, string eventType, string eventName, string location, string occurance, string startTime, string endTime, string notes, List<int> alerts)
        {
            await _updateItem.UpdateEvent(id, eventType, eventName, location, occurance, startTime, endTime, notes, alerts);

>>>>>>> testbranch
            return Ok();
        }

        [HttpPut]
<<<<<<< HEAD
        [Route("putUser")]
        public IActionResult putUser([FromQuery] int id, string username, string email, string name, string password)
        {
            _putItem.AddNewUser(id, username, email, name, password);
=======
        [Route("updatenotification")]
        public async Task<IActionResult> UpdateNotification([FromQuery] int id, int senderID, string notificationMsg, bool status)
        {
            await _updateItem.UpdateNotification(id, senderID, notificationMsg, status);

>>>>>>> testbranch
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