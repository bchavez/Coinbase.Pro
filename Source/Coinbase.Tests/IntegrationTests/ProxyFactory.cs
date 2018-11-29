using System.Net;
using System.Net.Http;
using Flurl.Http.Configuration;

namespace Coinbase.Tests.IntegrationTests
{
   public class ProxyFactory : DefaultHttpClientFactory
   {
      private readonly WebProxy proxy;

      public ProxyFactory(WebProxy proxy)
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