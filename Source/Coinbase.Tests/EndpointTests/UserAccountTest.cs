using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Coinbase.Tests.EndpointTests
{
   public class UserAccountTest : TestWithAuth
   {
      [Test]
      public async Task trailing_volume()
      {
         server.RespondWithJsonTestFile();

         var r = await client.UserAccount.GetTrailingVolumeAsync();

         r.Dump();

         var o = r.First();
         o.ProductId.Should().Be("BTC-USD");
         o.Volume.Should().Be(100.00000000m);

         server.ShouldHaveCalledSomePathAndQuery("/users/self/trailing-volume")
            .WithVerb(HttpMethod.Get);

         await Verify(r);
      }
   }
}
