using System.Net.Http;
using System.Threading.Tasks;
using Coinbase.Tests.EndpointTests;
using FluentAssertions;
using NUnit.Framework;

namespace Coinbase.Tests.GitHubIssues
{
   public class Issue4 : TestWithAuth
   {
      public const string AmountWithExponent = @"{
    ""id"": ""593533d2-ff31-46e0-b22e-ca754147a96a"",
    ""amount"": ""7e-7"",
    ""currency"": ""ETH"",
}";
      [Test]
      public async Task can_parse_exponent_decimal()
      {
         server.RespondWith(AmountWithExponent);

         var r = await client.Withdrawals.WithdrawFundsToCryptoAddressAsync("fff", 33, "ETH");

         server.ShouldHaveCalledSomePathAndQuery("/withdrawals/crypto")
            .WithVerb(HttpMethod.Post);

         r.Amount.Should().Be(7e-7m);
         r.Currency.Should().Be("ETH");
      }
   }
}
