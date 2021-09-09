using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mongo.Console.Entities;
using Mongo.Console.Extensions;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Mongo.Console.Operations
{
    public static class BulkOperation
    {
        private static int _total;
        private static readonly List<ProductTempEntity> ProductTemps = new();

        public static async Task BulkInsert()
        {
            await ReadData();
        }

        private static async Task ReadData()
        {
            using var streamReader = new StreamReader(Constants.Constants.FilePath);
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
            ProductTemps.Add(productTempEntity);

            // every 1000 records, add db
            if (ProductTemps.Count % 10000 == 0)
            {
                await UpsertAsync();
            }
        }

        private static async Task UpsertAsync()
        {
            if (ProductTemps.Any())
            {
                System.Console.WriteLine($"BulkOperation.InsertManyAsync Total:{_total} Start: {DateTime.Now:HH:mm:ss.fff}");

                // get collection
                var collectionProductTemp = GetCollection<ProductTempEntity>(Constants.Constants.CollectionProductTemp);
                var collectionProduct = GetCollection<ProductEntity>(Constants.Constants.CollectionProduct);

                // add product temp
                await collectionProductTemp.InsertManyAsync(ProductTemps);

                // compare product
                var newProducts = new List<ProductEntity>();
                foreach (var productTemp in ProductTemps)
                {
                    var product = await collectionProduct.Find(filter => filter.Code == productTemp.Code).FirstOrDefaultAsync();
                    if (product == null)
                    {
                        newProducts.Add(productTemp.Map());
                    }
                }

                // add product(s)
                if (newProducts.Any())
                {
                    await collectionProduct.InsertManyAsync(newProducts);
                }

                // clear
                ProductTemps.Clear();

                System.Console.WriteLine($"BulkOperation.InsertManyAsync Total:{_total} End: {DateTime.Now:HH:mm:ss.fff}");
            }
        }

        private static IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName)
        {
            // server
            var client = new MongoClient(Constants.Constants.ConnectionString);

            // database
            var database = client.GetDatabase(Constants.Constants.Database);

            // table
            var collection = database.GetCollection<TDocument>(collectionName);

            // return collection
            return collection;
        }
    }
}
