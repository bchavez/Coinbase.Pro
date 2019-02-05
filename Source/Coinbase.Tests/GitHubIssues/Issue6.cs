using System.Threading.Tasks;
using Coinbase.Pro.Models;
using Coinbase.Pro.WebSockets;
using FluentAssertions;
using NUnit.Framework;

namespace Coinbase.Tests.GitHubIssues
{
   public class Issue6
   {
      [Test]
      public void valid_markets_with_no_volume_should_parse_bestbid_bestask()
      {
         var tickerJsonWithNulls = @"{
   ""type"":""ticker"",
   ""product_id"":""ZIL-USDC"",
   ""price"":0,""open_24h"":0,
   ""volume_24h"":0,""low_24h"":0,
   ""high_24h"":0,""volume_30d"":0,
   ""best_bid"":null,""best_ask"":null
}";

         WebSocketHelper.TryParse(tickerJsonWithNulls, out var o);

         o.Should().BeOfType<TickerEvent>();
      }
   }
}
