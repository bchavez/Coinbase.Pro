using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Coinbase.Pro;
using Coinbase.Pro.Websockets;
using Coinbase.Pro.Websockets.Models;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Coinbase.Tests.IntegrationTests
{
   [Explicit]
   public class WebsocketTests : IntegrationTests
   {
      private CoinbaseProWebsocket socket;

      [SetUp]
      public void BeforeEachTest()
      {
         socket = new CoinbaseProWebsocket(new WebsocketConfig
            {
               UseTimeApi = true,
               ApiKey = this.secrets.ApiKey,
               Secret = this.secrets.ApiSecret,
               Passphrase = this.secrets.ApiPassphrase,
               SocketUri = "wss://ws-feed-public.sandbox.pro.coinbase.com"
         });
      }

      [Test]
      public async Task connect()
      {
         await socket.ConnectAsync();

         var sub = new Subscription
            {
               ProductIds =
                  {
                     "BTC-USD",
                     "ETH-USD"
                  },
               Channels =
                  {
                     "level2",
                     "heartbeat",
                     JObject.FromObject(
                        new Channel
                           {
                              Name = "ticker",
                              ProductIds = {"ETH-BTC", "ETH-USD"}
                           })
                  }
            };

         await socket.SubscribeAsync(sub);

         socket.RawSocket.MessageReceived += RawSocket_MessageReceived;

         await Task.Delay(TimeSpan.FromMinutes(1));
      }

      private void RawSocket_MessageReceived(object sender, WebSocket4Net.MessageReceivedEventArgs e)
      {
         Console.WriteLine(e.Message);
         Debug.WriteLine(e.Message);

         if( WebSocketHelper.TryParse(e.Message, out var msg) )
         {
            if( msg is HeartbeatEvent hbe )
            {
               hbe.Dump();
            }
         }

      }

      [Test]
      public async Task connect_simple()
      {
         await socket.ConnectAsync();

         var sub = new Subscription
            {
               ProductIds =
                  {
                     "ETH-USD",
                  },
               Channels =
                  {
                     "heartbeat",
                  }
            };

         await socket.SubscribeAsync(sub);

         socket.RawSocket.MessageReceived += RawSocket_MessageReceived;

         await Task.Delay(TimeSpan.FromMinutes(1));

      }
   }
}
