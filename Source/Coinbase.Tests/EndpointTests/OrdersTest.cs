using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using FluentAssertions;
using NUnit.Framework;
using VerifyNUnit;

namespace Coinbase.Tests.EndpointTests
{
   public class OrdersTest : TestWithAuth
   {
      [Test]
      public async Task get_all_orders()
      {
         server.RespondWithJsonTestFile();

         var r = await client.Orders.GetAllOrdersAsync();

         var o = r.Data.Last();
         o.Side.Should().Be(OrderSide.Buy);
         o.Type.Should().Be(OrderType.Market);
         o.TimeInForce.Should().Be(TimeInForce.GoodTillCanceled);

         server.ShouldHaveCalledSomePathAndQuery("/orders?status=all")
            .WithVerb(HttpMethod.Get);

         await Verifier.Verify(r);
      }

      [Test]
      public async Task get_paged_order_list()
      {
         var r = await client.Orders.GetAllOrdersAsync("open, pending", "ETH-USD", 20, "30", "40");

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
         server.RespondWithJsonTestFile();

         var r = await client.Orders.GetOrderAsync("fff");

         r.Id.Should().Be("e0c163ab-5823-4972-b9fb-9e788687a53b");
         r.ProductId.Should().Be("ETC-USD");

         server.ShouldHaveCalledSomePathAndQuery("/orders/fff")
            .WithVerb(HttpMethod.Get);

         await Verifier.Verify(r);
      }

      [Test]
      public async Task cancel_all_orders()
      {
         server.RespondWithJsonTestFile();

         var r = await client.Orders.CancelAllOrdersAsync();

         r.Count.Should().BeGreaterThan(1);

         server.ShouldHaveCalledSomePathAndQuery("/orders")
            .WithVerb(HttpMethod.Delete);

         await Verifier.Verify(r);
      }

      [Test]
      public async Task cancel_all_orders_product()
      {
         server.RespondWithJsonTestFile();
         var r = await client.Orders.CancelAllOrdersAsync("BTC-USD");

         r.Count.Should().BeGreaterThan(1);

         server.ShouldHaveCalledSomePathAndQuery("/orders?" +
                                    "product_id=BTC-USD")
            .WithVerb(HttpMethod.Delete);

         await Verifier.Verify(r);
      }

      [Test]
      public async Task cancel_order_by_id()
      {
         server.RespondWithJsonTestFile();

         var r = await client.Orders.CancelOrderById("ooo");

         r.Count.Should().Be(1);

         server.ShouldHaveCalledSomePathAndQuery("/orders/ooo")
            .WithVerb(HttpMethod.Delete);

         await Verifier.Verify(r);
      }
   }
}
