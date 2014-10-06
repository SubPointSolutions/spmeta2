using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SPMeta2.Utils
{
    /// <summary>
    /// Helper class to work with webpart xml files.
    /// Support v2/v3 XML definitision.
    /// </summary>
    public static class WebpartXmlExtensions
    {
        #region properties

        private const string WebPartNamespaceV3 = "http://schemas.microsoft.com/WebPart/v3";
        private const string WebPartNamespaceV2 = "http://schemas.microsoft.com/WebPart/v2";

        #endregion

        #region api

        public static XDocument LoadWebpartXmlDocument(string value)
        {
            return XDocument.Parse(value);
        }

        public static XDocument SetChromeType(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static XDocument SetPrimaryTaskListUrl(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static XDocument SetTitleUrl(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static XDocument SetListId(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static XDocument SetShowTimelineIfAvailable(this XDocument webpartXmlDocument, bool value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value.ToString());
        }

        public static XDocument SetTitle(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static XDocument SetXmlDefinition(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static XDocument SetWebUrl(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static XDocument SetListName(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static XDocument SetFilterDisplayValue1(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static XDocument SetDataMappingViewFields(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static XDocument SetItemStyle(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static XDocument SetListGuid(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static XDocument SetDataMappings(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static XDocument SetFilterValue1(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static XDocument BindXsltListViewWebPartToList(this XDocument webpartXmlDocument, string listId)
        {
            return webpartXmlDocument
                      .SetListGuid(listId)
                      .SetListId(listId)
                      .SetListName(listId);
        }


        #endregion

        #region utils

        #region version routines

        private static bool IsV3version(this XDocument webpartXmlDocument)
        {
            var webPartNode = webpartXmlDocument.Descendants("{" + WebPartNamespaceV3 + "}WebPart").FirstOrDefault();

            if (webPartNode == null)
                webPartNode = webpartXmlDocument.Descendants("{" + WebPartNamespaceV3 + "}webPart").FirstOrDefault();

            return webPartNode != null;
        }

        private static bool IsV2version(this XDocument webpartXmlDocument)
        {
            var webPartNode = webpartXmlDocument.Descendants("{" + WebPartNamespaceV2 + "}WebPart").FirstOrDefault();

            if (webPartNode == null)
                webPartNode = webpartXmlDocument.Descendants("{" + WebPartNamespaceV2 + "}webPart").FirstOrDefault();

            return webPartNode != null;
        }

        #endregion

        #region set routines

        private static XElement GetV3Node(this XDocument webpartXmlDocument, string propName)
        {
            var propsNode = webpartXmlDocument.Descendants("{" + WebPartNamespaceV3 + "}properties").FirstOrDefault();

            var propNodePath = "{" + WebPartNamespaceV3 + "}property";
            return propsNode.Descendants(propNodePath)
                                            .FirstOrDefault(e => e.Attribute("name") != null && e.Attribute("name").Value == propName);
        }

        private static XDocument SetOrUpdateV3Property(this XDocument webpartXmlDocument, string propName, string propValue, bool isCData)
        {
            var propsNode = webpartXmlDocument.Descendants("{" + WebPartNamespaceV3 + "}properties").FirstOrDefault();
            var propNode = GetV3Node(webpartXmlDocument, propName);

            if (propNode == null)
            {
                var newNode = (new XElement("{" + WebPartNamespaceV3 + "}property"));

                newNode.SetAttributeValue("name", propName);
                newNode.SetAttributeValue("type", "string");

                if (isCData)
                {
                    newNode.Add(new XCData(propValue));
                }
                else
                {
                    newNode.Value = propValue;
                }

                propsNode.Add(newNode);
            }
            else
            {
                if (isCData)
                {
                    propNode.ReplaceNodes(new XCData(propValue));
                }
                else
                {
                    propNode.Value = propValue;
                }
            }

            return webpartXmlDocument;
        }

        private static XElement GetV2Node(this XDocument webpartXmlDocument, string propName)
        {
            var webPartNode = webpartXmlDocument.Descendants("{" + WebPartNamespaceV2 + "}WebPart").FirstOrDefault();
            if (webPartNode == null) throw new ArgumentException("Web part xml template is very wrong");

            var propNodePath = "{" + WebPartNamespaceV2 + "}" + propName;
            return webPartNode.Descendants(propNodePath).FirstOrDefault();
        }

        private static XDocument SetOrUpdateV2Property(this XDocument webpartXmlDocument, string propName, string propValue, bool isCData)
        {
            var webPartNode = webpartXmlDocument.Descendants("{" + WebPartNamespaceV2 + "}WebPart").FirstOrDefault();

            var propNodePath = "{" + WebPartNamespaceV2 + "}" + propName;
            var propNode = GetV2Node(webpartXmlDocument, propName);

            if (propNode == null)
            {
                var newNode = new XElement(propNodePath);

                if (isCData)
                {
                    newNode.Add(new XCData(propValue));
                }
                else
                {
                    newNode.Value = propValue;
                }

                webPartNode.Add(newNode);
            }
            else
            {
                if (isCData)
                {
                    propNode.ReplaceNodes(new XCData(propValue));
                }
                else
                {
                    propNode.Value = propValue;
                }
            }

            return webpartXmlDocument;
        }

        public static XDocument SetOrUpdateProperty(this XDocument webpartXmlDocument, string propName, string propValue, bool isCData)
        {
            if (IsV3version(webpartXmlDocument))
                SetOrUpdateV3Property(webpartXmlDocument, propName, propValue, isCData);
            else if (IsV2version(webpartXmlDocument))
                SetOrUpdateV2Property(webpartXmlDocument, propName, propValue, isCData);
            else
                throw new Exception("http://schemas.microsoft.com/WebPart/v3 or http://schemas.microsoft.com/WebPart/v2 is expected, but missed");

            return webpartXmlDocument;
        }

        public static XDocument SetOrUpdateProperty(this XDocument webpartXmlDocument, string propName, string propValue)
        {
            return SetOrUpdateProperty(webpartXmlDocument, propName, propValue, false);
        }

        public static XDocument SetOrUpdateCDataProperty(this XDocument webpartXmlDocument, string propName, string propValue)
        {
            return SetOrUpdateProperty(webpartXmlDocument, propName, propValue, true);
        }

        #endregion

        #region get routines

        public static string GetProperty(this XDocument webpartXmlDocument, string propName)
        {
            XElement propNode = null;

            if (IsV3version(webpartXmlDocument))
                propNode = GetV3Node(webpartXmlDocument, propName);
            else if (IsV2version(webpartXmlDocument))
                propNode = GetV2Node(webpartXmlDocument, propName);
            else
                throw new Exception("http://schemas.microsoft.com/WebPart/v3 or http://schemas.microsoft.com/WebPart/v2 is expected, but missed");

            if (propNode != null)
                return propNode.Value;

            return string.Empty;
        }

        #endregion

        #endregion
    }
}
