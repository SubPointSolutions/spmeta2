using RazorEngine;
using SPMeta2.Regression.Reports.Data;
using SPMeta2.Regression.Reports.Html.Zip;
using SPMeta2.Regression.Reports.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Reports.Html.Services
{
    public class HtmlReportGenerator : ReportGenerator
    {
        public HtmlReportGenerator()
        {
            ThemeFolder = "DefaultHtmlTemplate";
            ThemeTemplateFileName = "index.template.html";
        }

        public string ThemeFolder { get; set; }
        public string ThemeTemplateFileName { get; set; }

        public override byte[] GenerateClassReport(TestClassReport report)
        {
            return GenerateReport(report);
        }

        private byte[] GenerateReport(TestClassReport report)
        {
            var zipService = new ZipService();

            var templateFolder = string.Format("{0}/Templates/{1}", Directory.GetCurrentDirectory(), ThemeFolder);
            var templateFileContent = File.ReadAllText(string.Format("{0}/{1}", templateFolder, ThemeTemplateFileName));

            var renderedResult = Razor.Parse(templateFileContent, report);
            var renderedResultContent = System.Text.Encoding.UTF8.GetBytes(renderedResult);

            var renderedZipItem = new ZipService.ZipItem
            {
                Name = "index.html",
                Content = renderedResultContent
            };

            var zipArchive = zipService.Zip(templateFolder, new ZipService.ZipItem[] { renderedZipItem });

            return ZipService.ReadToEnd(zipArchive);

        }

        public override string FileExtension
        {
            get
            {
                return "zip";
            }
            set
            {

            }
        }
    }
}
