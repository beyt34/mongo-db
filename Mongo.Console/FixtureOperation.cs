using System;
using System.Collections.Generic;
using AutoFixture;

namespace Mongo.Console
{
    public static class FixtureOperation
    {
        public static IEnumerable<ProductEntity> GetSampleData(int count)
        {
            System.Console.WriteLine($"FixtureOperation.GetSampleData Start: {DateTime.Now:HH:mm:ss.fff}");

            var fixture = new Fixture();
            var list = fixture.CreateMany<ProductEntity>(count);

            System.Console.WriteLine($"FixtureOperation.GetSampleData End: {DateTime.Now:HH:mm:ss.fff}");

            return list;
        }
    }
}