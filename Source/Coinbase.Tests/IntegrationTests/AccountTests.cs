using System.Threading.Tasks;
using Coinbase.Pro;
using Coinbase.Pro.Models;
using NUnit.Framework;

namespace Coinbase.Tests.IntegrationTests
{
   [Explicit]
   public class AccountTests : IntegrationTests
   {
      private CoinbaseProClient client;

      [SetUp]
      public void BeforeEachTest()
      {
         client = new CoinbaseProClient(new Config
            {
               UseTimeApi = true,
               ApiKey = this.secrets.ApiKey,
               Secret = this.secrets.ApiSecret,
               Passphrase = this.secrets.ApiPassphrase,
               ApiUrl = "https://api-public.sandbox.pro.coinbase.com"
            });
      }


      [Test]
      public async Task can_list_accounts()
      {
         var list = await this.client.Accounts.List();
         list.Dump();
      }

      [Test]
      public async Task can_get_account()
      {
         var a = await this.client.Accounts.GetAccount("046b8d74-34fc-419b-a19a-ae5e1c684684");
         a.Dump();
      }

      [Test]
      public async Task get_ledger()
      {
         var a = await this.client.Accounts.GetAccountHistory("9a2c74fd-9493-4660-85a9-a57c7f891990");

         a.Dump();
      }

      [Test]
      public async Task get_orders()
      {
         var o = await this.client.Orders.ListOrdersAsync();
         o.Dump();
      }

      [Test]
      public async Task create_order1()
      {
         var o = await this.client.Orders.PlaceStopOrderAsync(
            OrderSide.Buy,
            "ETC-USD", amount:100, AmountType.UseFunds, stopPrice: 999m);

         o.Dump();
      }

      [Test]
      public async Task create_order2()
      {
         var o = await this.client.Orders.PlaceStopOrderAsync(
            OrderSide.Buy,
            "ETC-USD", amount: 100, AmountType.UseFunds, stopPrice: 999m);

         o.Dump();
      }

      [Test]
      public async Task create_order3()
      {
         var o = await this.client.Orders.PlaceStopLimitOrderAsync(
            OrderSide.Buy,
            "ETC-USD", size: 1, stopPrice: 998m, limitPrice: 999m);

         o.Dump();
      }

      [Test]
      public async Task create_order4()
      {
         var o = await this.client.Orders.PlaceLimitOrderAsync(
            OrderSide.Buy, "ETC-USD", size: 100, limitPrice: 0.05m, timeInForce: TimeInForce.GoodTillCanceled);

         o.Dump();
      }

   }
}
