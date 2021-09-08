using System;
using System.Threading.Tasks;

namespace Mongo.Console
{
    public class Program
    {
        public static async Task Main()
        {
            try
            {
                System.Console.WriteLine($"Start: {DateTime.Now:HH:mm:ss.fff}");

                //await WriteOperation.WriteJson(10000);
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
