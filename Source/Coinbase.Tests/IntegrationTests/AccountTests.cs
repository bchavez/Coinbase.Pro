using System;
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
         var list = await this.client.Accounts.GetAllAccountsAsync();
         list.Dump();
      }

      [Test]
      public async Task can_get_account()
      {
         var a = await this.client.Accounts.GetAccountAsync("046b8d74-34fc-419b-a19a-ae5e1c684684");
         a.Dump();
      }

      [Test]
      public async Task get_ledger()
      {
         var a = await this.client.Accounts.GetAccountHistoryAsync("9a2c74fd-9493-4660-85a9-a57c7f891990");

         a.Dump();
      }

      [Test]
      public async Task get_orders()
      {
         var o = await this.client.Orders.GetAllOrdersAsync();
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

      [Test]
      public async Task list_fills1()
      {
         var f = await client.Fills.ListFillsByProductIdAsync("ETC-USD");

         f.Dump();
      }

      [Test]
      public async Task can_create_report()
      {
         var date = DateTimeOffset.Parse("1/1/2018");
         var r = await client.Reports.CreateFillReportAsync(date, date.AddYears(1), "ETC-USD");
         r.Dump();
      }

      [Test]
      public async Task get_report_status()
      {
         var r = await client.Reports.GetReportStatusAsync("2afc0924-d5f0-4b9f-b540-caee97e39fd9");

         r.Dump();
      }

   }
}
