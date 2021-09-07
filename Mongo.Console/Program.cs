using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MongoDB.Driver;

namespace Mongo.Console
{
    public class Program
    {
        private const string filePath = @"C:\Temp\sample.json";
        private static List<Product> products = new();
        
        private static ReaderWriterLockSlim _lockSlim = new();

        static async Task Main(string[] args)
        {
            try
            {
                System.Console.WriteLine($"Start: {DateTime.Now:HH:mm:ss.fff}");

                await BulkOperation.BulkInsert();

                System.Console.WriteLine($"End: {DateTime.Now:HH:mm:ss.fff}");
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                throw;
            }

            System.Console.ReadKey();
        }

        private static async Task WriteJson(int count)
        {
            System.Console.WriteLine($"WriteJson Start: {DateTime.Now.ToLongTimeString()}");

            var data = GetSampleData(count);
            await using FileStream fileStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(fileStream, data);

            System.Console.WriteLine($"WriteJson End: {DateTime.Now.ToLongTimeString()}");
        }

        public static async Task LoadJson()
        {
            System.Console.WriteLine($"LoadJson Start: {DateTime.Now.ToLongTimeString()}");

            var json = await new StreamReader(filePath).ReadToEndAsync();
            products = JsonSerializer.Deserialize<List<Product>>(json);

            System.Console.WriteLine($"LoadJson Count: {products.Count}");
            System.Console.WriteLine($"LoadJson End: {DateTime.Now.ToLongTimeString()}");
        }

        private static IEnumerable<Product> GetSampleData(int count)
        {
            System.Console.WriteLine($"GetSampleData Start: {DateTime.Now.ToLongTimeString()}");

            using var fileStream = File.Create(filePath);
            using var streamWriter = new StreamWriter(fileStream);

            var stepCount = count / 1;
            var totalChunks = (int)Math.Ceiling(count / (float)stepCount);
            Parallel.For(0, totalChunks, new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            }, it =>
            {
                AddProductList(fileStream, streamWriter, it, stepCount);
            });

            System.Console.WriteLine($"Flush Start: {DateTime.Now.ToLongTimeString()}");
            fileStream.Flush();
            System.Console.WriteLine($"Flush End: {DateTime.Now.ToLongTimeString()}");

            System.Console.WriteLine($"GetSampleData End: {DateTime.Now.ToLongTimeString()}");

            return products;
        }

        private static void AddProductList(FileStream fileStream, StreamWriter streamWriter, int index, int count)
        {
            System.Console.WriteLine($"AddProductList Index:{index}, Start: {DateTime.Now.ToLongTimeString()}");
            var fixture = new Fixture();
            var list = fixture.CreateMany<Product>(count);

            _lockSlim.EnterWriteLock();

            var serialize = JsonSerializer.Serialize(list);
            streamWriter.WriteLine(serialize);
            //fileStream.Flush();

            _lockSlim.ExitWriteLock();

            System.Console.WriteLine($"AddProductList Index:{index}, End: {DateTime.Now.ToLongTimeString()}");
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
}
