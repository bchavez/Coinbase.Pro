using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Coinbase.Tests.EndpointTests
{
   public class FillsTest : TestWithAuth
   {
      [Test]
      public async Task getall_by_product()
      {
         server.RespondWithJsonTestFilePagedResult(11, 22);

         var r = await client.Fills.GetFillsByProductIdAsync("ETC-USD");

         server.ShouldHaveCalledSomePathAndQuery("/fills?product_id=ETC-USD")
            .WithVerb(HttpMethod.Get);

         r.Before.Should().Be(11);
         r.After.Should().Be(22);

         var f = r.Data.First();
         f.UserId.Should().StartWith("fff");
         f.TradeId.Should().Be(59);
         f.Price.Should().Be(50m);

         await Verify(r);
      }

      [Test]
      public async Task getall_by_order()
      {
         server.RespondWithJsonTestFilePagedResult(11, 22);

         var r = await client.Fills.GetFillsByOrderIdAsync("333");

         server.ShouldHaveCalledSomePathAndQuery("/fills?order_id=333")
            .WithVerb(HttpMethod.Get);

         r.Before.Should().Be(11);
         r.After.Should().Be(22);

         var f = r.Data.First();
         f.UserId.Should().StartWith("fff");
         f.TradeId.Should().Be(59);
         f.Price.Should().Be(50m);

         await Verify(r);
      }
   }
}
