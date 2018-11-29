using System.Collections.Generic;
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
      Task<List<CoinbaseAccount>> ListAsync();
   }

   public partial class CoinbaseProClient : ICoinbaseAccountsEndpoint
   {
      public ICoinbaseAccountsEndpoint CoinbaseAccounts => this;

      protected internal Url CoinbaseAccountsEndpoint => this.Config.ApiUrl.AppendPathSegment("coinbase-accounts");

      Task<List<CoinbaseAccount>> ICoinbaseAccountsEndpoint.ListAsync()
      {
         return this.CoinbaseAccountsEndpoint
            .WithClient(this)
            .GetJsonAsync<List<CoinbaseAccount>>();
      }
   }
}
