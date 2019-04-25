using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
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
      IConversionEndpoint Conversion { get; }
      IDepositsEndpoint Deposits { get; }
      IFillsEndpoint Fills { get; }
      IPaymentMethodsEndpoint PaymentMethods { get; }
      IReportsEndpoint Reports { get; }
      IUserAccountEndpoint UserAccount { get; }
      IWithdrawalsEndpoint Withdrawals { get; }
      ICoinbaseAccountsEndpoint CoinbaseAccounts { get; }
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
            var timestamp = await TimeHelper.GetCurrentTimestampAsync(this.Config.UseTimeApi)
               .ConfigureAwait(false);

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

      /// <summary>
      /// Enable HTTP debugging via Fiddler. Ensure Tools > Fiddler Options... > Connections is enabled and has a port configured.
      /// Then, call this method with the following URL format: http://localhost.:PORT where PORT is the port number Fiddler proxy
      /// is listening on. (Be sure to include the period after the localhost).
      /// </summary>
      /// <param name="proxyUrl">The full proxy URL Fiddler proxy is listening on. IE: http://localhost.:8888 - The period after localhost is important to include.</param>
      public CoinbaseProClient EnableFiddlerDebugProxy(string proxyUrl)
      {
         var webProxy = new WebProxy(proxyUrl, BypassOnLocal: false);

         this.Configure(cf =>
            {
               cf.HttpClientFactory = new DebugProxyFactory(webProxy);
            });

         return this;
      }

      private class DebugProxyFactory : DefaultHttpClientFactory
      {
         private readonly WebProxy proxy;

         public DebugProxyFactory(WebProxy proxy)
         {
            this.proxy = proxy;
         }

         public override HttpMessageHandler CreateMessageHandler()
         {
            return new HttpClientHandler
               {
                  Proxy = this.proxy,
                  UseProxy = true
               };
         }
      }
   }

   public static class ExtensionsForExceptions
   {
      /// <summary>
      /// Parses the response body of the failed HTTP call return any error status messages.
      /// </summary>
      public static async Task<string> GetErrorMessageAsync(this FlurlHttpException ex)
      {
         if( ex is null ) return null;

         var error = await ex.GetResponseJsonAsync<JsonResponse>()
            .ConfigureAwait(false);

         return error?.Message;
      }

      /// <summary>
      /// Parses the response body of the failed HTTP call return any error status messages.
      /// </summary>
      public static Task<string> GetErrorMessageAsync(this Exception ex)
      {
         return GetErrorMessageAsync(ex as FlurlHttpException);
      }
   }
}
