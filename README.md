[![Build status](https://ci.appveyor.com/api/projects/status/f9wf44xwah32s0le/branch/master?svg=true)](https://ci.appveyor.com/project/bchavez/coinbase-pro) [![Nuget](https://img.shields.io/nuget/v/Coinbase.Pro.svg)](https://www.nuget.org/packages/Coinbase.Pro/) [![Users](https://img.shields.io/nuget/dt/Coinbase.Pro.svg)](https://www.nuget.org/packages/Coinbase.Pro/) <img src="https://raw.githubusercontent.com/bchavez/Coinbase.Pro/master/Docs/coinbase_pro.png" align='right' />

Coinbase.Pro for .NET/C# Library
======================

Project Description
-------------------
A .NET implementation for the [Coinbase Pro API](https://docs.pro.coinbase.com/).

:loudspeaker: ***HEY!*** Be sure to checkout these other Coinbase API integrations:
* [**Coinbase**](https://github.com/bchavez/Coinbase) - For Coinbase wallet account integration.
* [**Coinbase.Commerce**](https://github.com/bchavez/Coinbase.Commerce) - For e-commerce, merchants, and websites selling products or services looking to receive cryptocurrency as payment.


#### Supported Platforms
* **.NET Standard 2.0** or later
* **.NET Framework 4.5** or later

#### Crypto Tip Jar
<a href="https://commerce.coinbase.com/checkout/f1f0e303-cb53-4415-b720-4af1df473647"><img src="https://raw.githubusercontent.com/bchavez/Coinbase.Pro/master/Docs/tipjar.png" /></a>
* :dog2: **Dogecoin**: `DGVC2drEMt41sEzEHSsiE3VTrgsQxGn5qe`



### Download & Install
**Nuget Package [Coinbase.Pro](https://www.nuget.org/packages/Coinbase.Pro/)**

```powershell
Install-Package Coinbase.Pro
```

Getting Started
------

To get started, simply new create a new `CoinbaseProClient` object as shown below:
```csharp
var client = new CoinbaseProClient(new Config
            {
               ApiKey = "my-api-key",
               Secret = "my-api-secret",
               Passphrase = "my-api-passphrase",
            });
```
Once you have a `CoinbaseProClient` object, you can call any one of the many [API endpoints listed here](https://github.com/bchavez/Coinbase.Pro#full-api-support). Extensive examples can be [found here](https://github.com/bchavez/Coinbase.Pro/tree/master/Source/Coinbase.Tests/EndpointTests). For your reference, a link to the **Coinbase Pro** developer documentation can be [found here](https://docs.pro.coinbase.com). 

As an example, to create [a **limit** order](https://docs.pro.coinbase.com/?r=1#place-a-new-order) on the **buy** side of the **`ETH-USD`** order book for **`2 ETH`** at **`100 USD`** each, do the following:
```csharp
var order = await client.Orders.PlaceLimitOrderAsync(
                  OrderSide.Buy, "ETH-USD", size: 2, limitPrice: 100m);

order.Dump();
```
The `order` object returned by the trading engine will have similar values to the following JSON object: 
```json
{
  "id": "ba3d3318-d1f0-4f9d-ae6f-1bda6ff370fa",
  "price": 100.00000000,
  "size": 2.00000000,
  "product_id": "ETH-USD",
  "side": "buy",
  "stp": "dc",
  "type": "limit",
  "time_in_force": "GTC",
  "post_only": true,
  "created_at": "2018-11-30T05:11:54.000355+00:00",
  "fill_fees": 0.0000000000000000,
  "filled_size": 0.00000000,
  "executed_value": 0.0000000000000000,
  "status": "pending",
  "settled": false,
  "funds": 0.0,
  "specified_funds": 0.0
}
```

#### Full API Support
##### Private Endpoints
* [`client.Accounts`](https://docs.pro.coinbase.com/?r=1#accounts) - [Examples](https://github.com/bchavez/Coinbase.Pro/blob/master/Source/Coinbase.Tests/EndpointTests/AccountsTest.cs)
* [`client.Orders`](https://docs.pro.coinbase.com/?r=1#orders) - [Examples](https://github.com/bchavez/Coinbase.Pro/blob/master/Source/Coinbase.Tests/EndpointTests/OrdersTest.cs)
* [`client.Fills`](https://docs.pro.coinbase.com/?r=1#fills) - [Examples](https://github.com/bchavez/Coinbase.Pro/blob/master/Source/Coinbase.Tests/EndpointTests/FillsTest.cs)
* [`client.Deposits`](https://docs.pro.coinbase.com/?r=1#deposits) - [Examples](https://github.com/bchavez/Coinbase.Pro/blob/master/Source/Coinbase.Tests/EndpointTests/DepositsTest.cs)
* [`client.Withdrawls`](https://docs.pro.coinbase.com/?r=1#withdrawals) - [Examples](https://github.com/bchavez/Coinbase.Pro/blob/master/Source/Coinbase.Tests/EndpointTests/WithdrawlsTest.cs)
* [`client.Conversion`](https://docs.pro.coinbase.com/?r=1#stablecoin-conversions) - [Examples](https://github.com/bchavez/Coinbase.Pro/blob/master/Source/Coinbase.Tests/EndpointTests/ConversionsTest.cs) - For stablecoin conversions, ie: USD/USDC.
* [`client.PaymentMethods`](https://docs.pro.coinbase.com/?r=1#payment-methods) - [Examples](https://github.com/bchavez/Coinbase.Pro/blob/master/Source/Coinbase.Tests/EndpointTests/PaymentMethodsTest.cs)
* [`client.CoinbaseAccounts`](https://docs.pro.coinbase.com/?r=1#coinbase-accounts) - [Examples](https://github.com/bchavez/Coinbase.Pro/blob/master/Source/Coinbase.Tests/EndpointTests/CoinbaseAccountsTest.cs)
* [`client.Reports`](https://docs.pro.coinbase.com/?r=1#reports) - [Examples](https://github.com/bchavez/Coinbase.Pro/blob/master/Source/Coinbase.Tests/EndpointTests/ReportsTest.cs)
* [`client.UserAccount`](https://docs.pro.coinbase.com/?r=1#user-account) - [Examples](https://github.com/bchavez/Coinbase.Pro/blob/master/Source/Coinbase.Tests/EndpointTests/UserAccountTest.cs)

##### Market Data Endpoints
* [`client.MarketData`](https://docs.pro.coinbase.com/?r=1#market-data) - [Examples](https://github.com/bchavez/Coinbase.Pro/blob/master/Source/Coinbase.Tests/EndpointTests/MarketDataTests.cs)

##### WebSocket Feed
* [`CoinbaseProWebSocket`](https://docs.pro.coinbase.com/?r=1#websocket-feed) - [Examples](https://github.com/bchavez/Coinbase.Pro/blob/master/Source/Coinbase.Tests/IntegrationTests/WebSocketTests.cs)


### Error Handling
When errors occur after calling an API, **Coinbase Pro** delivers error messages in the response body of a failed HTTP call. First wrap your call in a `try/catch` statement and handle the `Exception ex`. Next, get the error message of a failed API call by calling `GetErrorMessageAsync()` extension method on the exception. The `GetErrorMessageAsync()` extension method will read the response body of the failed HTTP call as shown below:
```csharp
try
{
   var order = await client.Orders.PlaceLimitOrderAsync(
      OrderSide.Buy, "BTCX-USDX", size: 1, limitPrice: 5000m);
}
catch (Exception ex)
{
   var errorMsg = await ex.GetErrorMessageAsync();
   Console.WriteLine(errorMsg);
}
//OUTPUT: "Product not found"
```

### Pagination
Some **Coinbase Pro** APIs are [paginable](https://docs.pro.coinbase.com/?r=1#pagination). However, **Coinbase Pro**'s paging can be [a little confusing](https://docs.pro.coinbase.com/?r=1#pagination) at first. So, let's review. Consider the following diagram below that illustrates the `Current Point In Time` over a paginable set of data with an item size of 5 items per page:
```
    Five Items Per Page (limit=5)
     
Past                         Future
Older                         Newer
11   15 16   20     21   25 26   30
[items] [items]     [items] [items]
              ^
      After <-|-> Before
              ^
              |
    Current Point In Time
```
Suppose you grabbed the most recent trades from `var trades = client.MarketData.GetTradesAsync("ETH-USD", limit: 5)`. The data you captured in `trades` is the `Current Point In Time` with the most recent trade `20` as shown in the diagram above.

* To enumerate ***older*** trades beyond the initial page:
```csharp
//Get the initial page, items 16 through 20
var trades = await client.MarketData.GetTradesAsync("ETC-USD", limit: 5);

//Get the next batch of older trades after the current page.
while( trades.After.HasValue )
{
   trades = await client.MarketData.GetTradesAsync("ETC-USD", limit: 5, after: trades.After);
}
```  

Now suppose time advances, more trades happen in the market. Given the `Current Point In Time` with the initial page of items `16-20`.

* To enumerate ***newer*** trades beyond the initial page:
```csharp
//Get the initial page, items 16 through 20
var trades = await client.MarketData.GetTradesAsync("ETC-USD", limit: 5);

//Some time advances, trades execute.

//Now, get the next batch of newer trades before the current page.
while( trades.Before.HasValue )
{
   trades = await client.MarketData.GetTradesAsync("ETC-USD", limit: 5, before: trades.Before);
}
```
More information about pagination can be [found here](https://docs.pro.coinbase.com/?r=1#pagination).

## WebSocket Feeds

This library also supports live WebSocket feeds. There are two types **Authenticated** and **Unauthenticated**  feeds. 
### Unauthenticated WebSocket
To create an unauthenticated feed, simply do the following:
```csharp
var socket = new CoinbaseProWebsocket();
```

### Authenticated WebSocket
To create an authenticated feed, doing the following:
```csharp
//authenticated feed
var socket = new CoinbaseProWebSocket(new WebSocketConfig
   {
      ApiKey = "my-api-key",
      Secret = "my-api-secret",
      Passphrase = "my-api-passphrase",
   });
```
### Subscribing to Events over WebSocket
Be sure to [check the documentation here](https://docs.pro.coinbase.com/?r=1#subscribe) to know all the events you can subscribe to. The following example demonstrates how to continue setting  up the WebSocket for simple *heartbeat* events.
```csharp
var socket = ...; //using authenticated or unauthenticated instance

//Connect the websocket,
//when this connect method completes, the socket is ready
await socket.ConnectAsync();

//add an event handler for the message received event on the raw socket
socket.RawSocket.MessageReceived += RawSocket_MessageReceived;

//create a subscription of what to listen to
var sub = new Subscription
   {
      ProductIds =
         {
            "ETH-USD",
         },
      Channels =
         {
            "heartbeat",
         }
   };

//send the subscription upstream
await socket.SubscribeAsync(sub);

//now wait for data.
await Task.Delay(TimeSpan.FromMinutes(1));
```
Once your subscription is sent upstream, you should start receiving events over the WebSocket. The following example shows how to process incoming messages as they arrive:

```csharp
private void RawSocket_MessageReceived(object sender, MessageReceivedEventArgs e)
{
   //Try parsing the e.Message JSON.
   if( WebSocketHelper.TryParse(e.Message, out var msg) )
   {
      if( msg is HeartbeatEvent hb )
      {
         Console.WriteLine($"Last Trade Id: {hb.LastTradeId}");
      }
   }
}
```

A complete working example can be [found here](https://github.com/bchavez/Coinbase.Pro/blob/master/Source/Coinbase.Tests/IntegrationTests/WebSocketTests.cs).

If you'd like to use your own WebSocket implementation, the `WebSocketHelper` is a helpful utility class for creating authenticated JSON subscription messages.

-------


Easy peasy! **Happy crypto trading!** :tada:


Reference
---------
* [Coinbase Pro API Documentation](https://docs.pro.coinbase.com)


Building
--------
* Download the source code.
* Run `build.cmd`.

Upon successful build, the results will be in the `\__compile` directory. If you want to build NuGet packages, run `build.cmd pack` and the NuGet packages will be in `__package`.



Contributors
---------
Created by [Brian Chavez](http://www.bitarmory.com).

A big thanks to GitHub and all contributors:


---

*Note: This application/third-party library is not directly supported by Coinbase Inc. Coinbase Inc. makes no claims about this application/third-party library.  This application/third-party library is not endorsed or certified by Coinbase Inc.*
