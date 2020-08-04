using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using Flurl;
using Flurl.Http;

namespace Coinbase.Pro
{
   public interface IWithdrawalsEndpoint
   {
      /// <summary>
      /// Withdraw funds to a payment method.
      /// </summary>
      Task<PaymentMethodWithdraw> WithdrawFundsToPaymentMethodAsync(
         string paymentMethodId, decimal amount, string currency,
         CancellationToken cancellationToken = default);

      /// <summary>
      /// Withdraw funds to a coinbase account. You can move funds between your Coinbase
      /// accounts and your Coinbase Pro trading accounts within your daily limits.
      /// Moving funds between Coinbase and Coinbase Pro is instant and free.
      /// See the Coinbase Accounts section for retrieving your Coinbase accounts
      /// </summary>
      Task<CoinbaseWithdraw> WithdrawFundsToCoinbaseAsync(
         string coinbaseAccountId, decimal amount, string currency,
         CancellationToken cancellationToken = default);

      /// <summary>
      /// Withdraws funds to a crypto address.
      /// </summary>
      Task<CryptoWithdraw> WithdrawFundsToCryptoAddressAsync(
         string cryptoAddress, decimal amount, string currency,
         CancellationToken cancellationToken = default);

      /// <summary>
      /// Lists withdrawals history.
      /// </summary>
      Task<List<Withdrawal>> ListWithdrawals(
         string type, DateTime? before, DateTime? after,
         CancellationToken cancellationToken = default);
   }

   public partial class CoinbaseProClient : IWithdrawalsEndpoint
   {
      public IWithdrawalsEndpoint Withdrawals => this;

      protected internal Url WithdrawalsEndpoint => this.Config.ApiUrl.AppendPathSegment("withdrawals");
      protected internal Url TransfersEndpoint => this.Config.ApiUrl.AppendPathSegment("transfers");


      Task<PaymentMethodWithdraw> IWithdrawalsEndpoint.WithdrawFundsToPaymentMethodAsync(string paymentMethodId, decimal amount, string currency,
         CancellationToken cancellationToken)
      {
         var d = new CreatePaymentMethodWithdraw
            {
               Amount = amount,
               Currency = currency,
               PaymentMethodId = paymentMethodId
            };

         return this.WithdrawalsEndpoint
            .WithClient(this)
            .AppendPathSegment("payment-method")
            .PostJsonAsync(d, cancellationToken)
            .ReceiveJson<PaymentMethodWithdraw>();
      }

      Task<CoinbaseWithdraw> IWithdrawalsEndpoint.WithdrawFundsToCoinbaseAsync(string coinbaseAccountId, decimal amount, string currency,
         CancellationToken cancellationToken)
      {
         var d = new CreateCoinbaseWithdraw
            {
               Amount = amount,
               Currency = currency,
               CoinbaseAccountId = coinbaseAccountId
            };

         return this.WithdrawalsEndpoint
            .WithClient(this)
            .AppendPathSegment("coinbase-account")
            .PostJsonAsync(d, cancellationToken)
            .ReceiveJson<CoinbaseWithdraw>();
      }

      Task<CryptoWithdraw> IWithdrawalsEndpoint.WithdrawFundsToCryptoAddressAsync(string cryptoAddress, decimal amount, string currency,
         CancellationToken cancellationToken)
      {
         var d = new CreateCryptAddressWithdrawl
            {
               Amount = amount,
               Currency = currency,
               CryptoAddress = cryptoAddress
            };

         return this.WithdrawalsEndpoint
            .WithClient(this)
            .AppendPathSegment("crypto")
            .PostJsonAsync(d, cancellationToken)
            .ReceiveJson<CryptoWithdraw>();
      }

      public Task<List<Withdrawal>> ListWithdrawals(string type, DateTime? before, DateTime? after, CancellationToken cancellationToken = default)
      {
         return this.TransfersEndpoint
            .WithClient(this)
            .SetQueryParam("type", type)
            .SetQueryParam("before", before)
            .SetQueryParam("after", after)
            .GetJsonAsync<List<Withdrawal>>(cancellationToken);
      }
   }
}
