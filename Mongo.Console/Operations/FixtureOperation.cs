using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Mongo.Console.Entities;

namespace Mongo.Console.Operations
{
    public static class FixtureOperation
    {
        private static readonly List<ProductTempEntity> productEntities = new();
        private static readonly ReaderWriterLockSlim _lockSlim = new();

        public static IEnumerable<ProductTempEntity> GetSampleData(int count)
        {
            System.Console.WriteLine($"FixtureOperation.GetSampleData Start: {DateTime.Now:HH:mm:ss.fff}");

            var stepCount = count / 10;
            var totalChunks = (int)Math.Ceiling(count / (float)stepCount);
            Parallel.For(0, totalChunks, new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount * 2
            }, it =>
            {
                GetSampleData(it, stepCount);
            });

            //var fixture = new Fixture();
            //var list = fixture.CreateMany<ProductTempEntity>(count);

            System.Console.WriteLine($"FixtureOperation.GetSampleData End: {DateTime.Now:HH:mm:ss.fff}");

            return productEntities;
        }

        private static void GetSampleData(int index, int count)
        {
            System.Console.WriteLine($"FixtureOperation.GetSampleData-{index + 1} Start: {DateTime.Now:HH:mm:ss.fff}");

            var fixture = new Fixture();

            _lockSlim.EnterWriteLock();

            //var list = fixture.CreateMany<ProductTempEntity>(count);
            var list = fixture.Build<ProductTempEntity>().Without(p => p.Id).CreateMany(count);

            productEntities.AddRange(list);

            _lockSlim.ExitWriteLock();

            System.Console.WriteLine($"FixtureOperation.GetSampleData-{index + 1} End: {DateTime.Now:HH:mm:ss.fff}");
        }
    }
}