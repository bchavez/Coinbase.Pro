using System;
using System.Collections.Generic;
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
      /// <remarks>
      /// The base_min_size and base_max_size fields define the min and max order size. The quote_increment field specifies the min order price as well as the price increment.
      /// The order price must be a multiple of this increment(i.e. if the increment is 0.01, order prices of 0.001 or 0.021 would be rejected).
      /// Product ID will not change once assigned to a product but the min/max/quote sizes can be updated in the future.
      /// </remarks>
      //[Get("/products")]
      Task<List<Product>> GetProductsAsync();

      /// <summary>
      /// Get a list of open orders for a product. The amount of detail shown can be customized with the level parameter.
      /// </summary>
      /// <param name="productId"></param>
      /// <param name="level">1. Only the best bid and ask. 2. Top 50 bids and asks (aggregated). 3. Full order book (non aggregated)</param>
      //[Get("/products/{productId}/book")]
      Task<OrderBook> GetOrderBookAsync(string productId, int level = 1);

      /// <summary>
      /// Snapshot information about the last trade (tick), best bid/ask and 24h volume.
      /// </summary>
      //[Get("/products/{productId}/ticker")]
      Task<Ticker> GetTickerAsync(string productId);

      /// <summary>
      /// List the latest trades for a product.
      /// </summary>
      /// <param name="productId">The coinbase specific product id. IE: 'BTC-USD', 'ETH-USD', etc.</param>
      /// <param name="limit">Number of results per request. Maximum 100. (default 100)</param>
      /// <param name="before">Request page before (newer) this pagination id.</param>
      /// <param name="after">Request page after (older) this pagination id.</param>
      Task<PagedResponse<Trade>> GetTradesAsync(
         string productId,
         int? limit = null,
         long? before = null,
         long? after = null);

      /// <summary>
      /// Historic rates for a product. Rates are returned in grouped buckets based on requested granularity.
      /// </summary>
      /// <param name="granularity">Desired timeslice in seconds. The granularity field must be one of the following values: {60, 300, 900, 3600, 21600, 86400}. Otherwise, your request will be rejected. These values correspond to timeslices representing one minute, five minutes, fifteen minutes, one hour, six hours, and one day, respectively.</param>
      //[Get("/products/{productId}/candles")]
      Task<List<Candle>> GetHistoricRatesAsync(string productId, DateTime start, DateTime end, int granularity);

      Task<Stats> GetStatsAsync(string productId);
      Task<List<Currency>> GetCurrenciesAsync();
      Task<Time> GetTimeAsync();
   }


   public partial class CoinbaseProClient : IMarketDataEndpoint
   {
      public IMarketDataEndpoint MarketData => this;

      protected internal Url ProductsEndpoint => this.Config.ApiUrl.AppendPathSegment("products");

      Task<List<Product>> IMarketDataEndpoint.GetProductsAsync()
      {
         return this.ProductsEndpoint
            .WithClient(this)
            .GetJsonAsync<List<Product>>();
      }

      Task<OrderBook> IMarketDataEndpoint.GetOrderBookAsync(string productId, int level)
      {
         return this.ProductsEndpoint
            .WithClient(this)
            .AppendPathSegments(productId, "book")
            .SetQueryParam("level", level)
            .GetJsonAsync<OrderBook>();
      }

      Task<Ticker> IMarketDataEndpoint.GetTickerAsync(string productId)
      {
         return this.ProductsEndpoint
            .WithClient(this)
            .AppendPathSegments(productId, "ticker")
            .GetJsonAsync<Ticker>();
      }

      Task<PagedResponse<Trade>> IMarketDataEndpoint.GetTradesAsync(
         string productId,
         int? limit,
         long? before,
         long? after)
      {
         return this.ProductsEndpoint
            .WithClient(this)
            .AppendPathSegments(productId, "trades")
            .AsPagedRequest(limit, before, after)
            .GetPagedJsonAsync<Trade>();
         //.GetAsync();
         //.GetJsonAsync<List<Trade>>();
         //.GetJsonAsync<List<Trade>>();
         //HttpResponseMessageExtensions.
      }

      Task<List<Candle>> IMarketDataEndpoint.GetHistoricRatesAsync(string productId, DateTime start, DateTime end, int granularity)
      {
         return this.ProductsEndpoint
            .WithClient(this)
            .AppendPathSegments(productId, "candles")
            .SetQueryParam("start", start.ToString("O"))
            .SetQueryParam("end", end.ToString("O"))
            .SetQueryParam("granularity", granularity)
            .GetJsonAsync<List<Candle>>();
      }

      public Task<Stats> GetStatsAsync(string productId)
      {
         return this.ProductsEndpoint
            .WithClient(this)
            .AppendPathSegments(productId, "stats")
            .GetJsonAsync<Stats>();
      }

      Task<List<Currency>> IMarketDataEndpoint.GetCurrenciesAsync()
      {
         return this.Config.ApiUrl
            .WithClient(this)
            .AppendPathSegment("currencies")
            .GetJsonAsync<List<Currency>>();
      }

      Task<Time> IMarketDataEndpoint.GetTimeAsync()
      {
         return this.Config.ApiUrl
            .WithHeader("User-Agent", UserAgent)
            .AppendPathSegment("time")
            .GetJsonAsync<Time>();
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
