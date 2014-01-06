using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2;
using Amazon.SecurityToken;
using Amazon.Util;
using Amazon.DynamoDBv2.Model;

namespace AwsConsoleApp1
{
    class Program
    {
        private static AmazonDynamoDBClient client;
        public static void Main(string[] args)
        {
            try
            {
                var config = new AmazonDynamoDBConfig();
                config.ServiceURL = System.Configuration.ConfigurationManager.AppSettings["ServiceURL"];
                client = new AmazonDynamoDBClient(config);
         //       InsertData();
          //   GetData("5", "Test");
//                DeleteData("5", "Test");
               // InsertData1();
                //DeleteData1(5);
                //GetData1(4);
                Update1(11);
            }
            catch (Exception ex)
            { 
            
            }
        }

        public static void Update1(int Id)
        {
            var Req = new UpdateItemRequest()
            {
                TableName = "TestID",
                Key = new Dictionary<string, AttributeValue>() { { "Id", new AttributeValue { N = Id.ToString() } } },
                AttributeUpdates = new Dictionary<string, AttributeValueUpdate>() {
                {"Msg",new AttributeValueUpdate{Action="PUT",Value=new AttributeValue{S="Test Update"+Id.ToString()}} }
                }
            };
            var Rsp = client.UpdateItem(Req);
        }

        public static void InsertData()
        {
            Table testDB = Table.LoadTable(client, "Test");
            for (int i = 1; i <= 10; i++)
            {
                var test = new Document();
                test["UID"] = i.ToString();
                test["Message"] = "test" + i;
                testDB.PutItem(test);
            }
            
        }

        public static void InsertData1()
        {
            for (int i = 11; i <= 20; i++)
            {
                var Req = new PutItemRequest
                {
                    TableName = "TestID",
                    Item = new Dictionary<string, AttributeValue>() {
                    {"Id",new AttributeValue{N=i.ToString()}},
                    {"Msg",new AttributeValue{S="Test"+i}},
                    {"Memo",new AttributeValue{S="Memo  "+i}}
                    }
                };
                var Rsp = client.PutItem(Req);
            }
        }


        public static void GetData(string UID, string tableName)
        {
            Table testDB = Table.LoadTable(client, tableName);
            var request = new Amazon.DynamoDBv2.Model.GetItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string,Amazon.DynamoDBv2.Model.AttributeValue>()
                {{"UID",new Amazon.DynamoDBv2.Model.AttributeValue{S=UID}}}
               
            };
            
            var response = client.GetItem(request);
            var result = response.GetItemResult;
            Console.WriteLine("Units");
            Console.WriteLine(response.GetItemResult.ConsumedCapacity.CapacityUnits);
            Console.WriteLine("Data");
            Console.WriteLine(response.GetItemResult.Item.Count);
            Console.ReadLine();
        }

        public static void DeleteData(string UID, string tableName)
        {            
            var Req = new Amazon.DynamoDBv2.Model.DeleteItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue>() {{"UID",new Amazon.DynamoDBv2.Model.AttributeValue{S=UID}} }
            };
            
            var Rsp = client.DeleteItem(Req);        
        }

        // Get TestID
        public static void GetData1(int Id)
        {
            var Req = new Amazon.DynamoDBv2.Model.GetItemRequest
            {
                TableName = "TestID",
                Key = new Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue>() { {"Id",new Amazon.DynamoDBv2.Model.AttributeValue{N=Id.ToString()}}}
            };
            var Rsp = client.GetItem(Req);
            PrintItem(Rsp.GetItemResult.Item);
            Console.ReadLine();
        }

        // Print Item
        private static void PrintItem(Dictionary<string, AttributeValue> attributeList)
        {
            foreach (var kvp in attributeList)
            {
                string attributeName = kvp.Key;
                AttributeValue value = kvp.Value;

                Console.WriteLine(
                  attributeName + " " +
                  (value.S == null ? "" : "S=[" + value.S + "]") +
                  (value.N == null ? "" : "N=[" + value.N + "]") +
                  (value.SS == null ? "" : "SS=[" + string.Join(",", value.SS.ToArray()) + "]") +
                  (value.NS == null ? "" : "NS=[" + string.Join(",", value.NS.ToArray()) + "]")
                );
            }
            Console.WriteLine("************************************************");
        }

        // Delete TestID Table
        public static void DeleteData1(int Id)
        {
            var Req = new Amazon.DynamoDBv2.Model.DeleteItemRequest
            {
                TableName = "TestID",
                Key = new Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue>() { { "Id", new Amazon.DynamoDBv2.Model.AttributeValue { N = Id.ToString() } } }
            };
            var Rsp = client.DeleteItem(Req);

        }
    }
}