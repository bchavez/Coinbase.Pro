using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using Flurl;
using Flurl.Http;

namespace Coinbase.Pro
{
   public interface IPaymentMethodsEndpoint
   {
      /// <summary>
      /// Get a list of your payment methods.
      /// </summary>
      Task<List<PaymentMethod>> GetAllPaymentMethodsAsync(CancellationToken cancellationToken = default);
   }

   public partial class CoinbaseProClient : IPaymentMethodsEndpoint
   {
      public IPaymentMethodsEndpoint PaymentMethods => this;

      protected internal Url PaymentMethodsEndpoint => this.Config.ApiUrl.AppendPathSegment("payment-methods");

      Task<List<PaymentMethod>> IPaymentMethodsEndpoint.GetAllPaymentMethodsAsync(CancellationToken cancellationToken)
      {
         return this.PaymentMethodsEndpoint
            .WithClient(this)
            .GetJsonAsync<List<PaymentMethod>>(cancellationToken);
      }
   }
}
