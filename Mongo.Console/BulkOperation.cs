using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Mongo.Console
{
    public static class BulkOperation
    {
        private static List<Product> _products = new();

        public static async Task BulkInsert()
        {
            await ReadData();
        }

        private static async Task ReadData()
        {
            using var streamReader = new StreamReader(@"C:\Temp\sample.json");
            using var reader = new JsonTextReader(streamReader);
            var serializer = new JsonSerializer();

            while (await reader.ReadAsync())
            {
                if (reader.TokenType == JsonToken.StartObject)
                {
                    var productData = serializer.Deserialize<Product>(reader);
                    await ProcessProductData(productData);
                }
            }
        }

        private static async Task ProcessProductData(Product productData)
        {
            _products.Add(productData);

            if (_products.Count % 1000 == 0)
            {
                await UpsertAsync();
                _products.Clear();
            }
        }

        private static async Task UpsertAsync()
        {
            System.Console.WriteLine($"InsertManyAsync Start: {DateTime.Now:HH:mm:ss.fff}");

            // get connection
            var collection = GetCollection();

            // add-or-update
            await collection.InsertManyAsync(_products);

            System.Console.WriteLine($"InsertManyAsync End: {DateTime.Now:HH:mm:ss.fff}");
        }

        private static IMongoCollection<Product> GetCollection()
        {
            // server
            var client = new MongoClient("mongodb://localhost:27017");

            // database
            var database = client.GetDatabase("test-db");

            // table
            var collection = database.GetCollection<Product>("Product");

            // return collection
            return collection;
        }
    }
}
