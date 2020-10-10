using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Coinbase.Pro.Models
{

   [JsonConverter(typeof(StringEnumConverter))]
   public enum MessageType
   {
      [EnumMember(Value = "subscribe")]
      Subscribe,

      [EnumMember(Value = "unsubscribe ")]
      Unsubscribe
   }

   public partial class Subscription : Json
   {
      //[JsonProperty("type")]
      //public string Type { get; set; }

      public Subscription()
      {
         this.ProductIds = new List<string>();
         this.Channels = new JArray();
      }

      [JsonProperty("product_ids")]
      public List<string> ProductIds { get; set; }

      [JsonProperty("channels")]
      public JArray Channels { get; set; }
   }

   //public partial class Channel : Json
   public partial class Channel : Json
   {
      public Channel()
      {
         this.ProductIds = new List<string>();
      }

      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("product_ids")]
      public List<string> ProductIds { get; set; }
   }





   public class Event : Json
   {
      [JsonProperty("type")]
      public string Type { get; set; }
   }

   public partial class SubscriptionsEvent : Event
   {
      [JsonProperty("channels")]
      public Channel[] Channels { get; set; }
   }

   public partial class HeartbeatEvent : Event
   {
      [JsonProperty("sequence")]
      public long Sequence { get; set; }

      [JsonProperty("last_trade_id")]
      public long LastTradeId { get; set; }

      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      [JsonProperty("time")]
      public DateTimeOffset Time { get; set; }
   }

   public partial class TickerEvent : Event
   {
      [JsonProperty("trade_id")]
      public long TradeId { get; set; }

      [JsonProperty("sequence")]
      public long Sequence { get; set; }

      [JsonProperty("time")]
      public DateTimeOffset Time { get; set; }

      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      [JsonProperty("price")]
      public decimal Price { get; set; }

      [JsonProperty("side")]
      public OrderSide Side { get; set; }

      [JsonProperty("last_size")]
      public decimal LastSize { get; set; }

      [JsonProperty("best_bid")]
      public decimal? BestBid { get; set; }

      [JsonProperty("best_ask")]
      public decimal? BestAsk { get; set; }
   }


   public partial class SnapshotEvent : Event
   {
      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      [JsonProperty("bids", ItemConverterType = typeof(OrderLiquidityConverter))]
      public List<OrderLiquidity> Bids { get; set; }

      [JsonProperty("asks", ItemConverterType = typeof(OrderLiquidityConverter))]
      public List<OrderLiquidity> Asks { get; set; }
   }

   public class OrderLiquidity
   {
      public decimal Price { get; set; }
      public decimal Size { get; set; }
   }

   public class OrderLiquidityConverter : JsonConverter2<OrderLiquidity>
   {
      public override OrderLiquidity ReadJson(JsonReader reader, Type objectType, OrderLiquidity existingValue, bool hasExistingValue, JsonSerializer serializer)
      {
         var price = reader.ReadAsDecimal();
         var size = reader.ReadAsDecimal();

         reader.Read();

         if (price is null) throw new JsonReaderException("Price in L2 update change is unexpectedly null.");
         if (size is null) throw new JsonReaderException("Size in L2 update change is unexpectedly null.");

         var ol = new OrderLiquidity
            {
               Price = price.Value,
               Size = size.Value
            };

         return ol;
      }
   }

   public partial class L2UpdateEvent : Event
   {
      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      [JsonProperty("changes", ItemConverterType = typeof(L2UpdateChangeConverter))]
      public List<L2UpdateChange> Changes { get; set; }

      [JsonProperty("time")]
      public DateTimeOffset? Time { get; set; }
   }

   public class L2UpdateChange
   {
      public OrderSide Side { get; set; }
      public decimal Price { get; set; }
      public decimal Size { get; set; }
   }

   public class L2UpdateChangeConverter : JsonConverter2<L2UpdateChange>
   {
      public override L2UpdateChange ReadJson(JsonReader reader, Type objectType, L2UpdateChange existingValue, bool hasExistingValue, JsonSerializer serializer)
      {
         var side = reader.ReadAsString();
         var price = reader.ReadAsDecimal();
         var size = reader.ReadAsDecimal();

         reader.Read();

         if( price is null ) throw new JsonReaderException("Price in L2 update change is unexpectedly null.");
         if( size is null) throw new JsonReaderException("Size in L2 update change is unexpectedly null.");

         var c = new L2UpdateChange
            {
               Price = price.Value,
               Size = size.Value
            };

         if( Enum.TryParse(side, ignoreCase: true, out OrderSide s) )
         {
            c.Side = s;
         }

         return c;
      }
   }



   public partial class ReceivedEvent : Event
   {
      [JsonProperty("time")]
      public DateTimeOffset Time { get; set; }

      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      [JsonProperty("sequence")]
      public long Sequence { get; set; }

      [JsonProperty("order_id")]
      public string OrderId { get; set; }

      [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
      public decimal? Size { get; set; }

      [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
      public decimal? Price { get; set; }

      [JsonProperty("side")]
      public OrderSide Side { get; set; }

      [JsonProperty("order_type")]
      public OrderType OrderType { get; set; }

      [JsonProperty("funds", NullValueHandling = NullValueHandling.Ignore)]
      public decimal? Funds { get; set; }

      [JsonProperty("client_oid", NullValueHandling = NullValueHandling.Ignore)]
      public Guid? ClientOid { get; set; }
   }


   public partial class AuthenticatedEvent : Event
   {
      [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
      public string UserId { get; set; }

      [JsonProperty("profile_id", NullValueHandling = NullValueHandling.Ignore)]
      public string ProfileId { get; set; }
   }

   public partial class OpenEvent : AuthenticatedEvent
   {
      [JsonProperty("time")]
      public DateTimeOffset Time { get; set; }

      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      [JsonProperty("sequence")]
      public long Sequence { get; set; }

      [JsonProperty("order_id")]
      public string OrderId { get; set; }

      [JsonProperty("price")]
      public decimal Price { get; set; }

      [JsonProperty("remaining_size")]
      public decimal RemainingSize { get; set; }

      [JsonProperty("side")]
      public OrderSide Side { get; set; }
   }


   public partial class DoneEvent : AuthenticatedEvent
   {
      [JsonProperty("time")]
      public DateTimeOffset Time { get; set; }

      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      [JsonProperty("sequence")]
      public long Sequence { get; set; }

      [JsonProperty("price")]
      public decimal Price { get; set; }

      [JsonProperty("order_id")]
      public string OrderId { get; set; }

      [JsonProperty("reason")]
      public string Reason { get; set; }

      [JsonProperty("side")]
      public OrderSide Side { get; set; }

      [JsonProperty("remaining_size")]
      public decimal RemainingSize { get; set; }
   }


   public partial class MatchEvent : AuthenticatedEvent
   {
      [JsonProperty("trade_id")]
      public long TradeId { get; set; }

      [JsonProperty("sequence")]
      public long Sequence { get; set; }

      [JsonProperty("maker_order_id")]
      public string MakerOrderId { get; set; }

      [JsonProperty("taker_order_id")]
      public string TakerOrderId { get; set; }

      [JsonProperty("time")]
      public DateTimeOffset Time { get; set; }

      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      [JsonProperty("size")]
      public decimal Size { get; set; }

      [JsonProperty("price")]
      public decimal Price { get; set; }

      [JsonProperty("side")]
      public OrderSide Side { get; set; }
   }

   public partial class ChangeEvent : AuthenticatedEvent
   {
      [JsonProperty("time")]
      public DateTimeOffset Time { get; set; }

      [JsonProperty("sequence")]
      public long Sequence { get; set; }

      [JsonProperty("order_id")]
      public string OrderId { get; set; }

      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      [JsonProperty("new_size", NullValueHandling = NullValueHandling.Ignore)]
      public decimal? NewSize { get; set; }

      [JsonProperty("old_size", NullValueHandling = NullValueHandling.Ignore)]
      public decimal? OldSize { get; set; }

      [JsonProperty("price")]
      public decimal Price { get; set; }

      [JsonProperty("side")]
      public OrderSide Side { get; set; }

      [JsonProperty("new_funds", NullValueHandling = NullValueHandling.Ignore)]
      public decimal? NewFunds { get; set; }

      [JsonProperty("old_funds", NullValueHandling = NullValueHandling.Ignore)]
      public decimal? OldFunds { get; set; }
   }

   public partial class ActivateEvent : AuthenticatedEvent
   {
      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      [JsonProperty("timestamp")]
      public string Timestamp { get; set; }

      [JsonProperty("order_id")]
      public string OrderId { get; set; }

      [JsonProperty("stop_type")]
      public StopType StopType { get; set; }

      [JsonProperty("side")]
      public OrderSide Side { get; set; }

      [JsonProperty("stop_price")]
      public decimal StopPrice { get; set; }

      [JsonProperty("size")]
      public decimal Size { get; set; }

      [JsonProperty("funds")]
      public decimal Funds { get; set; }

      [JsonProperty("taker_fee_rate")]
      public decimal TakerFeeRate { get; set; }

      [JsonProperty("private")]
      public bool Private { get; set; }
   }


}
