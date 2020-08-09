using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using Flurl;
using Flurl.Http;

namespace Coinbase.Pro
{
   public interface IDepositsEndpoint
   {
      /// <summary>
      /// Deposit funds from a payment method. See: https://docs.pro.coinbase.com/?r=1#payment-methods
      /// </summary>
      /// <param name="paymentMethodId">ID of the payment method</param>
      /// <param name="amount">The amount to deposit</param>
      /// <param name="currency">The type of currency</param>
      /// <param name="cancellationToken"></param>
      /// <returns></returns>
      Task<PaymentMethodDeposit> DepositFundsFromPaymentMethodAsync(
         string paymentMethodId, decimal amount, string currency,
         CancellationToken cancellationToken = default);

      /// <summary>
      /// Deposit funds from a coinbase account. You can move funds between your Coinbase accounts and your Coinbase Pro trading accounts within your daily limits. Moving funds between Coinbase and Coinbase Pro is instant and free. See the Coinbase Accounts section for retrieving your Coinbase accounts.
      /// </summary>
      /// <param name="coinbaseAccountId">ID of the coinbase account</param>
      /// <param name="amount">The amount to deposit</param>
      /// <param name="currency">The type of currency</param>
      /// <param name="cancellationToken"></param>
      /// <returns></returns>
      Task<CoinbaseDeposit> DepositFundsFromCoinbaseAccountAsync(
         string coinbaseAccountId, decimal amount, string currency,
         CancellationToken cancellationToken = default);

      /// <summary>
      /// Get information on a single deposit.
      /// </summary>
      Task<Deposit> GetDeposit(string transferId = null,
         CancellationToken cancellationToken = default);

      /// <summary>
      /// Get a list of deposits from the profile of the API key, in descending order by created time.
      /// </summary>
      /// <param name="profileId">Limit list of deposits to this profile_id. By default, it retrieves deposits across all of the user's profiles</param>
      /// <param name="limit">Truncate list to this many deposits, capped at 100. Default is 100.</param>
      /// <param name="before">If before is set, then it returns deposits created after the before timestamp, sorted by oldest creation date</param>
      /// <param name="after">If after is set, then it returns deposits created before the after timestamp, sorted by newest</param>
      Task<List<Deposit>> ListDeposits(string profileId = null,
         int? limit = null, DateTimeOffset? before = null, DateTimeOffset? after = null,
         CancellationToken cancellationToken = default);


      /// <summary>
      /// You can generate an address for crypto deposits. See the Coinbase Accounts section for information on how to retrieve your coinbase account ID.
      /// </summary>
      /// <returns></returns>
      Task<GeneratedDepositCryptoAddress> GenerateCryptoDepositAddress(
         string coinbaseAccountId,
         CancellationToken cancellationToken = default);

   }

   public partial class CoinbaseProClient : IDepositsEndpoint
   {
      public IDepositsEndpoint Deposits => this;

      protected internal Url DepositsEndpoint => this.Config.ApiUrl.AppendPathSegment("deposits");


      Task<PaymentMethodDeposit> IDepositsEndpoint.DepositFundsFromPaymentMethodAsync(
         string paymentMethodId, decimal amount, string currency,
         CancellationToken cancellationToken)
      {
         var d = new CreatePaymentMethodDeposit
            {
               Amount = amount,
               Currency = currency,
               PaymentMethodId = paymentMethodId
            };

         return this.DepositsEndpoint
            .WithClient(this)
            .AppendPathSegment("payment-method")
            .PostJsonAsync(d, cancellationToken)
            .ReceiveJson<PaymentMethodDeposit>();
      }

      Task<CoinbaseDeposit> IDepositsEndpoint.DepositFundsFromCoinbaseAccountAsync(
         string coinbaseAccountId, decimal amount, string currency,
         CancellationToken cancellationToken)
      {
         var d = new CreateCoinbaseDeposit
            {
               Amount = amount,
               Currency = currency,
               CoinbaseAccountId = coinbaseAccountId
            };

         return this.DepositsEndpoint
            .WithClient(this)
            .AppendPathSegment("coinbase-account")
            .PostJsonAsync(d, cancellationToken)
            .ReceiveJson<CoinbaseDeposit>();
      }

      Task<Deposit> IDepositsEndpoint.GetDeposit(string transferId,
         CancellationToken cancellationToken)
      {
         return this.TransfersEndpoint
            .WithClient(this)
            .AppendPathSegment(transferId)
            .GetJsonAsync<Deposit>(cancellationToken);
      }

      Task<List<Deposit>> IDepositsEndpoint.ListDeposits(string profileId,
         int? limit, DateTimeOffset? before, DateTimeOffset? after,
         CancellationToken cancellationToken)
      {
         return this.TransfersEndpoint
            .WithClient(this)
            .SetQueryParam("type", "deposit")
            .SetQueryParam("profile_id", profileId)
            .SetQueryParam("before", before)
            .SetQueryParam("after", after)
            .SetQueryParam("limit", limit)
            .GetJsonAsync<List<Deposit>>(cancellationToken);
      }

      Task<GeneratedDepositCryptoAddress> IDepositsEndpoint.GenerateCryptoDepositAddress(
         string coinbaseAccountId,
         CancellationToken cancellationToken)
      {
         return this.CoinbaseAccountsEndpoint
            .WithClient(this)
            .AppendPathSegments(coinbaseAccountId, "addresses")
            .PostJsonAsync(null, cancellationToken)
            .ReceiveJson<GeneratedDepositCryptoAddress>();
      }
   }
}
