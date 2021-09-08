using System;
using System.Threading.Tasks;

namespace Mongo.Console
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                System.Console.WriteLine($"Start: {DateTime.Now:HH:mm:ss.fff}");

                //await WriteOperation.WriteJson(1100111);
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
    }
}
