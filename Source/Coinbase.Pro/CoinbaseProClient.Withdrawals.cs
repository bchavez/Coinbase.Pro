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
         string cryptoAddress, decimal amount, string currency, string destinationTag = null, bool? noDestinationTag = null, bool? addNetworkFeeToTotal = null,
         CancellationToken cancellationToken = default);

      /// <summary>
      /// Gets the network fee estimate when sending to the given address.
      /// </summary>
      /// <param name="currency">The type of currency</param>
      /// <param name="cryptoAddress">A crypto address of the recipient</param>
      Task<FeeEstimate> GetFeeEstimate(string currency, string cryptoAddress,
         CancellationToken cancellationToken = default);

      /// <summary>
      /// Get information on a single withdrawal.
      /// </summary>
      Task<Withdrawal> GetWithdrawal(string transferId = null,
         CancellationToken cancellationToken = default);

      /// <summary>
      /// Get a list of deposits from the profile of the API key, in descending order by created time.
      /// </summary>
      /// <param name="profileId">Limit list of deposits to this profile_id. By default, it retrieves deposits across all of the user's profiles</param>
      /// <param name="limit">Truncate list to this many deposits, capped at 100. Default is 100.</param>
      /// <param name="before">If before is set, then it returns deposits created after the before timestamp, sorted by oldest creation date</param>
      /// <param name="after">If after is set, then it returns deposits created before the after timestamp, sorted by newest</param>
      Task<List<Withdrawal>> ListWithdrawals(string profileId = null,
         int? limit = null, DateTimeOffset? before = null, DateTimeOffset? after = null,
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

      Task<CryptoWithdraw> IWithdrawalsEndpoint.WithdrawFundsToCryptoAddressAsync(string cryptoAddress, decimal amount, string currency, string destinationTag, bool? noDestinationTag, bool? addNetworkFeeToTotal,
         CancellationToken cancellationToken)
      {
         var d = new CreateCryptAddressWithdrawl
            {
               Amount = amount,
               Currency = currency,
               CryptoAddress = cryptoAddress,
               DestinationTag = destinationTag,
               NoDestinationTag = noDestinationTag,
               AddNetworkFeeToTotal = addNetworkFeeToTotal
            };

         return this.WithdrawalsEndpoint
            .WithClient(this)
            .AppendPathSegment("crypto")
            .PostJsonAsync(d, cancellationToken)
            .ReceiveJson<CryptoWithdraw>();
      }

      Task<FeeEstimate> IWithdrawalsEndpoint.GetFeeEstimate(string currency, string cryptoAddress,
         CancellationToken cancellationToken)
      {
         return this.WithdrawalsEndpoint
            .WithClient(this)
            .AppendPathSegment("fee-estimate")
            .SetQueryParam("currency", currency)
            .SetQueryParam("crypto_address", cryptoAddress)
            .GetJsonAsync<FeeEstimate>(cancellationToken);
      }

      Task<Withdrawal> IWithdrawalsEndpoint.GetWithdrawal(string transferId,
         CancellationToken cancellationToken)
      {
         return this.TransfersEndpoint
            .WithClient(this)
            .AppendPathSegment(transferId)
            .GetJsonAsync<Withdrawal>(cancellationToken);
      }

      Task<List<Withdrawal>> IWithdrawalsEndpoint.ListWithdrawals(string profileId,
         int? limit, DateTimeOffset? before, DateTimeOffset? after,
         CancellationToken cancellationToken)
      {
         return this.TransfersEndpoint
            .WithClient(this)
            .SetQueryParam("type", "withdraw")
            .SetQueryParam("profile_id", profileId)
            .SetQueryParam("before", before)
            .SetQueryParam("after", after)
            .SetQueryParam("limit", limit)
            .GetJsonAsync<List<Withdrawal>>(cancellationToken);
      }
   }
}
