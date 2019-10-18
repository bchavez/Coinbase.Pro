using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using Flurl;
using Flurl.Http;

namespace Coinbase.Pro
{
   public interface IUserAccountEndpoint
   {
      /// <summary>
      /// This request will return your 30-day trailing volume for all products.
      /// This is a cached value that’s calculated every day at midnight UTC.
      /// </summary>
      /// <param name="cancellationToken"></param>
      Task<List<TrailingVolume>> GetTrailingVolumeAsync(CancellationToken cancellationToken = default);
   }

   public partial class CoinbaseProClient : IUserAccountEndpoint
   {
      public IUserAccountEndpoint UserAccount => this;

      protected internal Url UserAccountEndpoint => this.Config.ApiUrl.AppendPathSegments("users", "self");

      Task<List<TrailingVolume>> IUserAccountEndpoint.GetTrailingVolumeAsync(CancellationToken cancellationToken)
      {
         return this.UserAccountEndpoint
            .WithClient(this)
            .AppendPathSegment("trailing-volume")
            .GetJsonAsync<List<TrailingVolume>>(cancellationToken);
      }
   }

   public interface IFeesEndpoint
   {
      /// <summary>
      /// This request will return your 30-day trailing volume for all products.
      /// This is a cached value that’s calculated every day at midnight UTC.
      /// </summary>
      /// <param name="cancellationToken"></param>
      Task<Fee> GetFeesAsync(CancellationToken cancellationToken = default);
   }

   public partial class CoinbaseProClient : IFeesEndpoint
   {
      public IFeesEndpoint Fees => this;

      protected internal Url FeesEndpoint => this.Config.ApiUrl.AppendPathSegments("fees");

      Task<Fee> IFeesEndpoint.GetFeesAsync(CancellationToken cancellationToken)
      {
         return this.FeesEndpoint
            .WithClient(this)            
            .GetJsonAsync<Fee>(cancellationToken);
      }
   }



}
