using Coinbase.Pro;

namespace Coinbase.Tests.EndpointTests
{
   public class TestWithAuth : Test
   {
      protected CoinbaseProClient client;

      private string apiKey = "key";
      private string apiSecret = "cGhyYXNl";
      private string apiPassprhase = "phrase";

      public override void BeforeEachTest()
      {
         base.BeforeEachTest();

         client = new CoinbaseProClient(new Config
            {
               ApiKey = apiKey,
               Secret = apiSecret,
               Passphrase = apiPassprhase,
               UseTimeApi = false
            });
      }

      protected override void EnsureEveryRequestHasCorrectHeaders()
      {
         base.EnsureEveryRequestHasCorrectHeaders();
         server.ShouldHaveMadeACall()
            .WithHeader(HeaderNames.AccessPassphrase, apiPassprhase)
            .WithHeader(HeaderNames.AccessKey, apiKey)
            .WithHeader(HeaderNames.AccessSign)
            .WithHeader(HeaderNames.AccessTimestamp);
      }
   }
}
