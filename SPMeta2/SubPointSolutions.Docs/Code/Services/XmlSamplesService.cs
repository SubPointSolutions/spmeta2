using System;
using System.Collections.Generic;
using System.IO;
using HtmlAgilityPack;
using SubPointSolutions.Docs.Code.Data;
using SubPointSolutions.Docs.Services.Base;

namespace SubPointSolutions.Docs.Code.Services
{
    public class XmlSamplesService : SamplesServiceBase
    {
        public XmlSamplesService()
        {
            FileExtension = "*.xml";
        }

        public override IEnumerable<DocSample> CreateSamplesFromSourceFile(string filePath)
        {
            var result = new List<DocSample>();

            var xmlDoc = new HtmlDocument();
            xmlDoc.LoadHtml(File.ReadAllText(filePath));

            var samples = xmlDoc.DocumentNode.Descendants("Sample");

            foreach (var sample in samples)
            {
                var docSample = new DocSample();

                var methodName = sample.GetAttributeValue("MethodName", string.Empty);

                if (string.IsNullOrEmpty(methodName))
                    throw new ArgumentException("methodName attr must be present");

                docSample.SourceFileName = Path.GetFileName(filePath);
                docSample.SourceFileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

                docSample.SourceFileFolder = Path.GetDirectoryName(filePath);
                docSample.SourceFilePath = filePath;

                docSample.Language = "xml";

                docSample.ClassName = Path.GetFileNameWithoutExtension(filePath);

                docSample.MethodName = methodName;
                docSample.MethodBody = sample.InnerHtml;

                result.Add(docSample);
            }

            //var samples = xmlDoc.DescendantNodes("");

            return result;
        }
    }
}
