using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions;

namespace SPMeta2.Containers.Standard.DefinitionGenerators
{
    public class SearchConfigurationDefinitionGenerator : TypedDefinitionGeneratorServiceBase<SearchConfigurationDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                var searchSchema = SearchTemplates.DefaultSearchConfiguration;

                def.SearchConfiguration = SearchTemplatesUtils.SetSourceNode(searchSchema, Rnd.String(), Rnd.String());
            });
        }
    }

    public static class SearchTemplatesUtils
    {
        private static string d4p1 = "{http://schemas.datacontract.org/2004/07/Microsoft.Office.Server.Search.Administration.Query}";
        private static string sourcesPath = string.Format("{0}Source", d4p1);

        private static string namePath = string.Format("{0}Name", d4p1);
        private static string descriptionPath = string.Format("{0}Description", d4p1);

        public static string SetSourceNode(string searchConfigXml, string sourceName, string sourceDescription)
        {
            var xml = XDocument.Parse(searchConfigXml);

            var sourceNodes = xml.Descendants(sourcesPath);

            var sourceNode = sourceNodes.FirstOrDefault(n => n.Descendants(namePath).FirstOrDefault().Value == "SPMeta2Test");

            var nameNode = sourceNode.Descendants(namePath).FirstOrDefault();
            var descriptionNode = sourceNode.Descendants(descriptionPath).FirstOrDefault();

            nameNode.Value = sourceName;
            descriptionNode.Value = sourceDescription;

            return xml.ToString();
        }

        public static XElement GetSetSourceNode(string searchConfigXml)
        {
            var xml = XDocument.Parse(searchConfigXml);

            return xml.Descendants(sourcesPath).FirstOrDefault();
        }

        public static IEnumerable<XElement> GetSetSourceNodes(string searchConfigXml)
        {
            var xml = XDocument.Parse(searchConfigXml);

            var sourceNodes = xml.Descendants(sourcesPath);
            return sourceNodes.ToList();
        }

        public static string GetSetSourceName(XElement node)
        {
            var nameNode = node.Descendants(namePath).FirstOrDefault();

            return nameNode != null ? nameNode.Value : string.Empty;
        }

        public static string GetSetSourceDescription(XElement node)
        {
            var nameNode = node.Descendants(descriptionPath).FirstOrDefault();

            return nameNode != null ? nameNode.Value : string.Empty;
        }
    }
}
