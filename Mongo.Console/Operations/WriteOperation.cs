using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mongo.Console.Operations
{
    public static class WriteOperation
    {
        public static async Task WriteJson(int count)
        {
            System.Console.WriteLine($"WriteOperation.WriteJson Start: {DateTime.Now:HH:mm:ss.fff}");

            var data = FixtureOperation.GetSampleData(count);
            await using FileStream fileStream = File.Create(Constants.Constants.FilePath);
            await JsonSerializer.SerializeAsync(fileStream, data);

            System.Console.WriteLine($"WriteOperation.WriteJson End: {DateTime.Now:HH:mm:ss.fff}");
        }
    }
}