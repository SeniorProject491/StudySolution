﻿using System;
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

        public DynamoDbController(ICreateTable createTable, IPutItem putItem, IGetItem getItem, IUpdateItem updateItem, IDeleteItem deleteItem)
        {
            _createTable = createTable;
            _putItem = putItem;
            _getItem = getItem;
            _updateItem = updateItem;
            _deleteItem = deleteItem;
        }

        [Route("createtable")]
        public IActionResult CreateDynamoDbTable()
        {
            _createTable.CreateDynamoDbTable();

            return Ok();
        }

        [Route("getitems")]
        public async Task<IActionResult> GetItems([FromQuery] string tableName, int id)
        {
            var response = await _getItem.GetItems(tableName, id);

            return Ok(response);
        }

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
            return Ok();
        }

        [HttpPut]
        [Route("putUser")]
        public IActionResult putUser([FromQuery] int id, string username, string email, string name, string password)
        {
            _putItem.AddNewUser(id, username, email, name, password);
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