using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Coinbase.Tests.EndpointTests
{
   public class OrdersTest : TestWithAuth
   {
      [Test]
      public async Task list()
      {
         server.RespondWith(Examples.OrdersListJson);

         var r = await client.Orders.ListOrdersAsync();

         r.Dump();

         var o = r.Data.Last();
         o.Side.Should().Be(OrderSide.Buy);
         o.Type.Should().Be(OrderType.Market);
         o.TimeInForce.Should().Be(TimeInForce.GoodTillCanceled);
         
         server.ShouldHaveExactCall("https://api.pro.coinbase.com/orders?status=all")
            .WithVerb(HttpMethod.Get);
      }

      [Test]
      public async Task list_page()
      {
         var r = await client.Orders.ListOrdersAsync("open, pending", "ETH-USD", 20, 30, 40);

         server.ShouldHaveExactCall("https://api.pro.coinbase.com/orders?" +
                                    "status=open&" +
                                    "status=pending&" +
                                    "product_id=ETH-USD&" +
                                    "limit=20&" +
                                    "before=30&" +
                                    "after=40")
            .WithVerb(HttpMethod.Get);
      }

      [Test]
      public async Task can_get_order()
      {
         server.RespondWith(Examples.Order1Json);

         var r = await client.Orders.GetOrderAsync("fff");

         r.Dump();

         r.Id.Should().Be("e0c163ab-5823-4972-b9fb-9e788687a53b");
         r.ProductId.Should().Be("ETC-USD");

         server.ShouldHaveExactCall("https://api.pro.coinbase.com/orders/fff")
            .WithVerb(HttpMethod.Get);
      }

      [Test]
      public async Task cancel_all_orders()
      {
         server.RespondWith(Examples.CancellAllOrdersJson);

         var r = await client.Orders.CancelAllOrdersAsync();

         r.Count.Should().BeGreaterThan(1);

         server.ShouldHaveExactCall("https://api.pro.coinbase.com/orders")
            .WithVerb(HttpMethod.Delete);
      }

      [Test]
      public async Task cancel_all_orders_product()
      {
         server.RespondWith(Examples.CancellAllOrdersJson);
         var r = await client.Orders.CancelAllOrdersAsync("BTC-USD");

         r.Count.Should().BeGreaterThan(1);

         server.ShouldHaveExactCall("https://api.pro.coinbase.com/orders?" +
                                    "product_id=BTC-USD")
            .WithVerb(HttpMethod.Delete);
      }
   }
}
