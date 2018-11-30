using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using Flurl;
using Flurl.Http;

namespace Coinbase.Pro
{
   public interface ICoinbaseAccountsEndpoint
   {
      /// <summary>
      /// Get a list of your payment methods.
      /// </summary>
      /// <param name="cancellationToken"></param>
      Task<List<CoinbaseAccount>> GetAllAccountsAsync(CancellationToken cancellationToken = default);
   }

   public partial class CoinbaseProClient : ICoinbaseAccountsEndpoint
   {
      public ICoinbaseAccountsEndpoint CoinbaseAccounts => this;

      protected internal Url CoinbaseAccountsEndpoint => this.Config.ApiUrl.AppendPathSegment("coinbase-accounts");

      Task<List<CoinbaseAccount>> ICoinbaseAccountsEndpoint.GetAllAccountsAsync(CancellationToken cancellationToken)
      {
         return this.CoinbaseAccountsEndpoint
            .WithClient(this)
            .GetJsonAsync<List<CoinbaseAccount>>(cancellationToken);
      }
   }
}
