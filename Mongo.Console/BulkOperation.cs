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
        private static int _total;
        private static readonly List<ProductTempEntity> Products = new();

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
                    var productData = serializer.Deserialize<ProductTempEntity>(reader);
                    await ProcessData(productData);
                }
            }

            // add db left records
            await UpsertAsync();
        }

        private static async Task ProcessData(ProductTempEntity productTempEntity)
        {
            _total++;
            Products.Add(productTempEntity);

            // every 1000 records, add db
            if (Products.Count % 10000 == 0)
            {
                await UpsertAsync();
            }
        }

        private static async Task UpsertAsync()
        {
            if (Products.Count > 0)
            {
                System.Console.WriteLine($"BulkOperation.InsertManyAsync Total:{_total} Start: {DateTime.Now:HH:mm:ss.fff}");

                // get temp collection
                var collection = GetCollection(Constants.CollectionProductTemp);

                // add temp
                await collection.InsertManyAsync(Products);

                // get product

                // clear
                Products.Clear();

                System.Console.WriteLine($"BulkOperation.InsertManyAsync Total:{_total} End: {DateTime.Now:HH:mm:ss.fff}");
            }
        }

        private static IMongoCollection<ProductTempEntity> GetCollection(string collectionName)
        {
            // server
            var client = new MongoClient(Constants.ConnectionString);

            // database
            var database = client.GetDatabase(Constants.Database);

            // table
            var collection = database.GetCollection<ProductTempEntity>(collectionName);

            // return collection
            return collection;
        }
    }
}
