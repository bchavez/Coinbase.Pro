﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Coinbase.Pro.Models;
using FluentAssertions;
using NUnit.Framework;
using VerifyNUnit;

namespace Coinbase.Tests.EndpointTests
{
   public class ReportsTest : TestWithAuth
   {
      [Test]
      public async Task create_report()
      {
         server.RespondWithJsonTestFile();

         var date = DateTimeOffset.Parse("11/28/2018");

         var r = await client.Reports.CreateFillReportAsync(date, date.AddDays(1),
            "BTC-USD", email: "satoshi@bitcoin.com");

         var expectedBody =
$@"{{
   ""type"": ""fills"",
   ""start_date"": ""{date:O}"",
   ""end_date"": ""{date.AddDays(1):O}"",
   ""format"": ""pdf"",
   ""email"": ""satoshi@bitcoin.com"",
   ""product_id"": ""BTC-USD""
}}";

         server.ShouldHaveCalledSomePathAndQuery("/reports")
            .WithSomeRequestBody(expectedBody)
            .WithVerb(HttpMethod.Post);

         await Verifier.Verify(r);
      }

      [Test]
      public async Task get_report_status()
      {
         server.RespondWithJsonTestFile();

         var r = await client.Reports.GetReportStatusAsync("fff");

         r.Id.Should().Be("D94DF955-4A2C-4C08-80EA-CDA8249ED099");
         r.Type.Should().Be(ReportType.Fills);

         server.ShouldHaveCalledSomePathAndQuery("/reports/fff")
            .WithVerb(HttpMethod.Get);

         await Verifier.Verify(r);
      }
   }
}
