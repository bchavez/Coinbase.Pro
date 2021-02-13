using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Coinbase.Pro;
using Coinbase.Pro.Models;
using FluentAssertions;
using Flurl;
using NUnit.Framework;
using VerifyNUnit;

//using Refit;
//using RichardSzalay.MockHttp;

namespace Coinbase.Tests.EndpointTests
{
   [TestFixture]
   public class MarketDataTests : Test
   {
      private CoinbaseProClient client;

      public override void BeforeEachTest()
      {
         base.BeforeEachTest();
         this.client = new CoinbaseProClient();
      }

      [Test]
      public async Task can_get_currencies()
      {
         server.RespondWithJsonTestFile();

         var r = await client.MarketData.GetCurrenciesAsync();

         server.ShouldHaveCalledSomePathAndQuery("/currencies")
            .WithVerb(HttpMethod.Get);

         await Verifier.Verify(r);
      }

      [Test]
      public async Task can_get_stats()
      {
         server.RespondWithJsonTestFile();

         var r = await client.MarketData.GetStatsAsync("BTC-USD");

         server.ShouldHaveCalledSomePathAndQuery("/products/BTC-USD/stats")
            .WithVerb(HttpMethod.Get);

         await Verifier.Verify(r);
      }

      [Test]
      public async Task can_get_candles()
      {
         //http.ExpectCall(HttpMethod.Get, "/products/BTC-USD/candles")
         //   .RespondJson(HttpStatusCode.OK, Examples.HistoricRatesJson);

         server.RespondWithJsonTestFile();
         var start = DateTime.Now.AddMinutes(-5);
         var end = DateTime.Now;


         var r = await client.MarketData.GetHistoricRatesAsync("BTC-USD", start, end, 60);

         server.ShouldHaveCalledSomePathAndQuery("/products/BTC-USD/candles?" +
                                                 $"start={Url.Encode(start.ToString("o"))}&" +
                                                 $"end={Url.Encode(end.ToString("o"))}&" +
                                                 "granularity=60")
            .WithVerb(HttpMethod.Get);

         await Verifier.Verify(r);
      }

      [Test]
      public async Task can_get_trades()
      {
         //http.ExpectCall(HttpMethod.Get, "/products/BTC-USD/trades")
         //   .RespondJson(HttpStatusCode.OK, Examples.TradesJson);
         server.RespondWithJsonTestFilePagedResult();

         var r = await client.MarketData.GetTradesAsync("BTC-USD");

         server.ShouldHaveCalledSomePathAndQuery("/products/BTC-USD/trades")
            .WithVerb(HttpMethod.Get);

         var t = r.Data.First();

         t.Price.Should().Be(4013.64000000m);
         t.Side.Should().Be(OrderSide.Sell);
         t.Size.Should().Be(0.00842855m);
         t.Time.Should().Be(DateTimeOffset.Parse("2018-11-26T07:23:53.243Z"));
         t.TradeId.Should().Be(54706228);

         r.Data.Count.Should().BeGreaterOrEqualTo(3);
         r.Before.Should().Be("54870014");
         r.After.Should().Be("54870113");

         await Verifier.Verify(r);
      }

      [Test]
      public async Task can_make_trades_paged()
      {
         server.RespondWithJsonTestFilePagedResult();

         var r = await client.MarketData.GetTradesAsync("BTC-USD", limit: 50, before: "27", after: "28");

         server.ShouldHaveCalledSomePathAndQuery("/products/BTC-USD/trades?" +
                                    "limit=50&" +
                                    "before=27&" +
                                    "after=28")
            .WithVerb(HttpMethod.Get);

         r.Before.Should().NotBeNull();
         r.After.Should().NotBeNull();

         await Verifier.Verify(r);
      }

      [Test]
      public async Task can_get_ticker()
      {
         server.RespondWithJsonTestFile();

         var r = await client.MarketData.GetTickerAsync("BTC-USD");

         server.ShouldHaveCalledSomePathAndQuery("/products/BTC-USD/ticker")
            .WithVerb(HttpMethod.Get);

         r.Ask.Should().Be(4027.95m);
         r.Bid.Should().Be(4027.82m);
         r.Price.Should().Be(4029.70000000m);
         r.Size.Should().Be(0.00100000m);
         r.Time.Should().Be(DateTimeOffset.Parse("2018-11-26T07:13:57.089000Z"));
         r.TradeId.Should().Be(54705877);
         r.Volume.Should().Be(32218.97638837m);

         await Verifier.Verify(r);
      }

      [Test]
      public async Task can_get_orderbook_l3()
      {
         server.RespondWithJsonTestFile();

         var r = await client.MarketData.GetOrderBookAsync("ETC-BTC", 3);

         server.ShouldHaveCalledSomePathAndQuery("/products/ETC-BTC/book?level=3")
            .WithVerb(HttpMethod.Get);

         r.Bids[0].Price.Should().Be(0.00117m);
         r.Bids[0].Size.Should().Be(5.5m);
         r.Bids[0].OrderId.Should().Be("88588a7f-5d24-4131-b270-394dd05a1353");

         await Verifier.Verify(r);
      }

      [Test]
      public async Task can_get_orderbook_l2()
      {
         server.RespondWithJsonTestFile();

         var r = await client.MarketData.GetOrderBookAsync("BTC-USD", 2);

         server.ShouldHaveCalledSomePathAndQuery("/products/BTC-USD/book?level=2")
            .WithVerb(HttpMethod.Get);

         r.Asks[0].Price.Should().Be(3931.11m);
         r.Asks[0].Size.Should().Be(0.96328664m);
         r.Asks[0].OrderCount.Should().Be(1);

         await Verifier.Verify(r);
      }

      [Test]
      public async Task can_get_orderbook()
      {
         server.RespondWithJsonTestFile();

         var r = await client.MarketData.GetOrderBookAsync("BTC-USD", 1);

         server.ShouldHaveCalledSomePathAndQuery("/products/BTC-USD/book?level=1")
            .WithVerb(HttpMethod.Get);

         r.Sequence.Should().Be(7416643032);
         r.Bids[0].Price.Should().Be(3967.55m);
         r.Bids[0].Size.Should().Be(57.6603824m);
         r.Bids[0].OrderCount.Should().Be(12);
         r.Bids.Length.Should().Be(1);

         r.Asks[0].Price.Should().Be(3967.56m);
         r.Asks[0].Size.Should().Be(0.001m);
         r.Asks[0].OrderCount.Should().Be(1);
         r.Asks.Length.Should().Be(1);

         await Verifier.Verify(r);
      }

      [Test]
      public async Task can_get_products()
      {
         server.RespondWithJsonTestFile();

         var r = await client.MarketData.GetProductsAsync();

         server.ShouldHaveCalledSomePathAndQuery("/products")
            .WithVerb(HttpMethod.Get);

         r.Count.Should().BeGreaterOrEqualTo(3);
         
         await Verifier.Verify(r);
      }

      [Test]
      public async Task can_get_single_product()
      {
         server.RespondWithJsonTestFile();

         var r = await client.MarketData.GetSingleProductAsync("BTC-USD");

         server.ShouldHaveCalledSomePathAndQuery("/products/BTC-USD")
            .WithVerb(HttpMethod.Get);

         await Verifier.Verify(r);
      }
   }


   public class AuthenticatedApiTest : Test
   {
      protected CoinbaseProClient client;

      [SetUp]
      public void BeforeEachTest()
      {
         client = new CoinbaseProClient(new Config{ ApiKey = "key", Secret = "secret", Passphrase = "satoshi", UseTimeApi = false});
      }

      protected override void EnsureEveryRequestHasCorrectHeaders()
      {
         base.EnsureEveryRequestHasCorrectHeaders();

         server.ShouldHaveMadeACall()
            .WithHeader(HeaderNames.AccessKey, "key")
            .WithHeader(HeaderNames.AccessSign)
            .WithHeader(HeaderNames.AccessPassphrase, "satoshi")
            .WithHeader(HeaderNames.AccessTimestamp);
      }
   }

   //public static class TestExtensions
   //{
   //   public static HttpTest RespondJson(this HttpTest source, HttpStatusCode statusCode, string content)
   //   {
   //      source.RespondWith(content, status: (int)statusCode,  )
   //      return source.Respond(statusCode, Constants.Json, content);
   //   }
   //}

   public static class Constants
   {
      public const string Json = "application/json";
   }
}
