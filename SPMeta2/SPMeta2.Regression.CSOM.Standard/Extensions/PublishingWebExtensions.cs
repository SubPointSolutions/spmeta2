using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Standard.ModelHandlers;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Extensions
{
    internal static class PublishingWebExtensions
    {
        public class PageLayoutXmlItem
        {
            public Guid UniqueId { get; set; }
            public string Url { get; set; }

            public string Name { get; set; }
        }

        private static PageLayoutXmlItem GetPageLyoutNameFromXml(string pageLayoutXml)
        {
            var result = new List<PageLayoutXmlItem>();

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(pageLayoutXml);

            if (xmlDocument.FirstChild != null)
                return ConvertXmlNodeToItem(xmlDocument.FirstChild);

            return null;
        }

        private static List<PageLayoutXmlItem> GetPageLyoutNamesFromXml(string pageLayoutXml)
        {
            var result = new List<PageLayoutXmlItem>();

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(pageLayoutXml);

            var documentElement = xmlDocument.DocumentElement;

            for (XmlNode xmlNode = documentElement.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
                result.Add(ConvertXmlNodeToItem(xmlNode));

            return result;
        }

        private static PageLayoutXmlItem ConvertXmlNodeToItem(XmlNode xmlNode)
        {
            string guid = xmlNode.Attributes["guid"].Value;
            string url = xmlNode.Attributes["url"].Value;

            var item = new PageLayoutXmlItem
            {
                Url = url,
                UniqueId = new Guid(guid)
            };
            return item;
        }

        public static PageLayoutXmlItem GetDefaultPageLayoutName(this Web web, IEnumerable<ListItem> pageLayouts)
        {
            var value = ConvertUtils.ToString(PageLayoutAndSiteTemplateSettingsModelHandler.GetPropertyBagValue(web, "__DefaultPageLayout"));
            var pageNameItem = GetPageLyoutNameFromXml(value);

            if (pageNameItem != null)
            {
                var pageLayoutUniqueId = pageNameItem.UniqueId;
                pageNameItem.Name = GetLayoutFileNameByUniqueId(pageLayouts, pageLayoutUniqueId);

                return pageNameItem;
            }

            return null;
        }

        public static string GetLayoutFileNameByUniqueId(IEnumerable<ListItem> pageLayouts, Guid uniqueId)
        {
            var pageLayoutItem = pageLayouts.FirstOrDefault(i => new Guid(i[BuiltInInternalFieldNames.UniqueId].ToString()) == uniqueId);

            if (pageLayoutItem != null)
                return ConvertUtils.ToString(pageLayoutItem[BuiltInInternalFieldNames.FileLeafRef]);

            return string.Empty;
        }

        public static List<PageLayoutXmlItem> GetAvailablePageLayoutNames(this Web web, IEnumerable<ListItem> pageLayouts)
        {
            var value = ConvertUtils.ToString(PageLayoutAndSiteTemplateSettingsModelHandler.GetPropertyBagValue(web, "__PageLayouts"));
            var pageNameItems = GetPageLyoutNamesFromXml(value);

            foreach (var pageItem in pageNameItems)
            {
                var pageLayoutUniqueId = pageItem.UniqueId;
                pageItem.Name = GetLayoutFileNameByUniqueId(pageLayouts, pageLayoutUniqueId);
            }

            return pageNameItems;
        }

        public static bool GetIsInheritingAvailablePageLayouts(this Web web)
        {
            var value = ConvertUtils.ToString(PageLayoutAndSiteTemplateSettingsModelHandler.GetPropertyBagValue(web, "__DefaultPageLayout"));

            if (string.IsNullOrEmpty(value))
                return false;

            return value.ToUpper() == "__inherit".ToUpper();
        }

        public static bool GetIsAllowingAllPageLayouts(this Web web)
        {
            var value = ConvertUtils.ToString(PageLayoutAndSiteTemplateSettingsModelHandler.GetPropertyBagValue(web, "__PageLayouts"));

            if (string.IsNullOrEmpty(value))
                return false;

            return value.ToUpper() == string.Empty;
        }

        public static bool GetInheritingAvailablePageLayouts(this Web web)
        {
            var value = ConvertUtils.ToString(PageLayoutAndSiteTemplateSettingsModelHandler.GetPropertyBagValue(web, "__PageLayouts"));

            if (string.IsNullOrEmpty(value))
                return false;

            return value.ToUpper() == "__inherit".ToUpper();
        }

        public static bool GetInheritWebTemplates(this Web publishingWeb)
        {
            throw new NotImplementedException();
        }

        public static bool GetConverBlankSpacesIntoHyphen(this Web publishingWeb)
        {
            var value = ConvertUtils.ToString(PageLayoutAndSiteTemplateSettingsModelHandler.GetPropertyBagValue(publishingWeb, "__AllowSpacesInNewPageName"));

            if (string.IsNullOrEmpty(value))
                return false;

            return ConvertUtils.ToBool(value).Value;
        }
    }
}
