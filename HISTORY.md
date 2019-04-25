## v2.0.6
* Issue 8: Fixed JSON deseralization error with `client.Fills.GetFillsByProductIdAsync` where `usd_volume` could be null.

## v2.0.5
* Issue 7: Fixed JSON deserialization error with `client.MarketData.GetProductsAsync` when using sandbox.

## v2.0.4
* Issue 6: Fixed issue parsing null for best bid/ask in markets without volume and empty order book for `TickerEvent` webhook.

## v2.0.3
* Issue 4: Updates Newtonsoft.Json to 12.0.1 that contains fix for parsing decimals with exponential notation.

## v2.0.2
* Added GetErrorMessageAsync() extension method.

## v2.0.1
* Production ready. Models and APIs finalized.

## v0.3.0
* All APIs are now implemented.
* Websocket support implemented.

## v0.2.0
* Deposits, Withdraws, Fills, Stablecoin Conversions, PaymentMethods, Coinbase Accounts, Report and Trailing Volume endpoints supported.

## v0.1.0
* Initial implementation for orders and market data.