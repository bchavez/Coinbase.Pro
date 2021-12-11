using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Coinbase.Pro.WebSockets;
using Coinbase.Pro.Models;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Coinbase.Tests.IntegrationTests
{
   [Explicit]
   public class WebSocketTests : IntegrationTests
   {
      private CoinbaseProWebSocket socket;

      [SetUp]
      public void BeforeEachTest()
      {
         socket = new CoinbaseProWebSocket(new WebSocketConfig
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
         var result = await socket.ConnectAsync();
         if( !result.Success )
         {
            throw new Exception("Connection error.");
         }

         //https://docs.pro.coinbase.com/?r=1#protocol-overview
         // Request
         // Subscribe to ETH-USD and ETH-EUR with the level2, heartbeat and ticker channels,
         // plus receive the ticker entries for ETH-BTC and ETH-USD
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
         var result = await socket.ConnectAsync();
         if( !result.Success )
         {
            throw new Exception("Connect error.");
         }

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

      [Test]
      public async Task subscribe_with_error()
      {
         var result = await socket.ConnectAsync();
         if (!result.Success)
         {
            throw new Exception("Connect error.");
         }

         socket.RawSocket.MessageReceived += RawSocket_MessageReceived;

         var sub = new Subscription
            {
               ProductIds =
                  {
                     "ZZZ-YYY",
                  },
               Channels =
                  {
                     "heartbeat",
                  }
            };

         await socket.SubscribeAsync(sub);

         await Task.Delay(TimeSpan.FromMinutes(1));
      }
   }
}
