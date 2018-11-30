using System.Collections.Generic;
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
      Task<List<TrailingVolume>> GetTrailingVolumeAsync();
   }

   public partial class CoinbaseProClient : IUserAccountEndpoint
   {
      public IUserAccountEndpoint UserAccount => this;

      protected internal Url UserAccountEndpoint => this.Config.ApiUrl.AppendPathSegments("users", "self");

      Task<List<TrailingVolume>> IUserAccountEndpoint.GetTrailingVolumeAsync()
      {
         return this.UserAccountEndpoint
            .WithClient(this)
            .AppendPathSegment("trailing-volume")
            .GetJsonAsync<List<TrailingVolume>>();
      }
   }
}
