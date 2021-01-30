using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Collections.Generic;
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
      string Before { get; }
      string After { get; }
   }

   public class PagedResponse<T> : JsonResponse, IPagedResource
   {
      public List<T> Data { get; internal set; }

      /// <summary>
      /// Most paginated requests return the latest information (newest)
      /// as the first page sorted by newest (in chronological time) first.
      /// To get older information you would request pages after the
      /// initial page. To get information newer, you would request
      /// pages before the first page.
      /// 
      /// To request a page of records before the current one,
      /// use the before query parameter. Your initial request
      /// can omit this parameter to get the default first page.
      /// 
      /// The response will contain a CB-BEFORE header which will return the
      /// cursor id to use in your next request for the page before the
      /// current one. The page before is a newer page and not one that
      /// happened before in chronological time.
      /// </summary>
      public string Before { get; internal set; }

      /// <summary>
      /// To request a page of records before the current one,
      /// use the before query parameter. Your initial request
      /// can omit this parameter to get the default first page.
      /// 
      /// Most paginated requests return the latest information (newest)
      /// as the first page sorted by newest (in chronological time) first.
      /// To get older information you would request pages after the
      /// initial page. To get information newer, you would request
      /// pages before the first page.
      /// 
      /// The response will also contain a CB-AFTER header which will
      /// eturn the cursor id to use in your next request for
      /// the page after this one. The page after is an older page
      /// and not one that happened after this one in chronological time.
      /// </summary>
      public string After { get; internal set; }
   }
}
