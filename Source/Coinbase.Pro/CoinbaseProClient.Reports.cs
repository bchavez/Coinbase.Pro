using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using Flurl;
using Flurl.Http;

namespace Coinbase.Pro
{
   public interface IReportsEndpoint
   {
      /// <summary>
      /// Reports provide batches of historic information about your account in
      /// various human and machine readable forms. The file_url field will be available once
      /// the report has successfully been created and is available for download.
      /// </summary>
      /// <param name="startDate">Starting date for the report (inclusive)</param>
      /// <param name="endDate">Ending date for the report (inclusive)</param>
      /// <param name="productId">ID of the product to generate a fills report for. E.g. BTC-USD.</param>
      /// <param name="accountId">ID of the account to generate an account report for.</param>
      /// <param name="format">Default PDF.</param>
      /// <param name="email">Optional. Email address to send the report to.</param>
      Task<Report> CreateFillReportAsync(
         DateTimeOffset startDate, DateTimeOffset endDate,
         string productId, string accountId = null,
         ReportFormat format = ReportFormat.Pdf, string email = null,
         CancellationToken cancellationToken = default);

      /// <summary>
      /// Reports provide batches of historic information about your account in
      /// various human and machine readable forms. The file_url field will be available once
      /// the report has successfully been created and is available for download.
      /// </summary>
      /// <param name="startDate">Starting date for the report (inclusive)</param>
      /// <param name="endDate">Ending date for the report (inclusive)</param>
      /// <param name="accountId">ID of the account to generate an account report for.</param>
      /// <param name="productId">ID of the product to generate a fills report for. E.g. BTC-USD.</param>
      /// <param name="format">Default PDF.</param>
      /// <param name="email">Optional. Email address to send the report to.</param>
      Task<Report> CreateAccountReportAsync(
         DateTimeOffset startDate, DateTimeOffset endDate,
         string accountId, string productId= null,
         ReportFormat format = ReportFormat.Pdf, string email = null,
         CancellationToken cancellationToken = default);

      /// <summary>
      /// Advanced: For users that want to create their own report with custom parameters
      /// </summary>
      Task<Report> CreateReportAsync(CreateReport createReport, CancellationToken cancellationToken = default);

      /// <summary>
      /// Once a report request has been accepted for processing, the status is available by
      /// polling the report resource endpoint.
      /// The final report will be uploaded and available at file_url once the status
      /// indicates ready
      /// </summary>
      /// <param name="reportId">The Report ID</param>
      /// <param name="cancellationToken"></param>
      Task<Report> GetReportStatusAsync(string reportId, CancellationToken cancellationToken = default);
   }

   public partial class CoinbaseProClient : IReportsEndpoint
   {
      public IReportsEndpoint Reports => this;

      protected internal Url ReportsEndpoint => this.Config.ApiUrl.AppendPathSegment("reports");

      Task<Report> IReportsEndpoint.CreateFillReportAsync(
         DateTimeOffset startDate, DateTimeOffset endDate,
         string productId, string accountId, ReportFormat format, string email,
         CancellationToken cancellationToken)
      {
         var r = new CreateReport
            {
               Type = ReportType.Fills,
               StartDate = startDate,
               EndDate = endDate,
               ProductId = productId,
               AccountId = accountId,
               Format = format,
               Email = email,
            };

         return this.Reports.CreateReportAsync(r, cancellationToken);
      }

      Task<Report> IReportsEndpoint.CreateAccountReportAsync(
         DateTimeOffset startDate, DateTimeOffset endDate,
         string accountId, string productId, ReportFormat format, string email,
         CancellationToken cancellationToken)
      {
         var r = new CreateReport
            {
               Type = ReportType.Account,
               StartDate = startDate,
               EndDate = endDate,
               ProductId = productId,
               AccountId = accountId,
               Format = format,
               Email = email,
            };

         return this.Reports.CreateReportAsync(r, cancellationToken);
      }

      Task<Report> IReportsEndpoint.CreateReportAsync(CreateReport createReport, CancellationToken cancellationToken)
      {
         return this.ReportsEndpoint
            .WithClient(this)
            .PostJsonAsync(createReport, cancellationToken)
            .ReceiveJson<Report>();
      }

      Task<Report> IReportsEndpoint.GetReportStatusAsync(string reportId, CancellationToken cancellationToken)
      {
         return this.ReportsEndpoint
            .WithClient(this)
            .AppendPathSegment(reportId)
            .GetJsonAsync<Report>(cancellationToken);
      }
   }
}
