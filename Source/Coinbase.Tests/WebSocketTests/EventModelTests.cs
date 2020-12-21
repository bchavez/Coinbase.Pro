using System;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using Coinbase.Pro.WebSockets;
using FluentAssertions;
using NUnit.Framework;

namespace Coinbase.Tests.WebSocketTests
{
   public class EventModelTests
   {
      [Test]
      public async Task subscriptions()
      {
         var json = @"{
    ""type"": ""subscriptions"",
    ""channels"": [
        {
            ""name"": ""level2"",
            ""product_ids"": [
                ""ETH-USD"",
                ""ETH-EUR""
            ],
        },
        {
            ""name"": ""heartbeat"",
            ""product_ids"": [
                ""ETH-USD"",
                ""ETH-EUR""
            ],
        },
        {
            ""name"": ""ticker"",
            ""product_ids"": [
                ""ETH-USD"",
                ""ETH-EUR"",
                ""ETH-BTC""
            ]
        }
    ]
}";
         WebSocketHelper.TryParse(json, out var o);

         o.Should().BeOfType<SubscriptionsEvent>();
      }

      [Test]
      public async Task hb()
      {
         var json = @"{
    ""type"": ""heartbeat"",
    ""sequence"": 90,
    ""last_trade_id"": 20,
    ""product_id"": ""BTC-USD"",
    ""time"": ""2014-11-07T08:19:28.464459Z""
}";
         WebSocketHelper.TryParse(json, out var o);

         o.Should().BeOfType<HeartbeatEvent>();
      }

      [Test]
      public async Task ticker()
      {
         var json = @"{
    ""type"": ""ticker"",
    ""trade_id"": 20153558,
    ""sequence"": 3262786978,
    ""time"": ""2017-09-02T17:05:49.250000Z"",
    ""product_id"": ""BTC-USD"",
    ""price"": ""4388.01000000"",
    ""side"": ""buy"", // Taker side
    ""last_size"": ""0.03000000"",
    ""best_bid"": ""4388"",
    ""best_ask"": ""4388.01""
}";
         WebSocketHelper.TryParse(json, out var o);

         o.Should().BeOfType<TickerEvent>();
         var t = o as TickerEvent;
         t.Side.Should().Be(OrderSide.Buy);
      }

      [Test]
      public async Task snapshot()
      {
         var json = @"{
    ""type"": ""snapshot"",
    ""product_id"": ""BTC-EUR"",
    ""bids"": [[""6500.11"", ""0.45054140""]],
    ""asks"": [[""6500.15"", ""0.57753524""]]
}";
         WebSocketHelper.TryParse(json, out var o);

         o.Should().BeOfType<SnapshotEvent>();
         var b = o as SnapshotEvent;
         b.Asks.Should().BeEquivalentTo(new OrderLiquidity
            {
               Price = 6500.15m,
               Size = 0.57753524m
         });
      }

      [Test]
      public async Task l2update()
      {
         var json = @"{
    ""type"": ""l2update"",
    ""product_id"": ""BTC-EUR"",
    ""time"": ""2019-08-14T20:42:27.265Z"",
    ""changes"": [
        [""buy"", ""6500.09"", ""0.84702376""],
        [""sell"", ""6507.00"", ""1.88933140""],
        [""sell"", ""6505.54"", ""1.12386524""],
        [""sell"", ""6504.38"", ""0""]
    ]
}";
         WebSocketHelper.TryParse(json, out var o);

         o.Should().BeOfType<L2UpdateEvent>();

         o.Dump();
         var u = o as L2UpdateEvent;
         var c = u.Changes[0];
         c.Side.Should().Be(OrderSide.Buy);
         c.Price.Should().Be(6500.09m);
         c.Size.Should().Be(0.84702376m);
         u.ProductId.Should().Be("BTC-EUR");
         u.Time.Should().Be(DateTimeOffset.Parse("2019-08-14T20:42:27.265Z"));
      }

      [Test]
      public async Task received()
      {
         var json = @"{
    ""type"": ""received"",
    ""time"": ""2014-11-07T08:19:27.028459Z"",
    ""product_id"": ""BTC-USD"",
    ""sequence"": 10,
    ""order_id"": ""d50ec984-77a8-460a-b958-66f114b0de9b"",
    ""size"": ""1.34"",
    ""price"": ""502.1"",
    ""side"": ""buy"",
    ""order_type"": ""limit""
}";
         WebSocketHelper.TryParse(json, out var o);

         o.Should().BeOfType<ReceivedEvent>();
         o.Dump();
         var r = o as ReceivedEvent;
         r.Size.Should().Be(1.34m);
      }

      [Test]
      public async Task open()
      {
         var json = @"{
    ""type"": ""open"",
    ""time"": ""2014-11-07T08:19:27.028459Z"",
    ""product_id"": ""BTC-USD"",
    ""sequence"": 10,
    ""order_id"": ""d50ec984-77a8-460a-b958-66f114b0de9b"",
    ""price"": ""200.2"",
    ""remaining_size"": ""1.00"",
    ""side"": ""sell""
}";
         WebSocketHelper.TryParse(json, out var o);

         o.Should().BeOfType<OpenEvent>();
         o.Dump();
         var r = o as OpenEvent;
         r.RemainingSize.Should().Be(1.00m);
      }

      [Test]
      public async Task done()
      {
         var json = @"{
    ""type"": ""done"",
    ""time"": ""2014-11-07T08:19:27.028459Z"",
    ""product_id"": ""BTC-USD"",
    ""sequence"": 10,
    ""price"": ""200.2"",
    ""order_id"": ""d50ec984-77a8-460a-b958-66f114b0de9b"",
    ""reason"": ""filled"", // or ""canceled""
    ""side"": ""sell"",
    ""remaining_size"": ""0""
}";
         WebSocketHelper.TryParse(json, out var o);

         o.Should().BeOfType<DoneEvent>();
         o.Dump();
         var r = o as DoneEvent;
         r.Price.Should().Be(200.2m);
      }

      [Test]
      public async Task match()
      {
         var json = @"{
    ""type"": ""match"",
    ""trade_id"": 10,
    ""sequence"": 50,
    ""maker_order_id"": ""ac928c66-ca53-498f-9c13-a110027a60e8"",
    ""taker_order_id"": ""132fb6ae-456b-4654-b4e0-d681ac05cea1"",
    ""time"": ""2014-11-07T08:19:27.028459Z"",
    ""product_id"": ""BTC-USD"",
    ""size"": ""5.23512"",
    ""price"": ""400.23"",
    ""side"": ""sell""
}";
         WebSocketHelper.TryParse(json, out var o);

         o.Should().BeOfType<MatchEvent>();
         o.Dump();
         var r = o as MatchEvent;
         r.Price.Should().Be(400.23m);
      }

      [Test]
      public async Task change()
      {
         var json = @"{
    ""type"": ""change"",
    ""time"": ""2014-11-07T08:19:27.028459Z"",
    ""sequence"": 80,
    ""order_id"": ""ac928c66-ca53-498f-9c13-a110027a60e8"",
    ""product_id"": ""BTC-USD"",
    ""new_size"": ""5.23512"",
    ""old_size"": ""12.234412"",
    ""price"": ""400.23"",
    ""side"": ""sell""
}";
         WebSocketHelper.TryParse(json, out var o);

         o.Should().BeOfType<ChangeEvent>();
         o.Dump();
         var r = o as ChangeEvent;
         r.Price.Should().Be(400.23m);
      }

      [Test]
      public async Task change2()
      {
         var json = @"{
    ""type"": ""change"",
    ""time"": ""2014-11-07T08:19:27.028459Z"",
    ""sequence"": 80,
    ""order_id"": ""ac928c66-ca53-498f-9c13-a110027a60e8"",
    ""product_id"": ""BTC-USD"",
    ""new_funds"": ""5.23512"",
    ""old_funds"": ""12.234412"",
    ""price"": ""400.23"",
    ""side"": ""sell""
}";
         WebSocketHelper.TryParse(json, out var o);

         o.Should().BeOfType<ChangeEvent>();
         o.Dump();
         var r = o as ChangeEvent;
         r.Price.Should().Be(400.23m);
      }

      [Test]
      public async Task activate()
      {
         var json = @"{
  ""type"": ""activate"",
  ""product_id"": ""test-product"",
  ""timestamp"": ""1483736448.299000"",
  ""user_id"": ""12"",
  ""profile_id"": ""30000727-d308-cf50-7b1c-c06deb1934fc"",
  ""order_id"": ""7b52009b-64fd-0a2a-49e6-d8a939753077"",
  ""stop_type"": ""entry"",
  ""side"": ""buy"",
  ""stop_price"": ""80"",
  ""size"": ""2"",
  ""funds"": ""50"",
  ""taker_fee_rate"": ""0.0025"",
  ""private"": true
}";
         WebSocketHelper.TryParse(json, out var o);

         o.Should().BeOfType<ActivateEvent>();
         o.Dump();
         var r = o as ActivateEvent;
         r.Size.Should().Be(2);
      }
   }
}
