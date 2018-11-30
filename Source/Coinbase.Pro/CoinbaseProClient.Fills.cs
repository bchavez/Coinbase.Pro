using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using Flurl;
using Flurl.Http;

namespace Coinbase.Pro
{
   public interface IFillsEndpoint
   {
      /// <summary>
      /// Get a list of recent fills.
      /// </summary>
      Task<PagedResponse<Fill>> GetFillsByProductIdAsync(
         string productId,
         int? limit = null, long? before = null, long? after = null,
         CancellationToken cancellationToken = default);

      /// <summary>
      /// Get a list of recent fills.
      /// </summary>
      Task<PagedResponse<Fill>> GetFillsByOrderIdAsync(
         string orderId,
         int? limit = null, long? before = null, long? after = null,
         CancellationToken cancellationToken = default);
   }

   public partial class CoinbaseProClient : IFillsEndpoint
   {
      public IFillsEndpoint Fills => this;

      protected internal Url FillsEndpoint => this.Config.ApiUrl.AppendPathSegment("fills");


      Task<PagedResponse<Fill>> IFillsEndpoint.GetFillsByProductIdAsync(
         string productId,
         int? limit, long? before, long? after,
         CancellationToken cancellationToken)
      {
         return this.FillsEndpoint
            .WithClient(this)
            .SetQueryParam("product_id", productId)
            .AsPagedRequest(limit, before, after)
            .GetPagedJsonAsync<Fill>(cancellationToken);
      }

      Task<PagedResponse<Fill>> IFillsEndpoint.GetFillsByOrderIdAsync(
         string orderId,
         int? limit, long? before, long? after,
         CancellationToken cancellationToken)
      {
         return this.FillsEndpoint
            .WithClient(this)
            .SetQueryParam("order_id", orderId)
            .AsPagedRequest(limit, before, after)
            .GetPagedJsonAsync<Fill>(cancellationToken);
      }
   }
}
