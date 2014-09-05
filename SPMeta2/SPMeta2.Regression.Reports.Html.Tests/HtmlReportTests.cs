using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.Reports.Html.Services;
using SPMeta2.Regression.Reports.Data;
using System.IO;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.Regression.Reports.Html.Zip;

namespace SPMeta2.Regression.Reports.Html.Tests
{
    [TestClass]
    public class HtmlReportTests
    {
        [TestMethod]
        public void CanGenerateHtmlReport()
        {
            var generator = new HtmlReportGenerator();
            var report = XmlSerializerUtils.DeserializeFromString<TestClassReport>(TestResults.Result1);

            var result = generator.GenerateClassReport(report);

            var tocken = Environment.TickCount.ToString();

            var reportFileName = tocken + ".zip";

            File.WriteAllBytes(reportFileName, result);

            var unxipFolder = tocken + "_preview";
            Directory.CreateDirectory(unxipFolder);

            ZipService.Unzip(reportFileName, unxipFolder);

            var index = Path.Combine(unxipFolder, "index.html");
            System.Diagnostics.Process.Start(index);
        }
    }
}
