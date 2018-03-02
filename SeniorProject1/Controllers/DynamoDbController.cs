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

        [Route("putitem")]
        public IActionResult PutItem([FromQuery] int id, string replyDateTime, double price)
        {
            _putItem.AddNewEntry(id, replyDateTime, price);

            return Ok();
        }

        [Route("getitems")]
        public async Task<IActionResult> GetItems([FromQuery] int? id)
        {
            var response = await _getItem.GetItems(id);

            return Ok(response);
        }

        [HttpPut]
        [Route("updateitem")]
        public async Task<IActionResult> UpdateItem([FromQuery] int id, double price)
        {
            var response = await _updateItem.Update(id, price);

            return Ok(response);
        }

        [HttpDelete]
        [Route("deleteitem")]
        public async Task<IActionResult> DeleteItem([FromQuery] int id)
        {
            var response = await _deleteItem.Delete(id);

            return Ok(response);
        }
    }
}