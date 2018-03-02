using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject1.Models
{
    public class DynamoTableItems
    {
        public IEnumerable<Item> Items { get; set; }

        public IEnumerable<User> User { get; set; }
    }
}
