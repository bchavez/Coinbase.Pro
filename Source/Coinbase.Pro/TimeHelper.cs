using System;
using System.Globalization;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using Flurl;
using Flurl.Http;

namespace Coinbase.Pro
{
   public static class TimeHelper
   {
      private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

      public static long GetCurrentUnixTimestampSeconds()
      {
#if STANDARD
         return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
#else
         return (long)(DateTime.UtcNow - UnixEpoch).TotalSeconds;
#endif
      }

      public static DateTimeOffset FromUnixTimestampSeconds(long seconds)
      {
#if STANDARD
         return DateTimeOffset.FromUnixTimeSeconds(seconds);
#else
         return UnixEpoch.AddSeconds(seconds);
#endif
      }

      public static async Task<string> GetCurrentTimestampAsync(bool useTimeApi)
      {
         if (useTimeApi)
         {
            var timeResult =
               await CoinbaseProClient.Endpoint
                  .AppendPathSegment("time")
                  .WithHeader("User-Agent", CoinbaseProClient.UserAgent)
                  .GetJsonAsync<Time>().ConfigureAwait(false);

            return timeResult.Epoch.ToCoinbaseTime();
         }
         else
         {
            return GetCurrentUnixTimestampSeconds().ToCoinbaseTime();
         }
      }

   }

   internal static class TimeHelperExtensions
   {
      public static string ToCoinbaseTime(this long val)
      {
         return val.ToString("D", CultureInfo.InvariantCulture);
      }
   }
}
