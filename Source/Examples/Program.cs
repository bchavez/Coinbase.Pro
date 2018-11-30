using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Coinbase.Pro;

namespace Examples
{
   class Program
   {
      static async Task Main(string[] args)
      {
         Console.WriteLine("Hello World!");

         var client = new CoinbaseProClient(new Config
            {
               ApiKey = "foobar",
               Secret = "sauce",
               Passphrase = "bailouts are bad"
            });

         var data = await client.MarketData.GetStatsAsync("ETH-USD");

         Console.WriteLine($"ETH-USD Volume: {data.Volume}");

         
      }
   }
}
