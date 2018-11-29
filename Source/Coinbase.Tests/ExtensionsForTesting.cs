using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Flurl.Http;
using Flurl.Http.Testing;
using Newtonsoft.Json;

namespace Coinbase.Tests
{
   internal static class ExtensionsForTesting
   {
      public static void Dump(this object obj)
      {
         Console.WriteLine(obj.DumpString());
      }

      public static string DumpString(this object obj)
      {
         return JsonConvert.SerializeObject(obj, Formatting.Indented);
      }

      public static HttpCallAssertion2 ShouldHaveCalledSomePathAndQuery(this HttpTest test, string pathAndQuery)
      {
         var paths = test.CallLog.Select(c => c.Request.RequestUri.PathAndQuery);

         paths.Should().Contain(pathAndQuery);

         return new HttpCallAssertion2(test.CallLog);
      }

      public static HttpCallAssertion2 ShouldHaveCalledAnExactUrl(this HttpTest test, string exactUrl)
      {
         var fullPaths = test.CallLog.Select(c => c.FlurlRequest.Url.ToString());

         fullPaths.Should().Contain(exactUrl);
         return new HttpCallAssertion2(test.CallLog);
      }

      public static HttpCallAssertion2 ShouldHaveSomeRequestBody(this HttpTest test, string json)
      {
         var bodies = test.CallLog.Select(c => c.RequestBody);

         bodies.Should().Contain(json);

         return new HttpCallAssertion2(test.CallLog);
      }
      public static HttpCallAssertion2 WithSomeRequestBody(this HttpCallAssertion2 test, string json)
      {
         var bodies = test.LoggedCalls.Select(c => c.RequestBody);

         var expectedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(json));

         bodies.Should().Contain(expectedJson);

         return test;
      }

      public static HttpTest RespondWithPagedResult(this HttpTest test, string json, int before, int after)
      {
         return test.RespondWith(json, headers: new { cb_before=before, cb_after = after});
      }
   }

   public class HttpCallAssertion2 : HttpCallAssertion
   {
      public IEnumerable<HttpCall> LoggedCalls { get; }

      public HttpCallAssertion2(IEnumerable<HttpCall> loggedCalls, bool negate = false) : base(loggedCalls, negate)
      {
         this.LoggedCalls = loggedCalls;
      }
   }

   
}
