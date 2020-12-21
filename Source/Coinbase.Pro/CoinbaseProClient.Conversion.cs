using System.Threading;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using Flurl;
using Flurl.Http;

namespace Coinbase.Pro
{
   public interface IConversionEndpoint
   {
      /// <summary>
      /// Stablecoin Conversions
      /// </summary>
      /// <param name="from">Currency Id</param>
      /// <param name="to">Currency id</param>
      /// <param name="amount">Amount of from to convert to to</param>
      /// <param name="cancellationToken"></param>
      Task<Conversion> ConvertAsync(string from, string to, decimal amount, CancellationToken cancellationToken = default);
   }

   public partial class CoinbaseProClient : IConversionEndpoint
   {
      /// <summary>
      /// Stablecoin conversions
      /// </summary>
      public IConversionEndpoint Conversion => this;

      protected internal Url ConversionEndpoint => this.Config.ApiUrl.AppendPathSegment("conversions");

      Task<Conversion> IConversionEndpoint.ConvertAsync(string @from, string to, decimal amount, CancellationToken cancellationToken)
      {
         var c = new CreateConversion
            {
               Amount = amount,
               From = from,
               To = to
            };

         return this.ConversionEndpoint
            .WithClient(this)
            .PostJsonAsync(c, cancellationToken)
            .ReceiveJson<Conversion>();
      }
   }
}
