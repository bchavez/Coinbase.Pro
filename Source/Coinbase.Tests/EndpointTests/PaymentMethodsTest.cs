using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Coinbase.Tests.EndpointTests
{
   public class PaymentMethodsTest : TestWithAuth
   {
      [Test]
      public async Task payment_methods()
      {
         server.RespondWithJsonTestFile();

         var r = await client.PaymentMethods.GetAllPaymentMethodsAsync();

         server.ShouldHaveCalledSomePathAndQuery("/payment-methods")
            .WithVerb(HttpMethod.Get);


         r.Count.Should().BeGreaterThan(0);

         var p = r.First();

         p.Name.Should().Be("Bank of America - eBan... ********7134");

         p.Limits.InstantBuy[0].PeriodInDays.Should().Be(7);

         await Verify(r);
      }
   }
}
