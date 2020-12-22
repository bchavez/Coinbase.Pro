using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using VerifyNUnit;

namespace Coinbase.Tests.EndpointTests
{
   public class ConversionsTest : TestWithAuth
   {
      [Test]
      public async Task convert_usd_usdc()
      {
         server.RespondWithJsonTestFile();

         var r = await client.Conversion.ConvertAsync("USD", "USDC", 99);
         var expectedBody =
@"{
   ""from"": ""USD"",
   ""to"": ""USDC"",
   ""amount"": 99.0
}";

         server.ShouldHaveCalledSomePathAndQuery("/conversions")
            .WithSomeRequestBody(expectedBody)
            .WithVerb(HttpMethod.Post);

         r.Amount.Should().Be(10000.00m);
         r.From.Should().Be("USD");
         r.To.Should().Be("USDC");

         await Verifier.Verify(r);
      }
   }
}
