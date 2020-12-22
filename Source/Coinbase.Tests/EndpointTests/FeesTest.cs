using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using VerifyNUnit;

namespace Coinbase.Tests.EndpointTests
{
   public class FeesTest : TestWithAuth
   {
      [Test]
      public async Task can_get_current_fees()
      {
         server.RespondWithJsonTestFile();

         var r = await this.client.Fees.GetCurrentFeesAsync();

         server.ShouldHaveCalledSomePathAndQuery("/fees")
            .WithVerb(HttpMethod.Get);

         await Verifier.Verify(r);
      }
   }
}
