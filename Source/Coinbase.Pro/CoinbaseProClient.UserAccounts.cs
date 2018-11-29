using System.Collections.Generic;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using Flurl;
using Flurl.Http;

namespace Coinbase.Pro
{
   public interface IUserAccountEndpoint
   {
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
