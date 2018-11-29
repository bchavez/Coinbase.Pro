using System.Collections.Generic;
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
      /// <param name="paymentMethodId"></param>
      /// <param name="amount"></param>
      /// <param name="currency"></param>
      Task<PaymentMethodWithdraw> WithdrawFundsToPaymentMethodAsync(string paymentMethodId, decimal amount, string currency);    
      Task<CoinbaseWithdraw> WithdrawFundsToCoinbaseAsync(string coinbaseAccountId, decimal amount, string currency);
      Task<CryptoWithdraw> WithdrawFundsToCryptoAddressAsync(string cryptoAddress, decimal amount, string currency);
   }

   public partial class CoinbaseProClient : IWithdrawalsEndpoint
   {
      public IWithdrawalsEndpoint Withdrawals => this;

      protected internal Url WithdrawalsEndpoint => this.Config.ApiUrl.AppendPathSegment("withdrawals");


      Task<PaymentMethodWithdraw> IWithdrawalsEndpoint.WithdrawFundsToPaymentMethodAsync(string paymentMethodId, decimal amount, string currency)
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
            .PostJsonAsync(d)
            .ReceiveJson<PaymentMethodWithdraw>();
      }

      Task<CoinbaseWithdraw> IWithdrawalsEndpoint.WithdrawFundsToCoinbaseAsync(string coinbaseAccountId, decimal amount, string currency)
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
            .PostJsonAsync(d)
            .ReceiveJson<CoinbaseWithdraw>();
      }

      Task<CryptoWithdraw> IWithdrawalsEndpoint.WithdrawFundsToCryptoAddressAsync(string cryptoAddress, decimal amount, string currency)
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
            .PostJsonAsync(d)
            .ReceiveJson<CryptoWithdraw>();
      }
   }
}
