using System.IO;
using Coinbase.Pro;
using Flurl.Http.Testing;
using NUnit.Framework;

namespace Coinbase.Tests.EndpointTests
{
   public class Test
   {
      protected HttpTest server;

      [SetUp]
      public virtual void BeforeEachTest()
      {
         server = new HttpTest();
         Directory.SetCurrentDirectory(Path.GetDirectoryName(this.GetType().Assembly.Location));
      }

      [TearDown]
      public virtual void AfterEachTest()
      {
         EnsureEveryRequestHasCorrectHeaders();

         this.server.Dispose();
      }

      protected void SetupServerPagedResponse(string pageJson, int cbBefore = 54870014, int cbAfter = 54870113)
      {
         server.RespondWith(pageJson,
            headers: new {cb_before = cbBefore, cb_after = cbAfter});
      }

      protected virtual void EnsureEveryRequestHasCorrectHeaders()
      {
         server.ShouldHaveMadeACall()
            .WithHeader("User-Agent", CoinbaseProClient.UserAgent);
      }
   }
}
