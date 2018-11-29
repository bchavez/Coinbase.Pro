using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Flurl.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Coinbase.Pro.Models
{
   public partial class Product : Json
   {
      [JsonProperty("base_currency")]
      public string BaseCurrency { get; set; }

      [JsonProperty("base_max_size")]
      public decimal BaseMaxSize { get; set; }

      [JsonProperty("base_min_size")]
      public decimal BaseMinSize { get; set; }

      [JsonProperty("cancel_only")]
      public bool CancelOnly { get; set; }

      [JsonProperty("display_name")]
      public string DisplayName { get; set; }

      [JsonProperty("id")]
      public string Id { get; set; }

      [JsonProperty("limit_only")]
      public bool LimitOnly { get; set; }

      [JsonProperty("margin_enabled")]
      public bool MarginEnabled { get; set; }

      [JsonProperty("max_market_funds")]
      public decimal MaxMarketFunds { get; set; }

      [JsonProperty("min_market_funds")]
      public decimal MinMarketFunds { get; set; }

      [JsonProperty("post_only")]
      public bool PostOnly { get; set; }

      [JsonProperty("quote_currency")]
      public string QuoteCurrency { get; set; }

      [JsonProperty("quote_increment")]
      public decimal QuoteIncrement { get; set; }

      [JsonProperty("status")]
      public string Status { get; set; }

      [JsonProperty("status_message")]
      public object StatusMessage { get; set; }
   }


   public partial class OrderBook : Json
   {
      [JsonProperty("asks", ItemConverterType = typeof(OrderBookItemConverter))]
      public OrderBookEntry[] Asks { get; set; }

      [JsonProperty("bids", ItemConverterType = typeof(OrderBookItemConverter))]
      public OrderBookEntry[] Bids { get; set; }

      [JsonProperty("sequence")]
      public long Sequence { get; set; }
   }

   public abstract class JsonConverter2<T> : JsonConverter<T>
   {
      public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
      {
         serializer.Serialize(writer, value);
      }
   }

   public class OrderBookItemConverter : JsonConverter2<OrderBookEntry>
   {
      public override OrderBookEntry ReadJson(JsonReader reader, Type objectType, OrderBookEntry existingValue, bool hasExistingValue, JsonSerializer serializer)
      {
         var price = reader.ReadAsDecimal();
         var size = reader.ReadAsDecimal();

         var obe = new OrderBookEntry
            {
               Price = price.Value,
               Size = size.Value,
            };

         reader.Read();

         if( reader.Value is string strGuid && Guid.TryParse(strGuid, out var guid) )
         {
            obe.OrderId = guid;
         }
         else if( reader.Value is long l)
         {
            obe.OrderCount = l;
         }

         reader.Read();

         return obe;
      }
   }

   public partial class OrderBookEntry
   {
      [JsonProperty(Order = 1)]
      public decimal Price { get; set; }
      [JsonProperty(Order = 2)]
      public decimal Size { get; set; }
      [JsonProperty(Order = 3, NullValueHandling = NullValueHandling.Ignore)]
      public long? OrderCount { get; set; }
      [JsonProperty(Order = 3, NullValueHandling = NullValueHandling.Ignore)]
      public Guid? OrderId { get; set; }
   }


   public partial class Ticker
   {
      [JsonProperty("ask")]
      public decimal Ask { get; set; }

      [JsonProperty("bid")]
      public decimal Bid { get; set; }

      [JsonProperty("price")]
      public decimal Price { get; set; }

      [JsonProperty("size")]
      public decimal Size { get; set; }

      [JsonProperty("time")]
      public DateTimeOffset Time { get; set; }

      [JsonProperty("trade_id")]
      public long TradeId { get; set; }

      [JsonProperty("volume")]
      public decimal Volume { get; set; }
   }


   public partial class Trade 
   {
      [JsonProperty("price")]
      public decimal Price { get; set; }

      [JsonProperty("side")]
      public OrderSide Side { get; set; }

      [JsonProperty("size")]
      public decimal Size { get; set; }

      [JsonProperty("time")]
      public DateTimeOffset Time { get; set; }

      [JsonProperty("trade_id")]
      public long TradeId { get; set; }
   }

   [JsonConverter(typeof(StringEnumConverter))]
   public enum OrderSide
   {
      [EnumMember(Value = "buy")]
      Buy,

      [EnumMember(Value = "sell")]
      Sell
   }

   [JsonConverter(typeof(CandleConverter))]
   public class Candle
   {
      [JsonProperty(Order = 1)]
      public DateTimeOffset Time { get; set; }

      [JsonProperty(Order = 2)]
      public decimal? Low { get; set; }

      [JsonProperty(Order = 3)]
      public decimal? High { get; set; }

      [JsonProperty(Order = 4)]
      public decimal? Open { get; set; }

      [JsonProperty(Order = 5)]
      public decimal? Close { get; set; }

      [JsonProperty(Order = 6)]
      public decimal? Volume { get; set; }
   }

   public class CandleConverter : JsonConverter<Candle>
   {
      public override void WriteJson(JsonWriter writer, Candle value, JsonSerializer serializer)
      {
         writer.WriteStartObject();
         writer.WritePropertyName("time");
         writer.WriteValue(value.Time);
         writer.WritePropertyName("low");
         writer.WriteValue(value.Low);
         writer.WritePropertyName("high");
         writer.WriteValue(value.High);
         writer.WritePropertyName("open");
         writer.WriteValue(value.Open);
         writer.WritePropertyName("close");
         writer.WriteValue(value.Close);
         writer.WritePropertyName("volume");
         writer.WriteValue(value.Volume);
         writer.WriteEndObject();

      }

      public override Candle ReadJson(JsonReader reader, Type objectType, Candle existingValue, bool hasExistingValue, JsonSerializer serializer)
      {
         var j = JArray.Load(reader);

         var time = j[0].Value<int>();
         var low = j.ElementAtOrDefault(1)?.Value<decimal?>();
         var high = j.ElementAtOrDefault(2)?.Value<decimal?>();
         var open = j.ElementAtOrDefault(3)?.Value<decimal?>();
         var close = j.ElementAtOrDefault(4)?.Value<decimal?>();
         var vol = j.ElementAtOrDefault(5)?.Value<decimal?>();

         var c = new Candle
            {
               Time = TimeHelper.FromUnixTimestampSeconds(time),
               Low = low,
               High = high,
               Open = open,
               Close = close,
               Volume = vol
            };

         return c;
      }
   }

   public partial class Stats : Json
   {
      [JsonProperty("high")]
      public decimal High { get; set; }

      [JsonProperty("last")]
      public decimal Last { get; set; }

      [JsonProperty("low")]
      public decimal Low { get; set; }

      [JsonProperty("open")]
      public decimal Open { get; set; }

      [JsonProperty("volume")]
      public decimal Volume { get; set; }

      [JsonProperty("volume_30day")]
      public decimal Volume30Day { get; set; }
   }

   public partial class Currency : Json
   {
      [JsonProperty("details")]
      public JObject Details { get; set; }

      [JsonProperty("id")]
      public string Id { get; set; }

      [JsonProperty("message")]
      public string Message { get; set; }

      [JsonProperty("min_size")]
      public decimal MinSize { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("status")]
      public string Status { get; set; }

      [JsonProperty("convertible_to", NullValueHandling = NullValueHandling.Ignore)]
      public string[] ConvertibleTo { get; set; }
   }

   public partial class Time : Json
   {
      [JsonProperty("iso")]
      public DateTimeOffset Iso { get; set; }

      [JsonProperty("epoch")]
      public long Epoch { get; set; }
   }


   public partial class Account : Json
   {
      [JsonProperty("id")]
      public Guid Id { get; set; }

      [JsonProperty("currency")]
      public string Currency { get; set; }

      [JsonProperty("balance")]
      public decimal Balance { get; set; }

      [JsonProperty("available")]
      public decimal Available { get; set; }

      [JsonProperty("hold")]
      public decimal Hold { get; set; }

      [JsonProperty("profile_id")]
      public Guid ProfileId { get; set; }
   }

   public class AccountHistory : Json
   {
      [JsonProperty("id")]
      public string Id { get; set; }

      [JsonProperty("created_at")]
      public DateTimeOffset CreatedAt { get; set; }

      [JsonProperty("amount")]
      public decimal Amount { get; set; }

      [JsonProperty("balance")]
      public decimal Balance { get; set; }

      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("details")]
      public AccountDetails Details { get; set; }
   }

   public partial class AccountDetails : Json
   {
      [JsonProperty("transfer_id")]
      public Guid? TransferId { get; set; }

      [JsonProperty("transfer_type")]
      public string TransferType { get; set; }

      [JsonProperty("order_id")]
      public Guid? OrderId { get; set; }

      [JsonProperty("trade_id")]
      public string TradeId { get; set; }

      public string ProductId { get; set; }
   }

   public partial class AccountHold : Json
   {
      [JsonProperty("id")]
      public Guid Id { get; set; }

      [JsonProperty("account_id")]
      public Guid AccountId { get; set; }

      [JsonProperty("created_at")]
      public DateTimeOffset CreatedAt { get; set; }

      [JsonProperty("updated_at")]
      public DateTimeOffset UpdatedAt { get; set; }

      [JsonProperty("amount")]
      public string Amount { get; set; }

      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("ref")]
      public Guid Ref { get; set; }
   }

   public partial class OrderRequest : Json
   {
      [JsonProperty("size")]
      public string Size { get; set; }

      [JsonProperty("price")]
      public string Price { get; set; }

      [JsonProperty("side")]
      public OrderSide Side { get; set; }

      [JsonProperty("product_id")]
      public string ProductId { get; set; }
   }

   public partial class Order : Json
   {
      [JsonProperty("id")]
      public Guid Id { get; set; }

      [JsonProperty("price")]
      public decimal Price { get; set; }

      [JsonProperty("size")]
      public decimal Size { get; set; }

      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      [JsonProperty("side")]
      public OrderSide Side { get; set; }

      /// <summary>
      /// Self-trading is not allowed on Coinbase Pro. Two orders from the
      /// same user will not fill one another. When placing an order,
      /// you can specify the self-trade prevention behavior.
      /// </summary>
      [JsonProperty("stp")]
      public SelfTradePrevention Stp { get; set; }

      /// <summary>
      /// When placing an order, you can specify the order type. The order type
      /// you specify will influence which other order parameters are required
      /// as well as how your order will be executed by the matching engine.
      /// If type is not specified, the order will default to a limit order.
      /// </summary>
      [JsonProperty("type")]
      public OrderType Type { get; set; }

      [JsonProperty("time_in_force")]
      public TimeInForce TimeInForce { get; set; }

      [JsonProperty("post_only")]
      public bool PostOnly { get; set; }

      [JsonProperty("created_at")]
      public DateTimeOffset CreatedAt { get; set; }

      [JsonProperty("fill_fees")]
      public decimal FillFees { get; set; }

      [JsonProperty("filled_size")]
      public decimal FilledSize { get; set; }

      [JsonProperty("executed_value")]
      public decimal ExecutedValue { get; set; }

      [JsonProperty("status")]
      public string Status { get; set; }

      [JsonProperty("settled")]
      public bool Settled { get; set; }

      [JsonProperty("funds", NullValueHandling = NullValueHandling.Ignore)]
      public decimal Funds { get; set; }

      [JsonProperty("specified_funds", NullValueHandling = NullValueHandling.Ignore)]
      public decimal SpecifiedFunds { get; set; }

      [JsonProperty("done_at", NullValueHandling = NullValueHandling.Ignore)]
      public DateTimeOffset? DoneAt { get; set; }

      [JsonProperty("done_reason", NullValueHandling = NullValueHandling.Ignore)]
      public string DoneReason { get; set; }
   }

   /// <summary>
   /// When placing an order, you can specify the order type. The order type
   /// you specify will influence which other order parameters are required
   /// as well as how your order will be executed by the matching engine.
   /// If type is not specified, the order will default to a limit order.
   /// </summary>
   [JsonConverter(typeof(StringEnumConverter))]
   public enum OrderType
   {
      /// <summary>
      /// Limit orders are both the default and basic order type. A limit
      /// order requires specifying a price and size. The size is the
      /// number of bitcoin to buy or sell, and the price is the price per
      /// bitcoin. The limit order will be filled at the price specified or
      /// better. A sell order can be filled at the specified price per
      /// bitcoin or a higher price per bitcoin and a buy order can be
      /// filled at the specified price or a lower price depending on
      /// market conditions. If market conditions cannot fill the limit
      /// order immediately, then the limit order will become part of
      /// the open order book until filled by another incoming order or
      /// canceled by the user.
      /// </summary>
      [EnumMember(Value = "limit")]
      Limit,

      /// <summary>
      /// market orders differ from limit orders in that
      /// they provide no pricing guarantees.
      /// They however do provide a way to buy or sell specific
      /// amounts of bitcoin or fiat without having to specify
      /// the price. Market orders execute immediately and no
      /// part of the market order will go on the open order book.
      /// Market orders are always considered takers and incur taker
      /// fees. When placing a market order you can specify funds
      /// and/or size. Funds will limit how much of your quote
      /// currency account balance is used and size will limit
      /// the bitcoin amount transacted.
      /// </summary>
      [EnumMember(Value = "market")]
      Market
   }

   /// <summary>
   /// Time in force policies provide guarantees about the lifetime of
   /// an order. There are four policies: good till canceled GTC, good
   /// till time GTT, immediate or cancel IOC, and fill or kill FOK
   /// </summary>
   [JsonConverter(typeof(StringEnumConverter))]
   public enum TimeInForce
   {
      /// <summary>
      /// Good till canceled orders remain open on the book until
      /// canceled. This is the default behavior if no policy is specified.
      /// </summary>
      [EnumMember(Value = "GTC")]
      GoodTillCanceled,

      /// <summary>
      /// Good till time orders remain open on the book until
      /// canceled or the allotted cancel_after is depleted on the
      /// matching engine. GTT orders are guaranteed to cancel before
      /// any other order is processed after the cancel_after timestamp
      /// which is returned by the API. A day is considered 24 hours.
      /// </summary>
      [EnumMember(Value = "GTT")]
      GoodTillTime,

      /// <summary>
      /// Immediate or cancel orders instantly cancel the remaining
      /// size of the limit order instead of opening it on the book.
      /// </summary>
      [EnumMember(Value = "IOC")]
      ImmediateOrCancel,

      /// <summary>
      /// Fill or kill orders are rejected if the entire size
      /// cannot be matched.
      /// </summary>
      [EnumMember(Value = "FOK")]
      FillOrKill
   }


   public partial class CreateOrder
   {
      [JsonProperty("client_oid", NullValueHandling = NullValueHandling.Ignore)]
      public Guid? ClientOid { get; set; }

      /// <summary>
      /// When placing an order, you can specify the order type. The order type
      /// you specify will influence which other order parameters are required
      /// as well as how your order will be executed by the matching engine.
      /// If type is not specified, the order will default to a limit order.
      /// </summary>
      [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
      public OrderType Type { get; set; }

      [JsonProperty("side")]
      public OrderSide Side { get; set; }

      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      /// <summary>
      /// Self-trading is not allowed on Coinbase Pro. Two orders from the
      /// same user will not fill one another. When placing an order,
      /// you can specify the self-trade prevention behavior.
      /// </summary>
      [JsonProperty("stp", NullValueHandling = NullValueHandling.Ignore)]
      public SelfTradePrevention? Stp { get; set; }

      /// <summary>
      /// Stop orders become active and wait to trigger based on the movement
      /// of the last trade price. There are two types of stop orders,
      /// stop loss and stop entry:
      /// </summary>
      [JsonProperty("stop", NullValueHandling = NullValueHandling.Ignore)]
      public StopType? Stop { get; set; }

      [JsonProperty("stop_price", NullValueHandling = NullValueHandling.Ignore)]
      public decimal? StopPrice { get; set; }

   }

   public partial class CreateLimitOrder : CreateOrder
   {
      [JsonProperty("price")]
      public decimal Price { get; set; }

      [JsonProperty("size")]
      public decimal Size { get; set; }

      [JsonProperty("time_in_force", NullValueHandling = NullValueHandling.Ignore)]
      public TimeInForce? TimeInForce { get; set; }

      [JsonProperty("cancel_after", NullValueHandling = NullValueHandling.Ignore)]
      public GoodTillTime? CancelAfter { get; set; }

      [JsonProperty("post_only", NullValueHandling = NullValueHandling.Ignore)]
      public bool? PostOnly { get; set; }
   }

   /// <summary>
   /// Market orders differ from limit orders in that they provide no pricing
   /// guarantees. They however do provide a way to buy or sell specific
   /// amounts of cryptocurrency or fiat without having to specify the price.
   /// Market orders execute immediately and no part of the market order
   /// will go on the open order book. Market orders are always considered
   /// takers and incur taker fees. When placing a market order you can
   /// specify funds and/or size. Funds will limit how much of your quote
   /// currency account balance is used and size will limit the cryptocurrency
   /// amount transacted.
   /// </summary>
   public partial class CreateMarketOrder : CreateOrder
   {
      /// <summary>
      /// Funds will limit how much of your quote currency account
      /// balance is used.
      /// </summary>
      [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
      public decimal? Size { get; set; }

      /// <summary>
      /// Size will limit the cryptocurrency amount transacted.
      /// </summary>
      [JsonProperty("funds", NullValueHandling = NullValueHandling.Ignore)]
      public decimal? Funds { get; set; }
   }

   /// <summary>
   /// When placing a market order you can specify funds and/or size.
   /// Funds will limit how much of your quote currency account balance
   /// is used and size will limit the cryptocurrency amount transacted.
   /// </summary>
   public enum AmountType
   {
      /// <summary>
      /// Size will limit the cryptocurrency amount transacted. The size must
      /// be greater than the base_min_size for the product and no larger
      /// than the base_max_size. The size can be in any increment of the
      /// base currency (BTC for the BTC-USD product), which includes satoshi
      /// units. size indicates the amount of BTC (or base currency) to buy or sell.
      ///
      /// A market sell order can also specify the funds. If funds is specified,
      /// it will limit the sell to the amount of funds specified. You can
      /// use funds with sell orders to limit the amount of quote
      /// currency funds received.
      /// </summary>
      UseSize,

      /// <summary>
      /// Funds will limit how much of your quote currency account balance
      /// is used. The funds field is optionally used for market orders.
      /// When specified it indicates how much of the product quote
      /// currency to buy or sell. For example, a market buy for
      /// BTC-USD with funds specified as 150.00 will spend
      /// 150 USD to buy BTC (including any fees). If the funds
      /// field is not specified for a market buy order, size
      /// must be specified and Coinbase Pro will use available
      /// funds in your account to buy bitcoin.
      /// </summary>
      UseFunds,
   }

   [JsonConverter(typeof(StringEnumConverter))]
   public enum GoodTillTime
   {
      [EnumMember(Value = "min")]
      Min,
      [EnumMember(Value = "hour")]
      Hour,
      [EnumMember(Value = "day")]
      Day
   }

   /// <summary>
   /// Stop orders become active and wait to trigger based on the
   /// movement of the last trade price. There are two types of stop
   /// orders, stop loss and stop entry:
   /// </summary>
   [JsonConverter(typeof(StringEnumConverter))]
   public enum StopType
   {
      /// <summary>
      /// stop: 'loss': Triggers when the last trade price changes to a value at or below the stop_price.
      /// </summary>
      [EnumMember(Value = "loss")]
      Loss,

      /// <summary>
      /// stop: 'entry': Triggers when the last trade price changes to a value at or above the stop_price.
      /// </summary>
      [EnumMember(Value = "entry")]
      Entry
   }

   /// <summary>
   /// Self-trading is not allowed on Coinbase Pro. Two orders from the
   /// same user will not fill one another. When placing an order,
   /// you can specify the self-trade prevention behavior.
   /// </summary>
   [JsonConverter(typeof(StringEnumConverter))]
   public enum SelfTradePrevention
   {
      /// <summary>
      /// The default behavior is decrement and cancel. When two orders from
      /// the same user cross, the smaller order will be canceled and the
      /// larger order size will be decremented by the smaller order size.
      /// If the two orders are the same size, both will be canceled.
      /// </summary>
      [EnumMember(Value = "dc")]
      DecreaseAndCancel,

      /// <summary>
      /// Cancel the older (resting) order in full. The new order
      /// continues to execute.
      /// </summary>
      [EnumMember(Value = "co")]
      CancelOldest,

      /// <summary>
      /// Cancel the newer (taking) order in full. The old resting order
      /// remains on the order book.
      /// </summary>
      [EnumMember(Value = "cn")]
      CancelNewest,

      /// <summary>
      /// Immediately cancel both orders.
      /// </summary>
      [EnumMember(Value = "cb")]
      CancelBoth
   }

}
