using System.Net.Http;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Coinbase.Tests.EndpointTests
{
   public class CoinbaseAccountsTest : TestWithAuth
   {
      [Test]
      public async Task coinbase_accounts()
      {
         server.RespondWithJsonTestFile();

         var r = await client.CoinbaseAccounts.GetAllAccountsAsync();

         server.ShouldHaveCalledSomePathAndQuery("/coinbase-accounts")
            .WithVerb(HttpMethod.Get);


         r.Count.Should().BeGreaterThan(0);

         var a0 = r[0];
         a0.Name.Should().Be("ETH Wallet");
         a0.IsWallet().Should().BeTrue();

         var a1 = r[1];
         a1.Name.Should().Be("USD Wallet");
         a1.IsFiat().Should().BeTrue();
         a1.WireDepositInformation.AccountNumber.Should().Be("0199003122");

         var a2 = r[2];
         a2.IsWallet().Should().BeTrue();
         a2.Name.Should().Be("BTC Wallet");

         //p.Name.Should().Be("Bank of America - eBan... ********7134");

         //p.Limits.InstantBuy[0].PeriodInDays.Should().Be(7);

         await Verify(r);
      }
   }
}
