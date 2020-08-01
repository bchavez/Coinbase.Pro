using System.IO;
using Newtonsoft.Json;

namespace Examples
{
   public class Credentials
   {
      public string ApiKey;
      public string ApiSecret;
      public string ApiPassphrase;

      public static Credentials ReadCredentials(string path)
      {
         var json = File.ReadAllText(path);
         var creds = JsonConvert.DeserializeObject<Credentials>(json);
         return creds;
      }
   }
}
