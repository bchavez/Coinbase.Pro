using System.IO;
using System.Net;
using Flurl.Http;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Coinbase.Tests.IntegrationTests
{
   public class Secrets
   {
      public string ApiKey { get; set; }
      public string ApiSecret { get; set; }
      public string ApiPassphrase { get; set; }
   }

   [Explicit]
   public class IntegrationTests
   {
      protected Secrets secrets;

      public IntegrationTests()
      {
         Directory.SetCurrentDirectory(Path.GetDirectoryName(typeof(IntegrationTests).Assembly.Location));

         ReadSecrets();

         var webProxy = new WebProxy("http://localhost.:8888", BypassOnLocal: false);

         FlurlHttp.Configure(settings =>
            {
               settings.HttpClientFactory = new ProxyFactory(webProxy);
            });

      }

      protected void ReadSecrets()
      {
         var json = File.ReadAllText("../../.secrets.txt");
         this.secrets = JsonConvert.DeserializeObject<Secrets>(json);
      }
   }
}
