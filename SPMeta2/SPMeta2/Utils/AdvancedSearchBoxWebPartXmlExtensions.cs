using System.Xml.Linq;

namespace SPMeta2.Utils
{
    /// <summary>
    /// Helper class to work with AdvancedSearchBoxWebPart xml properties.
    /// Allows to props with xmlns="urn:schemas-microsoft-com:AdvancedSearchBox"
    /// </summary>
    public static class AdvancedSearchBoxWebPartXmlExtensions
    {
        #region properties

        private const string XmlNs = "urn:schemas-microsoft-com:AdvancedSearchBox";

        #endregion

        #region methods

        public static string GetAdvancedSearchBoxWebPartProperty(this XDocument webpartXmlDocument, string propName)
        {
            return webpartXmlDocument.GetProperty(propName, XmlNs);
        }

        public static XDocument SetOrUpdateAdvancedSearchBoxWebPartProperty(this XDocument webpartXmlDocument,
            string propName,
            string propValue)
        {
            return SetOrUpdateAdvancedSearchBoxWebPartProperty(webpartXmlDocument, propName, propValue, false);
        }

        public static XDocument SetOrUpdateAdvancedSearchBoxWebPartProperty(this XDocument webpartXmlDocument, string propName,
            string propValue, bool isCData)
        {
            return webpartXmlDocument.SetOrUpdateProperty(propName, propValue, "string", XmlNs, isCData);
        }

        #endregion
    }
}
