using System;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperSocket.ClientEngine;
using WebSocket4Net;


namespace Coinbase.Pro.WebSockets
{
   public class WebSocketConfig
   {
      public string ApiKey { get; set; }
      public string Secret { get; set; }
      public string Passphrase { get; set; }

      public bool UseTimeApi { get; set; } = false;
      public string SocketUri { get; set; } = CoinbaseProWebSocket.Endpoint;

      public void EnsureValid()
      {
      }
   }

   public class ConnectResult
   {
      public ConnectResult(bool success, object sender, EventArgs eventArgs)
      {
         this.Success = success;
         this.Sender = sender;
         this.EventArgs = eventArgs;
      }

      public bool Success { get; }
      public object Sender { get; }
      public EventArgs EventArgs { get; }
   }

   public class CoinbaseProWebSocket : IDisposable
   {
      public const string Endpoint = "wss://ws-feed.pro.coinbase.com";

      public WebSocket RawSocket { get; set; }

      public CoinbaseProWebSocket(WebSocketConfig config = null)
      {
         this.Config = config ?? new WebSocketConfig();
      }

      public WebSocketConfig Config { get; }

      protected TaskCompletionSource<ConnectResult> connectingTcs;

      protected IProxyConnector Proxy { get; set; }

      /// <summary>
      /// Connect the websocket to Coinbase Pro.
      /// </summary>
      /// <returns></returns>
      public Task<ConnectResult> ConnectAsync()
      {
         if( this.RawSocket != null ) throw new InvalidOperationException(
            $"The {nameof(RawSocket)} is already created from a previous {nameof(ConnectAsync)} call. " +
            $"If you get this exception, you'll need to dispose of this {nameof(CoinbaseProWebSocket)} and create a new instance. " +
            $"Don't call {nameof(ConnectAsync)} multiple times on the same instance.");

         this.connectingTcs = new TaskCompletionSource<ConnectResult>();

         if( this.RawSocket is null )
         {
            this.RawSocket ??= new WebSocket(this.Config.SocketUri);
            this.RawSocket.Proxy = this.Proxy;
            this.RawSocket.Security.EnabledSslProtocols = SslProtocols.Tls12;
         }

         this.RawSocket.Opened += RawSocket_Opened;
         this.RawSocket.Error += RawSocket_Error;
         this.RawSocket.Open();

         return this.connectingTcs.Task;
      }

      private void RawSocket_Error(object sender, ErrorEventArgs e)
      {
         TrySetConnectResult(false, sender, e);
      }

      private void RawSocket_Opened(object sender, EventArgs e)
      {
         TrySetConnectResult(true, sender, e);
      }

      protected void TrySetConnectResult(bool result, object sender, EventArgs args)
      {
         var connectResult = new ConnectResult(result, sender, args);

         if( sender is WebSocket socket )
         {
            socket.Opened -= RawSocket_Opened;
            socket.Error -= RawSocket_Error;
         }

         Task.Run(() => this.connectingTcs.TrySetResult(connectResult));
      }

      public void EnableFiddlerDebugProxy(IProxyConnector proxy)
      {
         this.Proxy = proxy;
      }

      public async Task SubscribeAsync(Subscription subscription)
      {
         if( this.RawSocket.State != WebSocketState.Open ) throw new InvalidOperationException("Socket must be connected.");

         subscription.ExtraJson.Add("type", JToken.FromObject(MessageType.Subscribe));

         string subJson;
         if( !string.IsNullOrWhiteSpace(this.Config.ApiKey) )
         {
            subJson = await WebSocketHelper.MakeAuthenticatedSubscriptionAsync(subscription, this.Config)
               .ConfigureAwait(false);
         }
         else
         {
            subJson = JsonConvert.SerializeObject(subscription);
         }

         this.RawSocket.Send(subJson);
      }

      public void Unsubscribe(Subscription subscription)
      {
         subscription.ExtraJson.Add("type", JToken.FromObject(MessageType.Unsubscribe));

         var json = JsonConvert.SerializeObject(subscription);

         this.RawSocket.Send(json);
      }


      public void Dispose()
      {
         this.RawSocket?.Dispose();
         this.RawSocket = null;
      }
   }
}
