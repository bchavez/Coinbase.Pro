using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Coinbase.Pro;
using Flurl.Http.Testing;
using NUnit.Framework;
using VerifyNUnit;
using VerifyTests;

namespace Coinbase.Tests.EndpointTests
{
   public class Test
   {
      protected HttpTest server;

      static Test()
      {
         VerifierSettings.UseStrictJson();
      }

      [SetUp]
      public virtual void BeforeEachTest()
      {
         this.server = new HttpTest();

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

      protected virtual void EnsureEveryRequestHasCorrectHeaders()
      {
         server.ShouldHaveMadeACall()
            .WithHeader("User-Agent", CoinbaseProClient.UserAgent);
      }
   }
}
