using System.Collections.Generic;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using Flurl;
using Flurl.Http;

namespace Coinbase.Pro
{
   public interface IAccountsEndpoint
   {
      Task<List<Account>> List();
      Task<Account> GetAccount(string accountId);
      Task<PagedResponse<AccountHistory>> GetAccountHistory(string accountId, int? limit = null, long? before = null, long? after = null);
      Task<PagedResponse<AccountHold>> GetAccountHold(string accountId, int? limit = null, long? before = null, long? after = null);
   }

   public partial class CoinbaseProClient : IAccountsEndpoint
   {
      public IAccountsEndpoint Accounts => this;

      protected internal Url AccountsEndpoint => this.Config.ApiUrl.AppendPathSegment("accounts");

      Task<List<Account>> IAccountsEndpoint.List()
      {
         return this.AccountsEndpoint
            .WithClient(this)
            .GetJsonAsync<List<Account>>();
      }

      Task<Account> IAccountsEndpoint.GetAccount(string accountId)
      {
         return this.AccountsEndpoint
            .WithClient(this)
            .AppendPathSegment(accountId)
            .GetJsonAsync<Account>();
      }

      Task<PagedResponse<AccountHistory>> IAccountsEndpoint.GetAccountHistory(string accountId, int? limit, long? before, long? after)
      {
         return this.AccountsEndpoint
            .WithClient(this)
            .AppendPathSegments(accountId, "ledger")
            .AsPagedRequest(limit, before, after)
            .GetPagedJsonAsync<AccountHistory>();
      }

      Task<PagedResponse<AccountHold>> IAccountsEndpoint.GetAccountHold(string accountId, int? limit, long? before, long? after)
      {
         return this.AccountsEndpoint
            .WithClient(this)
            .AppendPathSegments(accountId, "holds")
            .AsPagedRequest(limit, before, after)
            .GetPagedJsonAsync<AccountHold>();
      }
   }
}
