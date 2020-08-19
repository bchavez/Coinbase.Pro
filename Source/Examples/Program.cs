using System;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Pro;
using Coinbase.Pro.Models;
using Coinbase.Pro.WebSockets;
using SuperSocket.ClientEngine.Proxy;
using static System.Console;

namespace Examples
{
   class Program
   {
      static async Task Main(string[] args)
      {
         WriteLine("Coinbase Example");

         var creds = Credentials.ReadCredentials(@"..\..\..\.secrets.txt");

#if NETFRAMEWORK
         ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
#endif

         WriteLine("Making Simple API call.");
         await MakeSimpleApiCall(creds);

         WriteLine("Subscribing to Websocket Events.");
         await SubscribeToWebsocketEvents(creds);

         WriteLine("Press enter to exit program.");
         ReadLine();
      }

      public static async Task MakeSimpleApiCall(Credentials creds)
      {
         using var client = new CoinbaseProClient(new Config
            {
               ApiKey = creds.ApiKey,
               Secret = creds.ApiSecret,
               Passphrase = creds.ApiPassphrase,
               ApiUrl = "https://api-public.sandbox.pro.coinbase.com"
            });

         //Uncomment if you want to debug requests to Coinbase with Fiddler
         //See more here: https://github.com/bchavez/Coinbase/wiki/Debugging-with-Fiddler
         //Uncommenting the line below will only apply to the CoinbaseProClient; not the
         //websocket client.
         client.EnableFiddlerDebugProxy("http://localhost.:8888");

         var products = await client.MarketData.GetProductsAsync();
         var productIds = products.Select(p => p.Id).ToArray();
         WriteLine(">> Available Product IDs:");
         foreach( var productId in productIds )
         {
            WriteLine($">>  {productId}");
         }

         var data = await client.MarketData.GetStatsAsync("BTC-USD");
         WriteLine($">> BTC-USD Volume: {data.Volume}");
      }

      public static async Task SubscribeToWebsocketEvents(Credentials creds)
      {
         var socket = new CoinbaseProWebSocket(new WebSocketConfig
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
         //socket.EnableFiddlerDebugProxy(new HttpConnectProxy(IPEndPoint.Parse("127.0.0.1:8888")));
#else
         //socket.EnableFiddlerDebugProxy(new HttpConnectProxy(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888)));
#endif

         await socket.ConnectAsync();

         WriteLine(">> Connected.");

         socket.RawSocket.Closed += Websocket_Closed;
         socket.RawSocket.Error += Websocket_Error;
         socket.RawSocket.MessageReceived += Websocket_MessageReceived;

         var sub = new Subscription
            {
               ProductIds = {"BTC-USD"},
               Channels = {"heartbeat"}
            };

         WriteLine(">> Subscribing to events...");
         await socket.SubscribeAsync(sub);

         WriteLine(">> Subscribed.");
      }

      private static void Websocket_Closed(object sender, EventArgs e)
      {
         WriteLine("!! The websocket closed!");
      }

      private static void Websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
      {
         WriteLine("!! Websocket Error! ");
         WriteLine(e);
      }

      private static void Websocket_MessageReceived(object sender, WebSocket4Net.MessageReceivedEventArgs e)
      {
         WriteLine("Message received.");
         if( WebSocketHelper.TryParse(e.Message, out var msg) )
         {
            if( msg is HeartbeatEvent hb )
            {
               WriteLine($"Sequence: {hb.Sequence}, Last Trade Id: {hb.LastTradeId}");
            }
         }
      }
   }
}
