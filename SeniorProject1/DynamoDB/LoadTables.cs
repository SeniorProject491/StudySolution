using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject1.DynamoDB
{
    public class LoadTables : ILoadTables
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private static string _tableName;
        private static string _fileName;
        private AmazonDynamoDBConfig ddbConfig = new AmazonDynamoDBConfig();

        public LoadTables(IAmazonDynamoDB dynamoDbClient)
        {
                _dynamoDbClient = dynamoDbClient;
        }

        public void LoadDynamDBTables()
        {
            try
            {
                LoadTable("User", "TableData\\userData.json");
                //LoadTable("Event", "TableData\\eventData.json");
                //LoadTable("Notification", "TableData\\notificationData.json");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        //populate the table
        public void LoadTable(string tableName, string fileName)
        {
            // First, read in the JSON data from the moviedate.json file
            _tableName = tableName;
            _fileName = fileName;
            StreamReader sr = null;
            JsonTextReader jtr = null;
            JArray userArray = null;
            try
            {
                sr = new StreamReader(_fileName);
                jtr = new JsonTextReader(sr);
                userArray = (JArray)JToken.ReadFrom(jtr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n Error: could not read from the " + _fileName + " file, because: " + ex.Message);
                throw;
            }
            finally
            {
                if (jtr != null)
                    jtr.Close();
                if (sr != null)
                    sr.Close();
            }

            // Get a Table object for the table that you created in Step 1
            Table table = GetTableObject(_tableName);
            if (table == null)
            {
                return;
            }

            // Load the data into the table (this could take some time)
            for (int i = 0; i < userArray.Count; i++)
            {
                try
                {
                    string itemJson = userArray[i].ToString();
                    Document doc = Document.FromJson(itemJson);
                    table.PutItemAsync(doc);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
        }

        //get the table object before loading
        public Table GetTableObject(string tableName)
        {
            // Now, create a Table object for the specified table
            
            Table table;
            try
            {
                table = Table.LoadTable(_dynamoDbClient, tableName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n Error: failed to load the 'Movies' table; " + ex.Message);
                return (null);
            }
            return (table);
        }
    }
}
