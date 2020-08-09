using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Coinbase.Tests.EndpointTests
{
   public class DepositsTest : TestWithAuth
   {
      [Test]
      public async Task deposit_via_paymethod()
      {
         server.RespondWithJsonTestFile();

         var r = await client.Deposits.DepositFundsFromPaymentMethodAsync("fff", 33, "USD");
         var expectedBody =
@"{
   ""amount"": 33.0,
   ""currency"": ""USD"",
   ""payment_method_id"": ""fff""
}";

         server.ShouldHaveCalledSomePathAndQuery("/deposits/payment-method")
            .WithSomeRequestBody(expectedBody)
            .WithVerb(HttpMethod.Post);

         r.Amount.Should().Be(10.00m);
         r.Currency.Should().Be("USD");

         await Verify(r);
      }

      [Test]
      public async Task deposit_via_coinbase()
      {
         server.RespondWithJsonTestFile();

         var r = await client.Deposits.DepositFundsFromCoinbaseAccountAsync("fff", 33, "USD");

         var expectedBody =
@"{
   ""amount"": 33.0,
   ""currency"": ""USD"",
   ""coinbase_account_id"": ""fff""
}";

         server.ShouldHaveCalledSomePathAndQuery("/deposits/coinbase-account")
            .WithSomeRequestBody(expectedBody)
            .WithVerb(HttpMethod.Post);

         r.Amount.Should().Be(10.00m);
         r.Currency.Should().Be("BTC");

         await Verify(r);
      }

      [Test]
      public async Task can_get_deposit()
      {
         server.RespondWithJsonTestFile();

         var r = await client.Deposits.GetDeposit("fefe");

         server.ShouldHaveCalled("/transfers/fefe")
            .WithVerb(HttpMethod.Get);

         await Verify(r);
      }


      [Test]
      public async Task can_list_deposits()
      {
         server.RespondWithJsonTestFile();

         var r = await client.Deposits.ListDeposits();

         server.ShouldHaveCalled("/transfers")
            .WithQueryParamValue("type", "deposit")
            .WithVerb(HttpMethod.Get);

         await Verify(r);
      }


      [Test]
      public async Task generate_crypto_address()
      {
         server.RespondWithJsonTestFile();

         var r = await client.Deposits.GenerateCryptoDepositAddress("ffff");

         server.ShouldHaveCalled("/coinbase-accounts/ffff/addresses")
            .WithVerb(HttpMethod.Post);

         await Verify(r);
      }
      
   }
}
