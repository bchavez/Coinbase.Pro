using System;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using Coinbase.Pro.WebSockets;
using WebSocket4Net;

class WebSocketSnippets
{
   void CoinbaseProWebSocket()
   {
      #region CoinbaseProWebSocket

      //authenticated feed
      var socket = new CoinbaseProWebSocket(new WebSocketConfig
         {
            ApiKey = "my-api-key",
            Secret = "my-api-secret",
            Passphrase = "my-api-passphrase",
            //Override the SocketUri property to use Sandbox.
            //SocketUri = "wss://ws-feed-public.sandbox.pro.coinbase.com"
         });
      #endregion
   }

   void UnauthenticatedCoinbaseProWebSocket()
   {
      #region UnauthenticatedCoinbaseProWebSocket

      var socket = new CoinbaseProWebSocket();

      #endregion
   }

   async Task SubscribingToEvents(CoinbaseProWebSocket socket)
   {
      #region SubscribingToEvents
      //Using authenticated or unauthenticated instance `socket`
      //Connect the websocket,
      //when this connect method completes, the socket is ready or failure occured.
      var result = await socket.ConnectAsync();
      if( !result.Success ) throw new Exception("Failed to connect.");

      //add an event handler for the message received event on the raw socket
      socket.RawSocket.MessageReceived += RawSocket_MessageReceived;

      //create a subscription of what to listen to
      var sub = new Subscription
         {
            ProductIds =
               {
                  "BTC-USD",
               },
            Channels =
               {
                  "heartbeat",
               }
         };

      //send the subscription upstream
      await socket.SubscribeAsync(sub);

      //now wait for data.
      await Task.Delay(TimeSpan.FromMinutes(1));
      #endregion
   }
   #region RawSocket_MessageReceived
   private void RawSocket_MessageReceived(object sender, MessageReceivedEventArgs e)
   {
      //Try parsing the e.Message JSON.
      if( WebSocketHelper.TryParse(e.Message, out var msg) )
      {
         if( msg is HeartbeatEvent hb )
         {
            Console.WriteLine($"Sequence: {hb.Sequence}, Last Trade Id: {hb.LastTradeId}");
         }
      }
   }
   #endregion
}
