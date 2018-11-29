using System;
using System.Globalization;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Coinbase.Pro
{
   public class Config
   {
      public string ApiKey { get; set; }
      public string Secret { get; set; }
      public string Passphrase { get; set; }

      public bool UseTimeApi { get; set; } = false;
      public string ApiUrl { get; set; } = CoinbaseProClient.Endpoint;

      public void EnsureValid()
      {
         //if( string.IsNullOrWhiteSpace(this.ApiKey) ||
         //    )
         //if( string.IsNullOrWhiteSpace(this.ApiKey) ) throw new ArgumentNullException(nameof(ApiKey), "An API key must be specified");
         //if( string.IsNullOrWhiteSpace(this.Secret) ) throw new ArgumentNullException(nameof(Secret), "An API secret must be specified");
         //if( string.IsNullOrWhiteSpace(this.Passphrase) ) throw new ArgumentNullException(nameof(Passphrase), "An API passphrase must be specified");
      }
   }

   public interface ICoinbaseProClient
   {
      IMarketDataEndpoint MarketData { get; }
      IAccountsEndpoint Accounts { get; }
      IOrdersEndpoint Orders { get; }
   }

   public partial class CoinbaseProClient : FlurlClient, ICoinbaseProClient
   {
      public const string Endpoint = "https://api.pro.coinbase.com";

      public CoinbaseProClient(Config config = null)
      {
         this.Config = config ?? new Config();
         this.Config.EnsureValid();
         this.ConfigureClient();
      }

      public Config Config { get; }

      internal static readonly string UserAgent =
         $"{AssemblyVersionInformation.AssemblyProduct}/{AssemblyVersionInformation.AssemblyVersion} ({AssemblyVersionInformation.AssemblyTitle}; {AssemblyVersionInformation.AssemblyDescription})";

      protected internal virtual void ConfigureClient()
      {
         this.WithHeader("User-Agent", UserAgent);

         if( !string.IsNullOrWhiteSpace(this.Config.ApiKey) )
         {
            this.Configure(ApiKeyAuth);
         }
      }

      private void ApiKeyAuth(ClientFlurlHttpSettings settings)
      {
         async Task SetHeaders(HttpCall http)
         {
            var body = http.RequestBody;
            var method = http.Request.Method.Method.ToUpperInvariant();
            var url = http.Request.RequestUri.PathAndQuery;

            string timestamp;
            if (this.Config.UseTimeApi)
            {
               var timeResult = await this.MarketData.GetTimeAsync().ConfigureAwait(false);
               timestamp = timeResult.Epoch.ToCoinbaseTime();
            }
            else
            {
               timestamp = TimeHelper.GetCurrentUnixTimestampSeconds().ToCoinbaseTime();
            }

            var signature = ApiKeyAuthenticator.GenerateSignature(
               timestamp,
               method,
               url,
               body,
               this.Config.Secret);

            http.FlurlRequest
               .WithHeader(HeaderNames.AccessKey, this.Config.ApiKey)
               .WithHeader(HeaderNames.AccessSign, signature)
               .WithHeader(HeaderNames.AccessTimestamp, timestamp)
               .WithHeader(HeaderNames.AccessPassphrase, this.Config.Passphrase);
         }

         settings.BeforeCallAsync = SetHeaders;
      }
   }
}
