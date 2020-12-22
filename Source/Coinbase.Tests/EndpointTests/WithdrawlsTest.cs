using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using VerifyNUnit;

namespace Coinbase.Tests.EndpointTests
{
   public class WithdrawlsTest : TestWithAuth
   {
      [Test]
      public async Task withdraw_via_paymethod()
      {
         server.RespondWithJsonTestFile();

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

         await Verifier.Verify(r);
      }

      [Test]
      public async Task withdraw_via_coinbase()
      {
         server.RespondWithJsonTestFile();

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

         await Verifier.Verify(r);
      }

      [Test]
      public async Task withdraw_via_crypto()
      {
         server.RespondWithJsonTestFile();

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

         await Verifier.Verify(r);
      }

      [Test]
      public async Task get_fees_estimate()
      {
         server.RespondWithJsonTestFile();

         var r = await client.Withdrawals.GetFeeEstimate("BTC", "fff");

         server.ShouldHaveCalledSomePath("/withdrawals/fee-estimate")
            .WithQueryParam("currency", "BTC")
            .WithQueryParam("crypto_address", "fff")
            .WithVerb(HttpMethod.Get);

         await Verifier.Verify(r);
      }

      [Test]
      public async Task list_withdrawals()
      {
         server.RespondWithJsonTestFile();
         var r = await client.Withdrawals.ListWithdrawals();

         server.ShouldHaveCalledSomePath("/transfers")
            .WithQueryParam("type", "withdraw")
            .WithVerb(HttpMethod.Get);

         r.Should().HaveCount(2);

         await Verifier.Verify(r);
      }

      [Test]
      public async Task get_single_withdrawal()
      {
         server.RespondWithJsonTestFile();
         var r = await client.Withdrawals.GetWithdrawal("fff");

         server.ShouldHaveCalledSomePath("/transfers/fff")
            .WithVerb(HttpMethod.Get);

         await Verifier.Verify(r);
      }

   }
}
