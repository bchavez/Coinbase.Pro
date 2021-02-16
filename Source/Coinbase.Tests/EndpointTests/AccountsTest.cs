﻿using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using VerifyNUnit;

namespace Coinbase.Tests.EndpointTests
{
   public class AccountsTest : TestWithAuth
   {
      [Test]
      public async Task get_all_accounts()
      {
         server.RespondWithJsonTestFile();

         var r = await client.Accounts.GetAllAccountsAsync();

         r.Dump();
         var a = r.First();
         a.Currency.Should().Be("BTC");
         a.Id.Should().Be("71452118-efc7-4cc4-8780-a5e22d4baa53");

         server.ShouldHaveCalledSomePathAndQuery("/accounts");

         await Verifier.Verify(r);
      }

      [Test]
      public async Task can_get_account()
      {
         server.RespondWithJsonTestFile();

         var r = await client.Accounts.GetAccountAsync("fff");

         r.Dump();

         r.Currency.Should().Be("BTC");
         r.Id.Should().Be("71452118-efc7-4cc4-8780-a5e22d4baa53");

         server.ShouldHaveCalledSomePathAndQuery("/accounts/fff");

         await Verifier.Verify(r);
      }

      [Test]
      public async Task get_history()
      {
         server.RespondWithJsonTestFile();

         var r = await client.Accounts.GetAccountHistoryAsync("fff");

         server.ShouldHaveCalledSomePathAndQuery("/accounts/fff/ledger");

         await Verifier.Verify(r);
      }

      [Test]
      public async Task get_hold()
      {
         server.RespondWithJsonTestFile();

         var r = await client.Accounts.GetAccountHoldAsync("fff");

         r.Dump();

         var h = r.Data.First();
         h.AccountId.Should().Be("e0b3f39a-183d-453e-b754-0c13e5bab0b3");

         server.ShouldHaveCalledSomePathAndQuery("/accounts/fff/holds");

         await Verifier.Verify(r);
      }
   }
}
