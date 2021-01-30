using Coinbase.Pro;
using Coinbase.Pro.Models;
using Coinbase.Tests;

using System;
using System.Threading.Tasks;

internal class CoinbaseProClientSnippets
{
   private void CoinbaseProClient()
   {
      #region CoinbaseProClient

      var client = new CoinbaseProClient(new Config
         {
            ApiKey = "my-api-key",
            Secret = "my-api-secret",
            Passphrase = "my-api-passphrase",
            //Override the ApiUrl property to use Sandbox.
            //ApiUrl = "https://api-public.sandbox.pro.coinbase.com"
         });

      #endregion
   }

   private async Task CreateOrder(CoinbaseProClient client)
   {
      #region createOrder

      var order = await client.Orders.PlaceLimitOrderAsync(
         OrderSide.Buy, "ETH-USD", size: 2, limitPrice: 100m);

      order.Dump();

      #endregion
   }

   private async Task ErrorHandling(CoinbaseProClient client)
   {
      #region ErrorHandling

      try
      {
         var order = await client.Orders.PlaceLimitOrderAsync(
            OrderSide.Buy, "BTCX-USDX", size: 1, limitPrice: 5000m);
      }
      catch( Exception ex )
      {
         var errorMsg = await ex.GetErrorMessageAsync();
         Console.WriteLine(errorMsg);
      }
      //OUTPUT: "Product not found"

      #endregion
   }

   private async Task EnumerateOlder(CoinbaseProClient client)
   {
      #region EnumerateOlder
      //Get the initial page, items 16 through 20
      var trades = await client.MarketData.GetTradesAsync("ETC-USD", limit: 5);

      //Get the next batch of older trades after the current page.
      while( trades.After != null )
      {
         trades = await client.MarketData.GetTradesAsync("ETC-USD", limit: 5, after: trades.After);
      }
      #endregion
   }

   private async Task EnumerateNewer(CoinbaseProClient client)
   {
      #region EnumerateNewer
      //Get the initial page, items 16 through 20
      var trades = await client.MarketData.GetTradesAsync("ETC-USD", limit: 5);

      //Some time advances, trades execute.

      //Now, get the next batch of newer trades before the current page.
      while( trades.Before != null )
      {
         trades = await client.MarketData.GetTradesAsync("ETC-USD", limit: 5, before: trades.Before);
      }
      #endregion
   }
}
