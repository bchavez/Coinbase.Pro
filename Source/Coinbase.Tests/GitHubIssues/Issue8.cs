using System.Threading.Tasks;
using Coinbase.Pro;
using Coinbase.Tests.EndpointTests;
using FluentAssertions;
using NUnit.Framework;

namespace Coinbase.Tests.GitHubIssues
{
   public class Issue8 : Test
   {
      public const string Json = @"[
{
      ""created_at"":""2017-09-27T22:57:14.483Z"",
      ""trade_id"":1111,
      ""product_id"":""ETH-BTC"",
      ""order_id"":""2F4F7C76-644E-4DCF-A7E1-5BEB57502E76"",
      ""user_id"":""CE3FABAC-CA62-45C2-8AAF-DB59FE353C90"",
      ""profile_id"":""C1B3739F-2301-4821-86B4-15463C7C353B"",
      ""liquidity"":""M"",
      ""price"":""0.08000"",
      ""size"":""0.9"",
      ""fee"":""0.0000000000000000"",
      ""side"":""sell"",
      ""settled"":true,
      ""usd_volume"":null
   },
   {
      ""created_at"":""2017-09-27T22:54:22.405Z"",
      ""trade_id"":2222,
      ""product_id"":""ETH-BTC"",
      ""order_id"":""58F5D697-66C4-428D-82A0-CEDFB881F8DA"",
      ""user_id"":""4476C87B-E489-44EC-9822-B8E2522FD17E"",
      ""profile_id"":""FEBC6314-2721-4C40-9EE6-CF5FAE135F9E"",
      ""liquidity"":""T"",
      ""price"":""0.80"",
      ""size"":""0.09"",
      ""fee"":""0.02"",
      ""side"":""sell"",
      ""settled"":true,
      ""usd_volume"":null
   }
]";

      [Test]
      public async Task can_deser_null_usdvolume()
      {
         var client = new CoinbaseProClient();

         server.RespondWithPagedResult(Json, 11, 33);

         var f = await client.Fills.GetFillsByProductIdAsync("ETH-BTC");

         f.Data.Count.Should().Be(2);
      }
   }
}
