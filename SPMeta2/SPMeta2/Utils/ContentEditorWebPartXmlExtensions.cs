using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Linq;

namespace SPMeta2.Utils
{
    /// <summary>
    /// Helper class to work with CEWP xml properties.
    /// Allows to setup ContentLink, Content, PartStorage and other props with xmlns="http://schemas.microsoft.com/WebPart/v2/ContentEditor"
    /// </summary>
    public static class ContentEditorWebPartXmlExtensions
    {
        #region properties

        private const string XmlNs = "http://schemas.microsoft.com/WebPart/v2/ContentEditor";

        #endregion

        #region methods

        public static string GetContentEditorWebPartProperty(this XDocument webpartXmlDocument, string propName)
        {
            return webpartXmlDocument.GetProperty(propName, XmlNs);
        }

        public static XDocument SetOrUpdateContentEditorWebPartProperty(this XDocument webpartXmlDocument,
            string propName,
            string propValue)
        {
            return SetOrUpdateContentEditorWebPartProperty(webpartXmlDocument, propName, propValue, false);
        }

        public static XDocument SetOrUpdateContentEditorWebPartProperty(this XDocument webpartXmlDocument, string propName,
            string propValue, bool isCData)
        {
            return webpartXmlDocument.SetOrUpdateProperty(propName, propValue, XmlNs, isCData);
        }

        #endregion
    }
}
