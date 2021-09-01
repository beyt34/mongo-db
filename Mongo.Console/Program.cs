using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoFixture;
using MongoDB.Driver;

namespace Mongo.Console
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.WriteLine(DateTime.Now.ToLongTimeString());

            var collection = GetCollection();

            var id = Guid.NewGuid().ToString();
            var list = GetSampleData(100000);

            await UpsertAsync(collection, list);

            System.Console.WriteLine(DateTime.Now.ToLongTimeString());

            System.Console.ReadKey();
        }

        private static IEnumerable<TestEntity> GetSampleData(int count)
        {
            // timer
            var watch = new Stopwatch();
            watch.Start();

            var fixture = new Fixture();
            var list = fixture.CreateMany<TestEntity>(count);

            // stop timer
            watch.Stop();
            System.Console.WriteLine($"Fixture ItemCount:{count} row, ElapsedTime:{watch.Elapsed.Milliseconds} ms");

            return list;
        }

        private static IMongoCollection<TestEntity> GetCollection()
        {
            // timer
            var watch = new Stopwatch();
            watch.Start();

            // server
            var client = new MongoClient("mongodb://localhost:27017");

            // database
            var database = client.GetDatabase("test-db");

            // table
            var collection = database.GetCollection<TestEntity>("ItemMaster");

            // stop timer
            watch.Stop();
            System.Console.WriteLine($"Connection, ElapsedTime:{watch.Elapsed.Milliseconds} ms");

            return collection;
        }

        private static async Task UpsertAsync(IMongoCollection<TestEntity> collection, IEnumerable<TestEntity> list)
        {
            // timer
            var watch = new Stopwatch();
            watch.Start();

            // add-or-update
            await collection.InsertManyAsync(list);

            // stop timer
            watch.Stop();
            System.Console.WriteLine($"Save, ElapsedTime:{watch.Elapsed.Milliseconds} ms");
        }
    }

    public class TestEntity
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
