using System.Collections.Generic;
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
      /// <returns></returns>
      Task<PaymentMethodDeposit> DepositFundsFromPaymentMethodAsync(string paymentMethodId, decimal amount, string currency);

      /// <summary>
      /// Deposit funds from a coinbase account. You can move funds between your Coinbase accounts and your Coinbase Pro trading accounts within your daily limits. Moving funds between Coinbase and Coinbase Pro is instant and free. See the Coinbase Accounts section for retrieving your Coinbase accounts.
      /// </summary>
      /// <param name="coinbaseAccountId">ID of the coinbase account</param>
      /// <param name="amount">The amount to deposit</param>
      /// <param name="currency">The type of currency</param>
      /// <returns></returns>
      Task<CoinbaseDeposit> DepositFundsFromCoinbaseAccountAsync(string coinbaseAccountId, decimal amount, string currency);
   }

   public partial class CoinbaseProClient : IDepositsEndpoint
   {
      public IDepositsEndpoint Deposits => this;

      protected internal Url DepositsEndpoint => this.Config.ApiUrl.AppendPathSegment("deposits");


      Task<PaymentMethodDeposit> IDepositsEndpoint.DepositFundsFromPaymentMethodAsync(string paymentMethodId, decimal amount, string currency)
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
            .PostJsonAsync(d)
            .ReceiveJson<PaymentMethodDeposit>();
      }

      Task<CoinbaseDeposit> IDepositsEndpoint.DepositFundsFromCoinbaseAccountAsync(string coinbaseAccountId, decimal amount, string currency)
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
            .PostJsonAsync(d)
            .ReceiveJson<CoinbaseDeposit>();
      }
   }
}
