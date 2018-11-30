using System.Collections.Generic;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using Flurl;
using Flurl.Http;

namespace Coinbase.Pro
{
   public interface IAccountsEndpoint
   {
      /// <summary>
      /// Get a list of trading accounts.
      /// </summary>
      Task<List<Account>> GetAllAccountsAsync();

      /// <summary>
      /// Information for a single account. Use this endpoint when you know the account_id.
      /// </summary>
      Task<Account> GetAccountAsync(string accountId);

      /// <summary>
      /// Get account activity. Account activity either increases or decreases your account balance. Items are paginated and sorted latest first.
      /// </summary>
      Task<PagedResponse<AccountHistory>> GetAccountHistoryAsync(string accountId, int? limit = null, long? before = null, long? after = null);

      /// <summary>
      /// Holds are placed on an account for any active orders or pending withdraw requests. As an order is filled, the hold amount is updated. If an order is canceled, any remaining hold is removed. For a withdraw, once it is completed, the hold is removed.
      /// </summary>
      Task<PagedResponse<AccountHold>> GetAccountHoldAsync(string accountId, int? limit = null, long? before = null, long? after = null);
   }

   public partial class CoinbaseProClient : IAccountsEndpoint
   {
      public IAccountsEndpoint Accounts => this;

      protected internal Url AccountsEndpoint => this.Config.ApiUrl.AppendPathSegment("accounts");

      Task<List<Account>> IAccountsEndpoint.GetAllAccountsAsync()
      {
         return this.AccountsEndpoint
            .WithClient(this)
            .GetJsonAsync<List<Account>>();
      }

      Task<Account> IAccountsEndpoint.GetAccountAsync(string accountId)
      {
         return this.AccountsEndpoint
            .WithClient(this)
            .AppendPathSegment(accountId)
            .GetJsonAsync<Account>();
      }

      Task<PagedResponse<AccountHistory>> IAccountsEndpoint.GetAccountHistoryAsync(string accountId, int? limit, long? before, long? after)
      {
         return this.AccountsEndpoint
            .WithClient(this)
            .AppendPathSegments(accountId, "ledger")
            .AsPagedRequest(limit, before, after)
            .GetPagedJsonAsync<AccountHistory>();
      }

      Task<PagedResponse<AccountHold>> IAccountsEndpoint.GetAccountHoldAsync(string accountId, int? limit, long? before, long? after)
      {
         return this.AccountsEndpoint
            .WithClient(this)
            .AppendPathSegments(accountId, "holds")
            .AsPagedRequest(limit, before, after)
            .GetPagedJsonAsync<AccountHold>();
      }
   }
}
