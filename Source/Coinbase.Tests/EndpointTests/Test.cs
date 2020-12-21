using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Coinbase.Pro;
using Flurl.Http.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using VerifyNUnit;
using VerifyTests;

namespace Coinbase.Tests.EndpointTests
{
   public class Test
   {
      protected HttpTest server;
      protected VerifySettings settings;

      [SetUp]
      public virtual void BeforeEachTest()
      {
         this.server = new HttpTest();
         this.settings = new VerifySettings();
         this.settings.UseExtension("json");
         //this.settings.AutoVerify();


#if NET45
         Directory.SetCurrentDirectory(Path.GetDirectoryName(this.GetType().Assembly.Location));
#endif
      }

      [TearDown]
      public virtual void AfterEachTest()
      {
         EnsureEveryRequestHasCorrectHeaders();

         this.server.Dispose();
      }

      protected Task Verify<T>(T t, [CallerFilePath] string sourceFile = "")
      {
         return Verifier.Verify(t, this.settings, sourceFile);
      }

      protected virtual void EnsureEveryRequestHasCorrectHeaders()
      {
         server.ShouldHaveMadeACall()
            .WithHeader("User-Agent", CoinbaseProClient.UserAgent);
      }
   }
}
