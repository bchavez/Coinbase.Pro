using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Coinbase.Pro.Websockets
{
   public static class WebSocketHelper
   {
      public static async Task<string> MakeAuthenticatedSubscriptionAsync(Subscription subscription, WebsocketConfig config)
      {
         subscription.ExtraJson.Add("key", config.ApiKey);
         subscription.ExtraJson.Add("passphrase", config.Passphrase);

         var timestamp = await TimeHelper.GetCurrentTimestampAsync(config.UseTimeApi)
            .ConfigureAwait(false);

         subscription.ExtraJson.Add("timestamp", timestamp);

         var signature = ApiKeyAuthenticator.GenerateSignature(timestamp, "GET", "/users/self/verify", null, config.Secret);

         subscription.ExtraJson.Add("signature", signature);

         return JsonConvert.SerializeObject(subscription);
      }

      //public static Dictionary<string, Type> WebSocketMessageTypes =
      //   new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
      //      {
      //         {nameof(Heartbeat), typeof(Heartbeat)}
      //      };

      public static bool TryParse(string json, out object parsed)
      {
         var obj = JObject.Parse(json);

         if (!obj.ContainsKey("type"))
         {
            parsed = null;
            return false;
         }

         var type = obj["type"].Value<string>();

         switch( type )
         {
            case "heartbeat":
               parsed = obj.ToObject<Models.Heartbeat>();
               break;

            case "subscriptions":
               parsed = obj.ToObject<Models.Subscriptions>();
               break;

            case "ticker":
               parsed = obj.ToObject<Models.Ticker>();
               break;

            case "snapshot":
               parsed = obj.ToObject<Models.Snapshot>();
               break;

            case "l2update":
               parsed = obj.ToObject<Models.L2Update>();
               break;

            case "received":
               parsed = obj.ToObject<Models.Received>();
               break;

            case "open":
               parsed = obj.ToObject<Models.Open>();
               break;

            case "done":
               parsed = obj.ToObject<Models.Done>();
               break;

            case "match":
               parsed = obj.ToObject<Models.Match>();
               break;

            case "change":
               parsed = obj.ToObject<Models.Change>();
               break;

            case "activate":
               parsed = obj.ToObject<Models.Activate>();
               break;

            default:
               parsed = null;
               return false;
         }
         
         return true;
      }
   }
}
