using System.Threading;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using Flurl.Http;

namespace Coinbase.Pro
{
   public interface IFeesEndpoint
   {
      /// <summary>
      /// This request will return your current maker and taker fee rates,
      /// as well as your 30-day trailing volume. Quoted rates are subject
      /// to change.
      /// </summary>
      /// <param name="cancellationToken"></param>
      Task<MakerTakerFees> GetCurrentFeesAsync(CancellationToken cancellationToken = default);
   }

   public partial class CoinbaseProClient : IFeesEndpoint
   {
      public IFeesEndpoint Fees => this;

      Task<MakerTakerFees> IFeesEndpoint.GetCurrentFeesAsync(CancellationToken cancellationToken)
      {
         return this.Config.ApiUrl
            .WithClient(this)
            .AppendPathSegment("fees")
            .GetJsonAsync<MakerTakerFees>(cancellationToken);
      }
   }
}
