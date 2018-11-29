using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Coinbase.Tests.EndpointTests
{
   public class WithdrawlsTest : TestWithAuth
   {
      [Test]
      public async Task withdraw_via_paymethod()
      {
         server.RespondWith(Examples.WithdrawPaymentMethodFundsJson);

         var r = await client.Withdrawals.WithdrawFundsToPaymentMethodAsync("fff", 33, "USD");
         var expectedBody =
@"{
   ""amount"": 33.0,
   ""currency"": ""USD"",
   ""payment_method_id"": ""fff""
}";

         server.ShouldHaveCalledSomePathAndQuery("/withdrawals/payment-method")
            .WithSomeRequestBody(expectedBody)
            .WithVerb(HttpMethod.Post);

         r.Amount.Should().Be(10.00m);
         r.Currency.Should().Be("USD");
      }

      [Test]
      public async Task withdraw_via_coinbase()
      {
         server.RespondWith(Examples.WithdrawCoinbaseFundsJson);

         var r = await client.Withdrawals.WithdrawFundsToCoinbaseAsync("fff", 33, "BTC");

         var expectedBody =
@"{
   ""amount"": 33.0,
   ""currency"": ""BTC"",
   ""coinbase_account_id"": ""fff""
}";

         server.ShouldHaveCalledSomePathAndQuery("/withdrawals/coinbase-account")
            .WithSomeRequestBody(expectedBody)
            .WithVerb(HttpMethod.Post);

         r.Amount.Should().Be(10.00m);
         r.Currency.Should().Be("BTC");
      }

      [Test]
      public async Task withdraw_via_crypto()
      {
         server.RespondWith(Examples.WithdrawCryptoFundsJson);

         var r = await client.Withdrawals.WithdrawFundsToCryptoAddressAsync("fff", 33, "ETH");

         var expectedBody =
            @"{
   ""amount"": 33.0,
   ""currency"": ""ETH"",
   ""crypto_address"": ""fff""
}";

         server.ShouldHaveCalledSomePathAndQuery("/withdrawals/crypto")
            .WithSomeRequestBody(expectedBody)
            .WithVerb(HttpMethod.Post);

         r.Amount.Should().Be(10.00m);
         r.Currency.Should().Be("ETH");

      }

   }
}
