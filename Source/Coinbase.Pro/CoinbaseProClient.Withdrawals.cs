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
      /// Get a list of withdrawals from the profile of the API key, in descending order by created time.
      /// </summary>
      Task<PagedResponse<Withdrawal>> ListWithdrawals(string profileId = null,
         int? limit = null, long? before = null, long? after = null,
         CancellationToken cancellationToken = default);

      /// <summary>
      /// Get information on a single withdrawal.
      /// </summary>
      Task<Withdrawal> GetWithdrawal(string transferId = null,
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

      Task<PagedResponse<Withdrawal>> IWithdrawalsEndpoint.ListWithdrawals(string profileId,
         int? limit, long? before, long? after,
         CancellationToken cancellationToken)
      {
         return this.TransfersEndpoint
            .WithClient(this)
            .SetQueryParam("type", "withdraw")
            .SetQueryParam("profile_id", profileId)
            .AsPagedRequest(limit, before, after)
            .GetPagedJsonAsync<Withdrawal>(cancellationToken);
      }

      Task<Withdrawal> IWithdrawalsEndpoint.GetWithdrawal(string transferId,
         CancellationToken cancellationToken)
      {
         return this.TransfersEndpoint
            .WithClient(this)
            .SetQueryParam("transfer_id", transferId)
            .GetJsonAsync<Withdrawal>(cancellationToken);
      }
   }
}
