## v3.2.1
* PR 23: Added `Withdrawals.GetFeeEstimate`. Thanks la4atld!
* PR 12: Added `Fees.GetCurrentFeesAsync` for maker and taker fees. Thanks vflame!

## v3.2.0
* Added `ConnectResult` return type from `CoinbaseProWebSocket.ConnectAsync()` for better semantic connection handling.

## v3.0.5
* Fixed `Withdrawals.GetWithdrawal()`. Previously used wrong URL path.

## v3.0.4
* Added `Deposits.GetDeposit()`
* Added `DateTimeOffset` parameters to `Withdrawals.ListWithdrawals()` and `Deposits.ListDeposits()`.

## v3.0.3
* Ensure `Withdrawals.ListWithdrawals()` returns `PagedResponse`.
* Add `Withdrawals.GetWithdrawal()`.

## v3.0.2
* PR 16: Add `Withdrawals.ListWithdrawals()` history support.

## v3.0.1
* External references updated.
* Added `CoinbaseProWebSocket.EnableFiddlerDebugProxy` method for debugging.
* Issue 15: `CoinbaseProWebSocket` using TLS 1.2 by default to prevent connection hanging.
* Issue 15: Fixed threading issue in `CoinbaseProWebSocket` that may have prevented websocket from working.

## v2.0.6
* Issue 8: Fixed JSON deseralization error with `client.Fills.GetFillsByProductIdAsync` where `usd_volume` could be null.
* Added `client.EnableFiddlerDebugProxy` helper method for debugging client requests.

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