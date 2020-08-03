using System;
using System.Linq;
using System.Runtime.Serialization;
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
      public decimal? MaxMarketFunds { get; set; }

      [JsonProperty("min_market_funds")]
      public decimal? MinMarketFunds { get; set; }

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

         if (reader.Value is long l)
         {
            obe.OrderCount = l;
         }
         else if ( reader.Value is string strGuid )
         {
            obe.OrderId = strGuid;
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
      public string OrderId { get; set; }
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
      /// <summary>
      /// Account Id
      /// </summary>
      [JsonProperty("id")]
      public string Id { get; set; }

      /// <summary>
      /// the currency of the account
      /// </summary>
      [JsonProperty("currency")]
      public string Currency { get; set; }

      /// <summary>
      /// total funds in the account
      /// </summary>
      [JsonProperty("balance")]
      public decimal Balance { get; set; }

      /// <summary>
      /// funds available to withdraw or trade
      /// </summary>
      [JsonProperty("available")]
      public decimal Available { get; set; }

      /// <summary>
      /// funds on hold (not available for use).
      /// When you place an order, the funds for the order are placed on hold. They cannot be used for other orders or withdrawn. Funds will remain on hold until the order is filled or canceled
      /// </summary>
      [JsonProperty("hold")]
      public decimal Hold { get; set; }

      [JsonProperty("profile_id")]
      public string ProfileId { get; set; }
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
      public string TransferId { get; set; }

      [JsonProperty("transfer_type")]
      public string TransferType { get; set; }

      [JsonProperty("order_id")]
      public string OrderId { get; set; }

      [JsonProperty("trade_id")]
      public string TradeId { get; set; }

      public string ProductId { get; set; }
   }

   public partial class AccountHold : Json
   {
      [JsonProperty("id")]
      public string Id { get; set; }

      [JsonProperty("account_id")]
      public string AccountId { get; set; }

      [JsonProperty("created_at")]
      public DateTimeOffset CreatedAt { get; set; }

      [JsonProperty("updated_at")]
      public DateTimeOffset UpdatedAt { get; set; }

      [JsonProperty("amount")]
      public string Amount { get; set; }

      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("ref")]
      public string Ref { get; set; }
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
      public string Id { get; set; }

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


   public partial class Fill : Json
   {
      [JsonProperty("created_at")]
      public DateTimeOffset CreatedAt { get; set; }

      [JsonProperty("trade_id")]
      public long TradeId { get; set; }

      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      [JsonProperty("order_id")]
      public string OrderId { get; set; }

      [JsonProperty("user_id")]
      public string UserId { get; set; }

      [JsonProperty("profile_id")]
      public string ProfileId { get; set; }

      [JsonProperty("liquidity")]
      public FillLiquidity Liquidity { get; set; }

      [JsonProperty("price")]
      public decimal Price { get; set; }

      [JsonProperty("size")]
      public string Size { get; set; }

      [JsonProperty("fee")]
      public decimal Fee { get; set; }

      [JsonProperty("side")]
      public OrderSide Side { get; set; }

      [JsonProperty("settled")]
      public bool Settled { get; set; }

      [JsonProperty("usd_volume")]
      public decimal? UsdVolume { get; set; }
   }

   /// <summary>
   /// The liquidity field indicates if the fill was the result of a
   /// liquidity provider or liquidity taker. M indicates Maker and T
   /// indicates Taker.
   /// </summary>
   [JsonConverter(typeof(StringEnumConverter))]
   public enum FillLiquidity
   {
      [EnumMember(Value = "M")]
      Maker,

      [EnumMember(Value = "T")]
      Taker
   }


   public partial class PaymentMethodDeposit : Json
   {
      [JsonProperty("id")]
      public string Id { get; set; }

      [JsonProperty("amount")]
      public decimal Amount { get; set; }

      [JsonProperty("currency")]
      public string Currency { get; set; }

      [JsonProperty("payout_at")]
      public DateTimeOffset PayoutAt { get; set; }
   }

   public partial class CoinbaseDeposit : Json
   {
      [JsonProperty("id")]
      public string Id { get; set; }

      [JsonProperty("amount")]
      public decimal Amount { get; set; }

      [JsonProperty("currency")]
      public string Currency { get; set; }
   }


   public class PaymentMethodWithdraw : Json
   {
      [JsonProperty("id")]
      public string Id { get; set; }

      [JsonProperty("amount")]
      public decimal Amount { get; set; }

      [JsonProperty("currency")]
      public string Currency { get; set; }

      [JsonProperty("payout_at")]
      public DateTimeOffset PayoutAt { get; set; }
   }

   public class CoinbaseWithdraw : Json
   {
      [JsonProperty("id")]
      public string Id { get; set; }

      [JsonProperty("amount")]
      public decimal Amount { get; set; }

      [JsonProperty("currency")]
      public string Currency { get; set; }
   }

   public class CryptoWithdraw : Json
   {
      [JsonProperty("id")]
      public string Id { get; set; }

      [JsonProperty("amount")]
      public decimal Amount { get; set; }

      [JsonProperty("currency")]
      public string Currency { get; set; }
   }


   public class Withdrawal : Json
   {
      [JsonProperty("id")]
      public string Id { get; set; }

      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("created_at")]
      public DateTime? CreatedAt { get; set; }

      [JsonProperty("completed_at")]
      public DateTime? CompletedAt { get; set; }

      [JsonProperty("canceled_at")]
      public DateTime? CancelledAt { get; set; }

      [JsonProperty("processed_at")]
      public DateTime? ProcessedAt { get; set; }

      [JsonProperty("account_id")]
      public string AccountId { get; set; }

      [JsonProperty("user_id")]
      public string UserId { get; set; }

      [JsonProperty("user_nonce")]
      public string UserNonce { get; set; }

      [JsonProperty("amount")]
      public decimal Amount { get; set; }

      [JsonProperty("details")]
      public Details Details { get; set; }
   }

   public class Details : Json
   {
      [JsonProperty("destination_tag")]
      public string DestinationTag { get; set; }

      [JsonProperty("sent_to_address")]
      public string SentToAddress { get; set; }

      [JsonProperty("coinbase_account_id")]
      public string CoinbaseAccountId { get; set; }

      [JsonProperty("destination_tag_name")]
      public string DestinationTagName { get; set; }

      [JsonProperty("coinbase_withdrawal_id")]
      public string CoinbaseWithdrawalId { get; set; }

      [JsonProperty("coinbase_transaction_id")]
      public string CoinbaseTransactionId { get; set; }

      [JsonProperty("crypto_transaction_hash")]
      public string CryptoTransactionHash { get; set; }

      [JsonProperty("coinbase_payment_method_id")]
      public string CoinbasePaymentMethodId { get; set; }
   }

   public partial class Conversion
   {
      [JsonProperty("id")]
      public string Id { get; set; }

      [JsonProperty("amount")]
      public decimal Amount { get; set; }

      [JsonProperty("from_account_id")]
      public string FromAccountId { get; set; }

      [JsonProperty("to_account_id")]
      public string ToAccountId { get; set; }

      [JsonProperty("from")]
      public string From { get; set; }

      [JsonProperty("to")]
      public string To { get; set; }
   }

   public partial class PaymentMethod : Json
   {
      [JsonProperty("id")]
      public string Id { get; set; }

      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("currency")]
      public string Currency { get; set; }

      [JsonProperty("primary_buy")]
      public bool PrimaryBuy { get; set; }

      [JsonProperty("primary_sell")]
      public bool PrimarySell { get; set; }

      [JsonProperty("allow_buy")]
      public bool AllowBuy { get; set; }

      [JsonProperty("allow_sell")]
      public bool AllowSell { get; set; }

      [JsonProperty("allow_deposit")]
      public bool AllowDeposit { get; set; }

      [JsonProperty("allow_withdraw")]
      public bool AllowWithdraw { get; set; }

      [JsonProperty("limits")]
      public Limits Limits { get; set; }
   }

   public partial class Limits : Json
   {
      [JsonProperty("buy")]
      public Limit[] Buy { get; set; }

      [JsonProperty("instant_buy")]
      public Limit[] InstantBuy { get; set; }

      [JsonProperty("sell")]
      public Limit[] Sell { get; set; }

      [JsonProperty("deposit")]
      public Limit[] Deposit { get; set; }
   }

   public partial class Limit : Json
   {
      [JsonProperty("period_in_days")]
      public long PeriodInDays { get; set; }

      [JsonProperty("total")]
      public Money Total { get; set; }

      [JsonProperty("remaining")]
      public Money Remaining { get; set; }
   }

   public partial class Money : Json
   {
      [JsonProperty("amount")]
      public decimal Amount { get; set; }

      [JsonProperty("currency")]
      public string Currency { get; set; }
   }






   public partial class CoinbaseAccount : Json
   {
      [JsonProperty("id")]
      public string Id { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("balance")]
      public decimal Balance { get; set; }

      [JsonProperty("currency")]
      public string Currency { get; set; }

      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("primary")]
      public bool Primary { get; set; }

      [JsonProperty("active")]
      public bool Active { get; set; }

      [JsonProperty("wire_deposit_information", NullValueHandling = NullValueHandling.Ignore)]
      public WireDepositInformation WireDepositInformation { get; set; }

      [JsonProperty("sepa_deposit_information", NullValueHandling = NullValueHandling.Ignore)]
      public SepaDepositInformation SepaDepositInformation { get; set; }
   }

   public static class ExtensionsForCoinbaseAccount
   {
      public static bool IsWallet(this CoinbaseAccount ca)
      {
         return ca.Type == "wallet";
      }

      public static bool IsFiat(this CoinbaseAccount ca)
      {
         return ca.Type == "fiat";
      }

      public static bool IsMultisig(this CoinbaseAccount ca)
      {
         return ca.Type == "multisig";
      }

      public static bool IsVault(this CoinbaseAccount ca)
      {
         return ca.Type == "vault";
      }

      public static bool IsMultiSigVault(this CoinbaseAccount ca)
      {
         return ca.Type == "multisig_vault";
      }
   }

   public partial class WireDepositInformation : Json
   {
      [JsonProperty("account_number")]
      public string AccountNumber { get; set; }

      [JsonProperty("routing_number")]
      public string RoutingNumber { get; set; }

      [JsonProperty("bank_name")]
      public string BankName { get; set; }

      [JsonProperty("bank_address")]
      public string BankAddress { get; set; }

      [JsonProperty("bank_country")]
      public BankCountry BankCountry { get; set; }

      [JsonProperty("account_name")]
      public string AccountName { get; set; }

      [JsonProperty("account_address")]
      public string AccountAddress { get; set; }

      [JsonProperty("reference")]
      public string Reference { get; set; }
   }

   public partial class SepaDepositInformation : Json
   {
      [JsonProperty("iban")]
      public string Iban { get; set; }

      [JsonProperty("swift")]
      public string Swift { get; set; }

      [JsonProperty("bank_name")]
      public string BankName { get; set; }

      [JsonProperty("bank_address")]
      public string BankAddress { get; set; }

      [JsonProperty("bank_country_name")]
      public string BankCountryName { get; set; }

      [JsonProperty("account_name")]
      public string AccountName { get; set; }

      [JsonProperty("account_address")]
      public string AccountAddress { get; set; }

      [JsonProperty("reference")]
      public string Reference { get; set; }
   }

   public partial class BankCountry : Json
   {
      [JsonProperty("code")]
      public string Code { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }
   }



   public partial class Report : Json
   {
      [JsonProperty("id")]
      public string Id { get; set; }

      [JsonProperty("type")]
      public ReportType Type { get; set; }

      [JsonProperty("status")]
      public string Status { get; set; }

      [JsonProperty("created_at")]
      public DateTimeOffset CreatedAt { get; set; }

      [JsonProperty("completed_at")]
      public DateTimeOffset? CompletedAt { get; set; }

      [JsonProperty("expires_at")]
      public DateTimeOffset ExpiresAt { get; set; }

      [JsonProperty("file_url")]
      public string FileUrl { get; set; }

      [JsonProperty("params")]
      public ReportParams Params { get; set; }
   }

   public partial class ReportParams : Json
   {
      [JsonProperty("start_date")]
      public DateTimeOffset StartDate { get; set; }

      [JsonProperty("end_date")]
      public DateTimeOffset EndDate { get; set; }
   }


   [JsonConverter(typeof(StringEnumConverter))]
   public enum ReportType
   {
      [EnumMember(Value = "fills")]
      Fills,
      [EnumMember(Value = "account")]
      Account
   }

   public partial class TrailingVolume
   {
      [JsonProperty("product_id")]
      public string ProductId { get; set; }

      [JsonProperty("exchange_volume")]
      public decimal ExchangeVolume { get; set; }

      [JsonProperty("volume")]
      public decimal Volume { get; set; }

      [JsonProperty("recorded_at")]
      public DateTimeOffset RecordedAt { get; set; }
   }


}
