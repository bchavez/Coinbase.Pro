using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Coinbase.Pro.Models
{
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

   public partial class CreatePaymentMethodDeposit
   {
      [JsonProperty("amount")]
      public decimal Amount { get; set; }

      [JsonProperty("currency")]
      public string Currency { get; set; }

      [JsonProperty("payment_method_id")]
      public string PaymentMethodId { get; set; }
   }

   public partial class CreateCoinbaseDeposit
   {
      [JsonProperty("amount")]
      public decimal Amount { get; set; }

      [JsonProperty("currency")]
      public string Currency { get; set; }

      [JsonProperty("coinbase_account_id")]
      public string CoinbaseAccountId { get; set; }
   }

   public partial class CreatePaymentMethodWithdraw
   {
      [JsonProperty("amount")]
      public decimal Amount { get; set; }

      [JsonProperty("currency")]
      public string Currency { get; set; }

      [JsonProperty("payment_method_id")]
      public string PaymentMethodId { get; set; }
   }

   public class CreateCoinbaseWithdraw
   {
      [JsonProperty("amount")]
      public decimal Amount { get; set; }

      [JsonProperty("currency")]
      public string Currency { get; set; }

      [JsonProperty("coinbase_account_id")]
      public string CoinbaseAccountId { get; set; }
   }

   public class CreateCryptAddressWithdrawl
   {
      [JsonProperty("amount")]
      public decimal Amount { get; set; }

      [JsonProperty("currency")]
      public string Currency { get; set; }

      [JsonProperty("crypto_address")]
      public string CryptoAddress { get; set; }
   }


   public partial class CreateConversion
   {
      [JsonProperty("from")]
      public string From { get; set; }

      [JsonProperty("to")]
      public string To { get; set; }

      [JsonProperty("amount")]
      public decimal Amount { get; set; }
   }

}
