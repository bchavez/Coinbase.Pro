using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Coinbase.Pro.Models;

namespace Coinbase.Pro.WebSockets
{
   public static class WebSocketHelper
   {
      public static async Task<string> MakeAuthenticatedSubscriptionAsync(Subscription subscription, WebSocketConfig config)
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
               parsed = obj.ToObject<HeartbeatEvent>();
               break;

            case "subscriptions":
               parsed = obj.ToObject<SubscriptionsEvent>();
               break;

            case "ticker":
               parsed = obj.ToObject<TickerEvent>();
               break;

            case "snapshot":
               parsed = obj.ToObject<SnapshotEvent>();
               break;

            case "l2update":
               parsed = obj.ToObject<L2UpdateEvent>();
               break;

            case "received":
               parsed = obj.ToObject<ReceivedEvent>();
               break;

            case "open":
               parsed = obj.ToObject<OpenEvent>();
               break;

            case "done":
               parsed = obj.ToObject<DoneEvent>();
               break;

            case "match":
               parsed = obj.ToObject<MatchEvent>();
               break;

            case "change":
               parsed = obj.ToObject<ChangeEvent>();
               break;

            case "activate":
               parsed = obj.ToObject<ActivateEvent>();
               break;

            default:
               parsed = null;
               return false;
         }
         
         return true;
      }
   }
}
