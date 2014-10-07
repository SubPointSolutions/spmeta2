using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SPMeta2.Utils
{
    /// <summary>
    /// Helper class to work with CEWP xml properties.
    /// Allows to setup WebId, ListViewXml and other props with xmlns="http://schemas.microsoft.com/WebPart/v2/ListView"
    /// </summary>
    public static class ListViewWebPartXmlExtensions
    {
        #region properties

        private const string XmlNs = "http://schemas.microsoft.com/WebPart/v2/ListView";

        #endregion

        #region methods

        public static string GetListViewWebPartProperty(this XDocument webpartXmlDocument, string propName)
        {
            return webpartXmlDocument.GetProperty(propName, XmlNs);
        }

        public static XDocument SetOrUpdateListVieweWebPartProperty(this XDocument webpartXmlDocument,
            string propName,
            string propValue)
        {
            return SetOrUpdateListVieweWebPartProperty(webpartXmlDocument, propName, propValue, false);
        }

        public static XDocument SetOrUpdateListVieweWebPartProperty(this XDocument webpartXmlDocument, string propName,
            string propValue, bool isCData)
        {
            return webpartXmlDocument.SetOrUpdateProperty(propName, propValue, XmlNs, isCData);
        }

        #endregion
    }
}
