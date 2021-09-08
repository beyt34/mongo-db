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
        private static int _total = 0;
        private static readonly List<ProductEntity> Products = new();

        public static async Task BulkInsert()
        {
            await ReadData();
        }

        private static async Task ReadData()
        {
            using var streamReader = new StreamReader(Constants.FilePath);
            using var reader = new JsonTextReader(streamReader);
            var serializer = new JsonSerializer();

            while (await reader.ReadAsync())
            {
                if (reader.TokenType == JsonToken.StartObject)
                {
                    // process row by row
                    var productData = serializer.Deserialize<ProductEntity>(reader);
                    await ProcessData(productData);
                }
            }

            // add db left records
            await UpsertAsync();
        }

        private static async Task ProcessData(ProductEntity productEntity)
        {
            _total++;
            Products.Add(productEntity);

            // every 1000 records, add db
            if (Products.Count % 10000 == 0)
            {
                await UpsertAsync();
            }
        }

        private static async Task UpsertAsync()
        {
            System.Console.WriteLine($"BulkOperation.InsertManyAsync Total:{_total} Start: {DateTime.Now:HH:mm:ss.fff}");

            // get connection
            var collection = GetCollection();

            // add-or-update
            await collection.InsertManyAsync(Products);

            // clear
            Products.Clear();

            System.Console.WriteLine($"BulkOperation.InsertManyAsync Total:{_total} End: {DateTime.Now:HH:mm:ss.fff}");
        }

        private static IMongoCollection<ProductEntity> GetCollection()
        {
            // server
            var client = new MongoClient(Constants.ConnectionString);

            // database
            var database = client.GetDatabase(Constants.Database);

            // table
            var collection = database.GetCollection<ProductEntity>(Constants.Collection);

            // return collection
            return collection;
        }
    }
}
