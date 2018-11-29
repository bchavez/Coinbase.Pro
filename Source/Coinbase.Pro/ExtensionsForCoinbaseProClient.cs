using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using Flurl.Http;

namespace Coinbase.Pro
{
   public static class ExtensionsForCoinbaseProClient
   {
      public static IFlurlRequest AsPagedRequest(this IFlurlRequest r, int? limit = 100, long? before = null, long? after = null)
      {
         return r.SetQueryParam("limit", limit)
            .SetQueryParam("before", before)
            .SetQueryParam("after", after);
      }

      public static async Task<PagedResponse<T>> GetPagedJsonAsync<T>(this IFlurlRequest request, CancellationToken cancellationToken = default(CancellationToken))
      {
         var task = request.GetAsync(cancellationToken);
         var r = await task.ConfigureAwait(false);

         var data = await task.ReceiveJson<List<T>>().ConfigureAwait(false);

         var p = new PagedResponse<T>
            {
               Data = data
            };

         if( long.TryParse(r.GetHeaderValue(HeaderNames.Before), out var before) )
         {
            p.Before = before;
         }

         if( long.TryParse(r.GetHeaderValue(HeaderNames.After), out var after) )
         {
            p.After = after;
         }

         return p;
      }

      //internal static HttpCall GetHttpCall(this HttpRequestMessage request)
      //{
      //   if (request?.Properties != null && request.Properties.TryGetValue("FlurlHttpCall", out var obj) && obj is HttpCall call)
      //      return call;
      //   return null;
      //}

      //public static async Task<T> ReceiveJson<T>(this HttpResponseMessage resp)
      //{
      //   using (resp)
      //   {
      //      if (resp == null) return default(T);
      //      var call = resp.RequestMessage.GetHttpCall();
      //      using (var stream = await resp.Content.ReadAsStreamAsync().ConfigureAwait(false))
      //      {
      //         try
      //         {
      //            return call.FlurlRequest.Settings.JsonSerializer.Deserialize<T>(stream);
      //         }
      //         catch (Exception ex)
      //         {
      //            var body = await resp.Content.ReadAsStringAsync();
      //            call.Exception = new FlurlParsingException(call, "JSON", body, ex);
      //            await FlurlRequest.HandleExceptionAsync(call, call.Exception, CancellationToken.None).ConfigureAwait(false);
      //            return default(T);
      //         }
      //      }
      //   }
      //}
   }
}
