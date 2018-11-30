using System;
using System.Collections.Generic;
using Coinbase.Pro.Models;
using Newtonsoft.Json;

namespace Coinbase.Pro.Websockets.Models
{
   public partial class Subscriptions : Json
   {
      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("channels")]
      public Channel[] Channels { get; set; }
   }

   public partial class Heartbeat : Json
   {
      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("sequence")]
      public long Sequence { get; set; }

      [JsonProperty("last_trade_id")]
      public long LastTradeId { get; set; }

      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      [JsonProperty("time")]
      public DateTimeOffset Time { get; set; }
   }

   public partial class Ticker : Json
   {
      [JsonProperty("type")]
      public string Type { get; set; }

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
      public decimal BestBid { get; set; }

      [JsonProperty("best_ask")]
      public decimal BestAsk { get; set; }
   }


   public partial class Snapshot : Json
   {
      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      [JsonProperty("bids")]
      public List<decimal[]> Bids { get; set; }

      [JsonProperty("asks")]
      public List<decimal[]> Asks { get; set; }
   }

   public partial class L2Update : Json
   {
      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      [JsonProperty("changes")]
      public List<string[]> Changes { get; set; }
   }



   public partial class Received : Json
   {
      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("time")]
      public DateTimeOffset Time { get; set; }

      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      [JsonProperty("sequence")]
      public long Sequence { get; set; }

      [JsonProperty("order_id")]
      public Guid OrderId { get; set; }

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



   public partial class Open : Json
   {
      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("time")]
      public DateTimeOffset Time { get; set; }

      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      [JsonProperty("sequence")]
      public long Sequence { get; set; }

      [JsonProperty("order_id")]
      public Guid OrderId { get; set; }

      [JsonProperty("price")]
      public decimal Price { get; set; }

      [JsonProperty("remaining_size")]
      public decimal RemainingSize { get; set; }

      [JsonProperty("side")]
      public OrderSide Side { get; set; }
   }


   public partial class Done : Json
   {
      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("time")]
      public DateTimeOffset Time { get; set; }

      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      [JsonProperty("sequence")]
      public long Sequence { get; set; }

      [JsonProperty("price")]
      public decimal Price { get; set; }

      [JsonProperty("order_id")]
      public Guid OrderId { get; set; }

      [JsonProperty("reason")]
      public string Reason { get; set; }

      [JsonProperty("side")]
      public OrderSide Side { get; set; }

      [JsonProperty("remaining_size")]
      public decimal RemainingSize { get; set; }
   }


   public partial class Match : Json
   {
      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("trade_id")]
      public long TradeId { get; set; }

      [JsonProperty("sequence")]
      public long Sequence { get; set; }

      [JsonProperty("maker_order_id")]
      public Guid MakerOrderId { get; set; }

      [JsonProperty("taker_order_id")]
      public Guid TakerOrderId { get; set; }

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

   public partial class Change : Json
   {
      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("time")]
      public DateTimeOffset Time { get; set; }

      [JsonProperty("sequence")]
      public long Sequence { get; set; }

      [JsonProperty("order_id")]
      public Guid OrderId { get; set; }

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

   public partial class Activate : Json
   {
      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      [JsonProperty("timestamp")]
      public string Timestamp { get; set; }

      [JsonProperty("user_id")]
      public string UserId { get; set; }

      [JsonProperty("profile_id")]
      public Guid ProfileId { get; set; }

      [JsonProperty("order_id")]
      public Guid OrderId { get; set; }

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
