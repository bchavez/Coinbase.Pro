using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using Coinbase.Pro.WebSockets;
using SuperSocket.ClientEngine;
using SuperSocket.ClientEngine.Proxy;
using WebSocket4Net;
using static System.Console;

namespace Examples
{
   public class ResilientWebSocket
   {
      private readonly Credentials credentials;
      private CoinbaseProWebSocket coinbase;
      private Subscription subscription;
      private SemaphoreSlim locker = new SemaphoreSlim(1, 1);
      private CancellationTokenSource cts;

      public ResilientWebSocket(Credentials credentials)
      {
         this.credentials = credentials;
      }

      public Task Start(Subscription subscription)
      {
         this.subscription = subscription;
         this.cts = new CancellationTokenSource();
         return Task.Run(() => SafeReconnect());
      }

      public async Task Stop()
      {
         this.cts?.Cancel();

         WriteLine("Waiting 80 sec for shutdown...");
         this.locker.Wait(80_000);

         WriteLine("Shutdown complete.");
         WriteLine("!! Websocket is closed! ResilientWebSocket stopped.");
      }

      private async Task Reconnect(Credentials creds, Subscription subscription)
      {
         if (this.cts.IsCancellationRequested) return;

         this.coinbase = new CoinbaseProWebSocket(new WebSocketConfig
            {
               ApiKey = creds.ApiKey,
               Secret = creds.ApiSecret,
               Passphrase = creds.ApiPassphrase,
               SocketUri = "wss://ws-feed-public.sandbox.pro.coinbase.com"
            });

         WriteLine(">> Connecting websocket...");

         //Uncomment depending on your TFM if you want to debug the websocket
         //connection to Coinbase Pro with Fiddler
#if !NETFRAMEWORK
         coinbase.EnableFiddlerDebugProxy(new HttpConnectProxy(IPEndPoint.Parse("127.0.0.1:8888")));
#else
         coinbase.EnableFiddlerDebugProxy(new HttpConnectProxy(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888)));
#endif

         var result = await coinbase.ConnectAsync();
         if( !result.Success )
         {
            var ex = new Exception("Connect failed.")
               {
                  Data = {{"ConnectResult", result}}
               };
            throw ex;
         }

         WriteLine(">> Connected.");

         coinbase.RawSocket.Closed += Websocket_Closed;
         coinbase.RawSocket.Error += Websocket_Error;
         coinbase.RawSocket.MessageReceived += Websocket_MessageReceived;

         WriteLine(">> Subscribing to events...");
         var sub = new Subscription
            {
               Channels = subscription.Channels,
               ProductIds = subscription.ProductIds
            };
         await coinbase.SubscribeAsync(sub);

         WriteLine(">> Subscribed.");
      }

      private void Websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
      {
         if (this.cts.IsCancellationRequested) return;

         WriteLine("Message received.");
         if (WebSocketHelper.TryParse(e.Message, out var msg))
         {
            if (msg is HeartbeatEvent hb)
            {
               WriteLine($"Sequence: {hb.Sequence}, Last Trade Id: {hb.LastTradeId}");
            }
         }
      }

      private void Websocket_Error(object sender, ErrorEventArgs e)
      {
         if (this.cts.IsCancellationRequested) return;

         WriteLine("!! Websocket Error! ");
         WriteLine(e);
      }

      private void Websocket_Closed(object sender, EventArgs e)
      {
         if (this.cts.IsCancellationRequested) return;

         WriteLine("!! The websocket closed!");
         WriteLine("!! Reconnecting...");
         Task.Run(() => SafeReconnect());
      }

      private async Task SafeReconnect()
      {
         if( this.cts.IsCancellationRequested ) return;

         if (!locker.Wait(0)) return; //any threads that can't acquire the lock, go away

         while ( !this.cts.IsCancellationRequested )
         {
            try
            {
               SafeShutdown();
               await Reconnect(this.credentials, this.subscription);
               break;
            }
            catch(Exception e)
            {
               WriteLine(e);
            }
         }

         locker.Release();
      }

      private void SafeShutdown()
      {
         if (this.coinbase?.RawSocket is null) return;
         if (this.subscription is null) return;

         this.coinbase.RawSocket.Closed -= Websocket_Closed;
         this.coinbase.RawSocket.Error -= Websocket_Error;
         this.coinbase.RawSocket.MessageReceived -= Websocket_MessageReceived;

         if (this.coinbase.RawSocket.State == WebSocketState.Open)
         {
            this.coinbase.Unsubscribe(this.subscription);
            WriteLine("!! Closing Websocket...");
            this.coinbase.RawSocket.Close();
         }

         this.coinbase.Dispose();
      }
   }
}
