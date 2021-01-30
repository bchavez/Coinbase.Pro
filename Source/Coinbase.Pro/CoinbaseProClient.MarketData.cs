using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using Flurl;
using Flurl.Http;

namespace Coinbase.Pro
{
   public interface IMarketDataEndpoint
   {
      /// <summary>
      /// Get a list of available currency pairs for trading.
      /// </summary>
      /// <param name="cancellationToken"></param>
      /// <remarks>
      /// The base_min_size and base_max_size fields define the min and max order size. The quote_increment field specifies the min order price as well as the price increment.
      /// The order price must be a multiple of this increment(i.e. if the increment is 0.01, order prices of 0.001 or 0.021 would be rejected).
      /// Product ID will not change once assigned to a product but the min/max/quote sizes can be updated in the future.
      /// </remarks>
      Task<List<Product>> GetProductsAsync(CancellationToken cancellationToken = default);

      /// <summary>
      /// Get a list of open orders for a product. The amount of detail shown can be customized with the level parameter.
      /// </summary>
      /// <param name="productId"></param>
      /// <param name="level">1. Only the best bid and ask. 2. Top 50 bids and asks (aggregated). 3. Full order book (non aggregated)</param>
      /// <param name="cancellationToken"></param>
      Task<OrderBook> GetOrderBookAsync(string productId, int level = 1, CancellationToken cancellationToken = default);

      /// <summary>
      /// Snapshot information about the last trade (tick), best bid/ask and 24h volume.
      /// </summary>
      Task<Ticker> GetTickerAsync(string productId, CancellationToken cancellationToken = default);

      /// <summary>
      /// List the latest trades for a product.
      /// </summary>
      /// <param name="productId">The coinbase specific product id. IE: 'BTC-USD', 'ETH-USD', etc.</param>
      /// <param name="limit">Number of results per request. Maximum 100. (default 100)</param>
      /// <param name="before">Request page before (newer) this pagination id.</param>
      /// <param name="after">Request page after (older) this pagination id.</param>
      Task<PagedResponse<Trade>> GetTradesAsync(
         string productId,
         int? limit = null, string before = null, string after = null,
         CancellationToken cancellationToken = default);

      /// <summary>
      /// Historic rates for a product. Rates are returned in grouped buckets based on requested granularity.
      /// </summary>
      /// <param name="start">Start time</param>
      /// <param name="end">End time</param>
      /// <param name="granularity">Desired timeslice in seconds. The granularity field must be one of the following values: {60, 300, 900, 3600, 21600, 86400}. Otherwise, your request will be rejected. These values correspond to timeslices representing one minute, five minutes, fifteen minutes, one hour, six hours, and one day, respectively.</param>
      Task<List<Candle>> GetHistoricRatesAsync(
         string productId, DateTime start, DateTime end, int granularity,
         CancellationToken cancellationToken = default);

      /// <summary>
      /// Get 24 hr stats for the product. volume is in base currency units. open, high, low are in quote currency units.
      /// </summary>
      Task<Stats> GetStatsAsync(string productId, CancellationToken cancellationToken = default);

      /// <summary>
      /// List known currencies.
      /// </summary>
      Task<List<Currency>> GetCurrenciesAsync(CancellationToken cancellationToken = default);

      /// <summary>
      /// Get the API server time.
      /// </summary>
      Task<Time> GetTimeAsync(CancellationToken cancellationToken = default);
   }


   public partial class CoinbaseProClient : IMarketDataEndpoint
   {
      public IMarketDataEndpoint MarketData => this;

      protected internal Url ProductsEndpoint => this.Config.ApiUrl.AppendPathSegment("products");

      Task<List<Product>> IMarketDataEndpoint.GetProductsAsync(CancellationToken cancellationToken)
      {
         return this.ProductsEndpoint
            .WithClient(this)
            .GetJsonAsync<List<Product>>(cancellationToken);
      }

      Task<OrderBook> IMarketDataEndpoint.GetOrderBookAsync(string productId, int level, CancellationToken cancellationToken)
      {
         return this.ProductsEndpoint
            .WithClient(this)
            .AppendPathSegments(productId, "book")
            .SetQueryParam("level", level)
            .GetJsonAsync<OrderBook>(cancellationToken);
      }

      Task<Ticker> IMarketDataEndpoint.GetTickerAsync(string productId, CancellationToken cancellationToken)
      {
         return this.ProductsEndpoint
            .WithClient(this)
            .AppendPathSegments(productId, "ticker")
            .GetJsonAsync<Ticker>(cancellationToken);
      }

      Task<PagedResponse<Trade>> IMarketDataEndpoint.GetTradesAsync(
         string productId,
         int? limit, string before, string after,
         CancellationToken cancellationToken)
      {
         return this.ProductsEndpoint
            .WithClient(this)
            .AppendPathSegments(productId, "trades")
            .AsPagedRequest(limit, before, after)
            .GetPagedJsonAsync<Trade>(cancellationToken);
         //.GetAsync();
         //.GetJsonAsync<List<Trade>>();
         //.GetJsonAsync<List<Trade>>();
         //HttpResponseMessageExtensions.
      }

      Task<List<Candle>> IMarketDataEndpoint.GetHistoricRatesAsync(
         string productId, DateTime start, DateTime end, int granularity,
         CancellationToken cancellationToken)
      {
         return this.ProductsEndpoint
            .WithClient(this)
            .AppendPathSegments(productId, "candles")
            .SetQueryParam("start", start.ToString("O"))
            .SetQueryParam("end", end.ToString("O"))
            .SetQueryParam("granularity", granularity)
            .GetJsonAsync<List<Candle>>(cancellationToken);
      }

      Task<Stats> IMarketDataEndpoint.GetStatsAsync(string productId, CancellationToken cancellationToken)
      {
         return this.ProductsEndpoint
            .WithClient(this)
            .AppendPathSegments(productId, "stats")
            .GetJsonAsync<Stats>(cancellationToken);
      }

      Task<List<Currency>> IMarketDataEndpoint.GetCurrenciesAsync(CancellationToken cancellationToken)
      {
         return this.Config.ApiUrl
            .WithClient(this)
            .AppendPathSegment("currencies")
            .GetJsonAsync<List<Currency>>(cancellationToken);
      }

      Task<Time> IMarketDataEndpoint.GetTimeAsync(CancellationToken cancellationToken)
      {
         return this.Config.ApiUrl
            .WithHeader("User-Agent", UserAgent)
            .AppendPathSegment("time")
            .GetJsonAsync<Time>(cancellationToken);
      }


      //private PagedResponse<T> MakePagedResponse<T>(HttpResponseMessage r, T body)
      //{
      //   var result = new PagedResponse<T>()
      //      {
      //         Data = body
      //      };

      //   if( r.Headers.TryGetValues(HeaderNames.Before, out var beforeItems)
      //       && long.TryParse(beforeItems?.FirstOrDefault(), out var before))
      //   {
      //      result.Before = before;
      //   }

      //   if( r.Headers.TryGetValues(HeaderNames.After, out var afterItems)
      //       && long.TryParse(afterItems?.FirstOrDefault(), out var after) )
      //   {
      //      result.After = after;
      //   }

      //   return result;
      //}
   }
}
