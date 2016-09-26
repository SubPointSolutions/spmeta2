using System;
using System.Collections.Generic;
using System.IO;
using HtmlAgilityPack;
using SubPointSolutions.Docs.Code.Data;
using SubPointSolutions.Docs.Services.Base;

namespace SubPointSolutions.Docs.Code.Services
{
    public class HtmlSamplesService : SamplesServiceBase
    {
        public HtmlSamplesService()
        {
            FileExtension = "*.html";
        }

        public override IEnumerable<DocSample> CreateSamplesFromSourceFile(string filePath)
        {
            var result = new List<DocSample>();

            var xmlDoc = new HtmlDocument();
            xmlDoc.LoadHtml(File.ReadAllText(filePath));

            var samples = new List<HtmlNode>();

            var htmlSamples = (xmlDoc.DocumentNode.SelectNodes("//div[@doc-sample]"));
            var jsSamples = (xmlDoc.DocumentNode.SelectNodes("//script[@doc-sample]"));

            if (htmlSamples != null)
                samples.AddRange(htmlSamples);

            if (jsSamples != null)
                samples.AddRange(jsSamples);

            foreach (var sample in samples)
            {
                var docSample = new DocSample();

                var methodName = sample.GetAttributeValue("doc-method-name", string.Empty);

                if (string.IsNullOrEmpty(methodName))
                    throw new ArgumentException("doc-method-name attr must be present");

                if (sample.Name == "script")
                    docSample.Language = "js";
                else
                    docSample.Language = "html";

                docSample.SourceFileName = Path.GetFileName(filePath);
                docSample.SourceFileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

                docSample.SourceFileFolder = Path.GetDirectoryName(filePath);
                docSample.SourceFilePath = filePath;

                docSample.ClassName = Path.GetFileNameWithoutExtension(filePath);

                docSample.MethodName = methodName;
                docSample.MethodBody = sample.InnerHtml;

                result.Add(docSample);
            }

            return result;
        }
    }
}
