using System.Threading.Tasks;
using Coinbase.Pro;
using Coinbase.Tests.EndpointTests;
using FluentAssertions;
using NUnit.Framework;
using VerifyNUnit;

namespace Coinbase.Tests.GitHubIssues
{
   public class Issue7 : Test
   {
      [Test]
      public async Task can_get_products2()
      {
         var client = new CoinbaseProClient();
         server.RespondWithJsonTestFile();

         var p = await client.MarketData.GetProductsAsync();

         p.Count.Should().NotBe(0);

         await Verifier.Verify(p);
      }
   }
}
