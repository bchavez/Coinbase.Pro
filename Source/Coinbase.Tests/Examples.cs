using System.IO;
using System.Net.Http;
using Flurl.Http.Testing;

namespace Coinbase.Tests
{
   public class Examples
   {
      public static string ReadJsonFile(string jsonFile) => File.ReadAllText($@"..\..\Json\{jsonFile}");

      public static string ProductsJson => ReadJsonFile("Products.json");

      public const string OrderBookLevel1Json = @"{
    ""asks"": [
        [
            ""3967.56"",
            ""0.001"",
            1
        ]
    ],
    ""bids"": [
        [
            ""3967.55"",
            ""57.6603824"",
            12
        ]
    ],
    ""sequence"": 7416643032
}
";
      public static string OrderBookLevel2Json => ReadJsonFile("OrderBookLevel2.json");
      public static string OrderBookLevel3Json => ReadJsonFile("OrderBookLevel3.json");
      public static string Currencies => ReadJsonFile("Currencies.json");
      public const string StatsJson = @"{
    ""high"": ""4072.95000000"",
    ""last"": ""3999.48000000"",
    ""low"": ""3562.90000000"",
    ""open"": ""3655.56000000"",
    ""volume"": ""29345.39170255"",
    ""volume_30day"": ""447043.36757766""
}
";

      public const string TickerJson = @"{
    ""ask"": ""4027.95"",
    ""bid"": ""4027.82"",
    ""price"": ""4029.70000000"",
    ""size"": ""0.00100000"",
    ""time"": ""2018-11-26T07:13:57.089000Z"",
    ""trade_id"": 54705877,
    ""volume"": ""32218.97638837""
}
";

      public const string TradesJson = @"[
    {
        ""price"": ""4013.64000000"",
        ""side"": ""sell"",
        ""size"": ""0.00842855"",
        ""time"": ""2018-11-26T07:23:53.243Z"",
        ""trade_id"": 54706228
    },
    {
        ""price"": ""4013.65000000"",
        ""side"": ""sell"",
        ""size"": ""0.00458841"",
        ""time"": ""2018-11-26T07:23:52.332Z"",
        ""trade_id"": 54706227
    },
    {
        ""price"": ""4013.89000000"",
        ""side"": ""sell"",
        ""size"": ""0.10817589"",
        ""time"": ""2018-11-26T07:23:49.649Z"",
        ""trade_id"": 54706226
    }
]
";

      public const string HistoricRatesJson = @"[
    [
        1543372800,
        3820,
        3826.76,
        3826.76,
        3823.83,
        2.36
    ],
    [
        1543372740,
        3824.97,
        3828.26,
        3824.97,
        3826.76,
        3.74
    ],
    [
        1543372680,
        3820.83,
        3824.97,
        3820.83,
        3824.97,
        11.06
    ],

]";

      public const string Account1Json = @"{
        ""id"": ""71452118-efc7-4cc4-8780-a5e22d4baa53"",
        ""currency"": ""BTC"",
        ""balance"": ""0.0000000000000000"",
        ""available"": ""0.0000000000000000"",
        ""hold"": ""0.0000000000000000"",
        ""profile_id"": ""75da88c5-05bf-4f54-bc85-5c775bd68254""
    }";

      public const string AccountListJson = @"[
    {
        ""id"": ""71452118-efc7-4cc4-8780-a5e22d4baa53"",
        ""currency"": ""BTC"",
        ""balance"": ""0.0000000000000000"",
        ""available"": ""0.0000000000000000"",
        ""hold"": ""0.0000000000000000"",
        ""profile_id"": ""75da88c5-05bf-4f54-bc85-5c775bd68254""
    },
    {
        ""id"": ""e316cb9a-0808-4fd7-8914-97829c1925de"",
        ""currency"": ""USD"",
        ""balance"": ""80.2301373066930000"",
        ""available"": ""79.2266348066930000"",
        ""hold"": ""1.0035025000000000"",
        ""profile_id"": ""75da88c5-05bf-4f54-bc85-5c775bd68254""
    }
]";


      public const string AccountHistoryJson = @"[
  {
    ""created_at"": ""2018-10-25T18:33:58.614187Z"",
    ""id"": 44512583,
    ""amount"": ""1000.0000000000000000"",
    ""balance"": ""1000.0000000000000000"",
    ""type"": ""transfer"",
    ""details"": {
      ""transfer_id"": ""ffff2937-ffff-ffff-ffff-44ae9640ffff"",
      ""transfer_type"": ""deposit""
    }
  }
]";

      public const string AccountHoldJson = @"[
    {
        ""id"": ""82dcd140-c3c7-4507-8de4-2c529cd1a28f"",
        ""account_id"": ""e0b3f39a-183d-453e-b754-0c13e5bab0b3"",
        ""created_at"": ""2014-11-06T10:34:47.123456Z"",
        ""updated_at"": ""2014-11-06T10:40:47.123456Z"",
        ""amount"": ""4.23"",
        ""type"": ""order"",
        ""ref"": ""0a205de4-dd35-4370-a285-fe8fc375a273"",
    }
]";

      public const string OrdersListJson = @"[
  {
    ""id"": ""dba30426-4ffd-4068-945c-21e1b9731a5b"",
    ""price"": ""1.10000000"",
    ""size"": ""100.00000000"",
    ""product_id"": ""ETC-USD"",
    ""side"": ""buy"",
    ""type"": ""limit"",
    ""time_in_force"": ""GTC"",
    ""post_only"": true,
    ""created_at"": ""2018-11-28T21:55:49.598587Z"",
    ""fill_fees"": ""0.0000000000000000"",
    ""filled_size"": ""0.00000000"",
    ""executed_value"": ""0.0000000000000000"",
    ""status"": ""open"",
    ""settled"": false
  },
  {
    ""id"": ""e0c163ab-5823-4972-b9fb-9e788687a53b"",
    ""product_id"": ""ETC-USD"",
    ""side"": ""buy"",
    ""funds"": ""99.7008973000000000"",
    ""specified_funds"": ""100.0000000000000000"",
    ""type"": ""market"",
    ""post_only"": false,
    ""created_at"": ""2018-11-28T21:55:12.049031Z"",
    ""done_at"": ""2018-11-28T21:55:12.079Z"",
    ""done_reason"": ""filled"",
    ""fill_fees"": ""0.2991026910000000"",
    ""filled_size"": ""1.99401794"",
    ""executed_value"": ""99.7008970000000000"",
    ""status"": ""done"",
    ""settled"": true
  }
]";

      public const string Order1Json = @"{
    ""id"": ""e0c163ab-5823-4972-b9fb-9e788687a53b"",
    ""product_id"": ""ETC-USD"",
    ""side"": ""buy"",
    ""funds"": ""99.7008973000000000"",
    ""specified_funds"": ""100.0000000000000000"",
    ""type"": ""market"",
    ""post_only"": false,
    ""created_at"": ""2018-11-28T21:55:12.049031Z"",
    ""done_at"": ""2018-11-28T21:55:12.079Z"",
    ""done_reason"": ""filled"",
    ""fill_fees"": ""0.2991026910000000"",
    ""filled_size"": ""1.99401794"",
    ""executed_value"": ""99.7008970000000000"",
    ""status"": ""done"",
    ""settled"": true
  }";

      public const string CancellAllOrdersJson = @"[
    ""144c6f8e-713f-4682-8435-5280fbe8b2b4"",
    ""debe4907-95dc-442f-af3b-cec12f42ebda"",
    ""cf7aceee-7b08-4227-a76c-3858144323ab"",
    ""dfc5ae27-cadb-4c0c-beef-8994936fde8a"",
    ""34fecfbf-de33-4273-b2c6-baf8e8948be4""
]";

      public const string CancelOrderByIdJson = @"[""2eb2992c-d795-442c-847a-88b60ebc64cb""]";

      public const string Fills1Json = @"[
   {
      ""created_at"": ""2018-11-28T21:55:12.079Z"",
      ""trade_id"": 59,
      ""product_id"": ""ETC-USD"",
      ""order_id"": ""ca1e85cc-6b7b-42c2-905d-328887226197"",
      ""user_id"": ""fffffffffffffffff"",
      ""profile_id"": ""AD0E45D7-B0C8-4138-AC5B-81640E3B581B"",
      ""liquidity"": ""T"",
      ""price"": ""50.00000000"",
      ""size"": ""1.99401794"",
      ""fee"": ""0.2991026910000000"",
      ""side"": ""buy"",
      ""settled"": true,
      ""usd_volume"": ""99.7008970000000000""
   }
]";

      public const string DepositPaymentMethodFundsJson = @"{
    ""id"": ""593533d2-ff31-46e0-b22e-ca754147a96a"",
    ""amount"": ""10.00"",
    ""currency"": ""USD"",
    ""payout_at"": ""2016-08-20T00:31:09Z""
}";

      public const string DepositCoinbaseFundsJson = @"{
    ""id"": ""593533d2-ff31-46e0-b22e-ca754147a96a"",
    ""amount"": ""10.00"",
    ""currency"": ""BTC"",
}";

      public const string WithdrawPaymentMethodFundsJson = @"{
    ""id"": ""593533d2-ff31-46e0-b22e-ca754147a96a"",
    ""amount"": ""10.00"",
    ""currency"": ""USD"",
    ""payout_at"": ""2016-08-20T00:31:09Z""
}";

      public const string WithdrawCoinbaseFundsJson = @"{
    ""id"": ""593533d2-ff31-46e0-b22e-ca754147a96a"",
    ""amount"": ""10.00"",
    ""currency"": ""BTC"",
}";


      public const string WithdrawCryptoFundsJson = @"{
    ""id"": ""593533d2-ff31-46e0-b22e-ca754147a96a"",
    ""amount"": ""10.00"",
    ""currency"": ""ETH"",
}";

      public const string ConversionJson = @"{
    ""id"": ""8942caee-f9d5-4600-a894-4811268545db"",
    ""amount"": ""10000.00"",
    ""from_account_id"": ""7849cc79-8b01-4793-9345-bc6b5f08acce"",
    ""to_account_id"": ""105c3e58-0898-4106-8283-dc5781cda07b"",
    ""from"": ""USD"",
    ""to"": ""USDC""
}";






      public static string PaymentMethodsJson => ReadJsonFile("PaymentMethods.json");

      public static string CoinbaseAccountsJson => ReadJsonFile("CoinbaseAccounts.json");



      public const string ReportCompletedJson = @"{
   ""created_at"": ""2018-11-29T23:22:26.952249Z"",
   ""completed_at"": ""2018-11-29T23:22:27.759Z"",
   ""expires_at"": ""2018-12-06T23:22:26.952249Z"",
   ""id"": ""D94DF955-4A2C-4C08-80EA-CDA8249ED099"",
   ""type"": ""fills"",
   ""status"": ""ready"",
   ""user_id"": ""ffffffffffffffff"",
   ""file_url"": ""https://gdax-reports-sandbox.s3.amazonaws.com/fffffffffile.pdf"",
   ""params"": {
      ""start_date"": ""2018-01-01T00:00:00-08:00"",
      ""end_date"": ""2019-01-01T00:00:00-08:00"",
      ""format"": ""pdf"",
      ""product_id"": ""ETC-USD"",
      ""user"": {
         ""created_at"": ""2018-10-25T18:30:26.504993Z"",
         ""active_at"": ""2018-10-25T18:30:26.531Z"",
         ""id"": ""ffffffffffffffffff"",
         ""name"": ""Satoshi Moshi"",
         ""email"": ""satoshi@bitcoin.com"",
         ""roles"": null,
         ""is_banned"": false,
         ""permissions"": null,
         ""user_type"": ""business"",
         ""fulfills_new_requirements"": true,
         ""flags"": null,
         ""details"": null,
         ""oauth_client"": ""pro"",
         ""preferences"": {},
         ""has_default"": false
      },
      ""new_york_state"": false
   }
}";

      public const string ReportUnfinishedJson = @"{
   ""id"": ""CE5B72BB-673D-4161-8D29-182D715F6556"",
   ""type"": ""fills"",
   ""status"": ""pending""
}";


      public const string TrailingVolumeJson = @"[
    {
        ""product_id"": ""BTC-USD"",
        ""exchange_volume"": ""11800.00000000"",
        ""volume"": ""100.00000000"",
        ""recorded_at"": ""1973-11-29T00:05:01.123456Z""
    },
    {
        ""product_id"": ""LTC-USD"",
        ""exchange_volume"": ""51010.04100000"",
        ""volume"": ""2010.04100000"",
        ""recorded_at"": ""1973-11-29T00:05:02.123456Z""
    }
]";



   }

   
}
