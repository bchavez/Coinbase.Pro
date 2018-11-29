using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using Refit;

namespace Coinbase.Pro.Models
{
   public class Json
   {
      /// <summary>
      /// Extra data for/from the JSON serializer/deserializer to included with the object model.
      /// </summary>
      [JsonExtensionData]
      public IDictionary<string, JToken> ExtraJson { get; internal set; } = new Dictionary<string, JToken>();
   }

   public class JsonResponse : Json
   {
      public string Message { get; set; }
   }

   public interface IPagedResource
   {
      long? Before { get; }
      long? After { get; }
   }

   public class PagedResponse<T> : JsonResponse, IPagedResource
   {
      public List<T> Data { get; internal set; }
      public long? Before { get; internal set; }
      public long? After { get; internal set; }
   }
}
