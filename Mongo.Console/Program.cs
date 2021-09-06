using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using AutoFixture;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Mongo.Console
{
    public class Program
    {
        private const string filePath = @"C:\Temp\sample.json";
        private static List<Product> products = new();
        private static List<WriteModel<Product>> productWriteModels = new();

        static async Task Main(string[] args)
        {
            try
            {
                System.Console.WriteLine($"Start: {DateTime.Now.ToLongTimeString()}");

                //await WriteJson(1000000);

                //var collection = GetCollection();
                //var list = GetSampleData(100000);
                //await UpsertAsync(collection, list);

                //LoadJson();
                //var collection = GetCollection();
                //await UpsertAsync(collection, products);

                await Bulk();

                System.Console.WriteLine($"End: {DateTime.Now.ToLongTimeString()}");
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                throw;
            }

            System.Console.ReadKey();
        }

        private static async Task Bulk()
        {
            using var streamReader = new StreamReader(filePath);
            using var reader = new JsonTextReader(streamReader);
            var serializer = new Newtonsoft.Json.JsonSerializer();

            while (reader.Read())
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
            //System.Console.WriteLine($"productCode:{productData.Code}");
            products.Add(productData);
            if (products.Count % 10000 == 0)
            {
                var collection = GetCollection();
                await UpsertAsync(collection, products);
                products.Clear();
            }
        }

        private static async Task WriteJson(int count)
        {
            System.Console.WriteLine($"WriteJson Start: {DateTime.Now.ToLongTimeString()}");

            var data = GetSampleData(count);
            await using FileStream fileStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(fileStream, data);

            System.Console.WriteLine($"WriteJson End: {DateTime.Now.ToLongTimeString()}");
        }

        public static void LoadJson()
        {
            System.Console.WriteLine($"LoadJson Start: {DateTime.Now.ToLongTimeString()}");

            using var streamReader = new StreamReader(filePath);
            var jsonString = streamReader.ReadToEnd();
            products = JsonSerializer.Deserialize<List<Product>>(jsonString);

            System.Console.WriteLine($"LoadJson Count: {products.Count}");
            System.Console.WriteLine($"LoadJson End: {DateTime.Now.ToLongTimeString()}");
        }

        private static IEnumerable<Product> GetSampleData(int count)
        {
            System.Console.WriteLine($"GetSampleData Start: {DateTime.Now.ToLongTimeString()}");

            var fixture = new Fixture();
            var list = fixture.CreateMany<Product>(count);

            System.Console.WriteLine($"GetSampleData End: {DateTime.Now.ToLongTimeString()}");

            return list;
        }

        private static IMongoCollection<Product> GetCollection()
        {
            System.Console.WriteLine($"GetCollection Start: {DateTime.Now.ToLongTimeString()}");

            // server
            var client = new MongoClient("mongodb://localhost:27017");

            // database
            var database = client.GetDatabase("test-db");

            // table
            var collection = database.GetCollection<Product>("Product");

            System.Console.WriteLine($"GetCollection End: {DateTime.Now.ToLongTimeString()}");

            return collection;
        }

        private static async Task UpsertAsync(IMongoCollection<Product> collection, IEnumerable<Product> list)
        {
            System.Console.WriteLine($"InsertManyAsync Start: {DateTime.Now.ToLongTimeString()}");

            // add-or-update
            await collection.InsertManyAsync(list);

            System.Console.WriteLine($"InsertManyAsync End: {DateTime.Now.ToLongTimeString()}");
        }
    }

    public class Product
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string AttributeA { get; set; }
        public string AttributeB { get; set; }
        public string AttributeC { get; set; }
        public string AttributeD { get; set; }
        public string AttributeE { get; set; }
        public string AttributeF { get; set; }
        public string AttributeG { get; set; }
        public string AttributeH { get; set; }
        public string AttributeI { get; set; }
        public string AttributeJ { get; set; }
        public string AttributeK { get; set; }
        public string AttributeL { get; set; }
        public string AttributeM { get; set; }
        public string AttributeN { get; set; }
        public string AttributeO { get; set; }
        public string AttributeP { get; set; }
        public string AttributeQ { get; set; }
        public string AttributeR { get; set; }
        public string AttributeS { get; set; }
        public string AttributeT { get; set; }
        public string AttributeU { get; set; }
        public string AttributeV { get; set; }
        public string AttributeW { get; set; }
        public string AttributeX { get; set; }
        public string AttributeY { get; set; }
        public string AttributeZ { get; set; }
    }
}
