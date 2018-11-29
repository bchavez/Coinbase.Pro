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
         server.RespondWith(Examples.DepositPaymentMethodFundsJson);

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
      }

      [Test]
      public async Task deposit_via_coinbase()
      {
         server.RespondWith(Examples.DepositCoinbaseFundsJson);

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
      }

   }
}
