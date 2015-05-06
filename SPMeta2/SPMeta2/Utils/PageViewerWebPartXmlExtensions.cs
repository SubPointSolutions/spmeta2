using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Linq;

namespace SPMeta2.Utils
{
    /// <summary>
    /// Helper class to work with PageViewerWebPart xml properties.
    /// Allows to setup ContentLink, SourceType and other props with xmlns="http://schemas.microsoft.com/WebPart/v2/PageViewer"
    /// </summary>
    public static class PageViewerWebPartXmlExtensions
    {
        #region properties

        private const string XmlNs = "http://schemas.microsoft.com/WebPart/v2/PageViewer";

        #endregion

        #region methods

        public static string GetPageViewerWebPartProperty(this XDocument webpartXmlDocument, string propName)
        {
            return webpartXmlDocument.GetProperty(propName, XmlNs);
        }

        public static XDocument SetOrUpdatePageViewerWebPartProperty(this XDocument webpartXmlDocument,
            string propName,
            string propValue)
        {
            return SetOrUpdatePageViewerWebPartProperty(webpartXmlDocument, propName, propValue, false);
        }

        public static XDocument SetOrUpdatePageViewerWebPartProperty(this XDocument webpartXmlDocument, string propName,
            string propValue, bool isCData)
        {
            return webpartXmlDocument.SetOrUpdateProperty(propName, propValue, XmlNs, isCData);
        }

        #endregion
    }
}
