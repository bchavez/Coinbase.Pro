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
      public async Task get_all_orders()
      {
         server.RespondWith(Examples.OrdersListJson);

         var r = await client.Orders.GetAllOrdersAsync();

         r.Dump();

         var o = r.Data.Last();
         o.Side.Should().Be(OrderSide.Buy);
         o.Type.Should().Be(OrderType.Market);
         o.TimeInForce.Should().Be(TimeInForce.GoodTillCanceled);
         
         server.ShouldHaveCalledSomePathAndQuery("/orders?status=all")
            .WithVerb(HttpMethod.Get);
      }

      [Test]
      public async Task get_paged_order_list()
      {
         var r = await client.Orders.GetAllOrdersAsync("open, pending", "ETH-USD", 20, 30, 40);

         server.ShouldHaveCalledSomePathAndQuery("/orders?" +
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

         server.ShouldHaveCalledSomePathAndQuery("/orders/fff")
            .WithVerb(HttpMethod.Get);
      }

      [Test]
      public async Task cancel_all_orders()
      {
         server.RespondWith(Examples.CancellAllOrdersJson);

         var r = await client.Orders.CancelAllOrdersAsync();

         r.Count.Should().BeGreaterThan(1);

         server.ShouldHaveCalledSomePathAndQuery("/orders")
            .WithVerb(HttpMethod.Delete);
      }

      [Test]
      public async Task cancel_all_orders_product()
      {
         server.RespondWith(Examples.CancellAllOrdersJson);
         var r = await client.Orders.CancelAllOrdersAsync("BTC-USD");

         r.Count.Should().BeGreaterThan(1);

         server.ShouldHaveCalledSomePathAndQuery("/orders?" +
                                    "product_id=BTC-USD")
            .WithVerb(HttpMethod.Delete);
      }
   }
}
