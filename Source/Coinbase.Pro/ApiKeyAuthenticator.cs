using System;
using System.Security.Cryptography;
using System.Text;

namespace Coinbase.Pro
{
   public static class ApiKeyAuthenticator
   {
      public static string GenerateSignature(string timestamp, string method, string requestPath, string body, string appSecret)
      {
         return Sign(appSecret, timestamp + method + requestPath + body);
      }

      internal static string Sign(string base64key, string data)
      {
         var hmacKey = Convert.FromBase64String(base64key);
         var dataBytes = Encoding.UTF8.GetBytes(data);

         using (var hmac = new HMACSHA256(hmacKey))
         {
            var sig = hmac.ComputeHash(dataBytes);
            return Convert.ToBase64String(sig);
         }
      }
   }
}
