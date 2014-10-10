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

        public static XDocument SetID(this XDocument webpartXmlDocument, string value)
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

        internal static XElement GetV3Node(this XDocument webpartXmlDocument, string propName)
        {
            return GetV3Node(webpartXmlDocument, propName, WebPartNamespaceV3);
        }

        internal static XElement GetV3Node(this XDocument webpartXmlDocument, string propName, string propXlmns)
        {
            var propsNode = webpartXmlDocument.Descendants("{" + WebPartNamespaceV3 + "}properties").FirstOrDefault();

            var propNodePath = "{" + propXlmns + "}property";
            return propsNode.Descendants(propNodePath)
                                            .FirstOrDefault(e => e.Attribute("name") != null && e.Attribute("name").Value == propName);
        }

        internal static XDocument SetOrUpdateV3Property(this XDocument webpartXmlDocument, string propName, string propValue, string propXlmns, bool isCData)
        {
            var propsNode = webpartXmlDocument.Descendants("{" + WebPartNamespaceV3 + "}properties").FirstOrDefault();
            var propNode = GetV3Node(webpartXmlDocument, propName, propXlmns);

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
                    newNode.Value = propValue ?? string.Empty;
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
                    propNode.Value = propValue ?? string.Empty;
                }
            }

            return webpartXmlDocument;
        }

        internal static XElement GetV2Node(this XDocument webpartXmlDocument, string propName)
        {
            return GetV2Node(webpartXmlDocument, propName, WebPartNamespaceV2);
        }

        internal static XElement GetV2Node(this XDocument webpartXmlDocument, string propName, string propXlmns)
        {
            var webPartNode = webpartXmlDocument.Descendants("{" + WebPartNamespaceV2 + "}WebPart").FirstOrDefault();
            if (webPartNode == null) throw new ArgumentException("Web part xml template is very wrong");

            var propNodePath = "{" + propXlmns + "}" + propName;
            return webPartNode.Descendants(propNodePath).FirstOrDefault();
        }


        internal static XDocument SetOrUpdateV2Property(this XDocument webpartXmlDocument, string propName, string propValue, string propXlmns, bool isCData)
        {
            var webPartNode = webpartXmlDocument.Descendants("{" + WebPartNamespaceV2 + "}WebPart").FirstOrDefault();

            var propNodePath = "{" + propXlmns + "}" + propName;
            var propNode = GetV2Node(webpartXmlDocument, propName, propXlmns);

            if (propNode == null)
            {
                var newNode = new XElement(propNodePath);

                if (isCData)
                {
                    newNode.Add(new XCData(propValue));
                }
                else
                {
                    newNode.Value = propValue ?? string.Empty;
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
                    propNode.Value = propValue ?? string.Empty;
                }
            }

            return webpartXmlDocument;
        }

        public static XDocument SetOrUpdateProperty(this XDocument webpartXmlDocument, string propName, string propValue,
            bool isCData)
        {
            if (IsV3version(webpartXmlDocument))
                return SetOrUpdateV3Property(webpartXmlDocument, propName, propValue, WebPartNamespaceV3, isCData);
            else if (IsV2version(webpartXmlDocument))
                return SetOrUpdateV2Property(webpartXmlDocument, propName, propValue, WebPartNamespaceV2, isCData);

            throw new Exception("http://schemas.microsoft.com/WebPart/v3 or http://schemas.microsoft.com/WebPart/v2 is expected, but missed");
        }

        internal static XDocument SetOrUpdateProperty(this XDocument webpartXmlDocument, string propName, string propValue, string propXlmns, bool isCData)
        {
            if (IsV3version(webpartXmlDocument))
                SetOrUpdateV3Property(webpartXmlDocument, propName, propValue, propXlmns, isCData);
            else if (IsV2version(webpartXmlDocument))
                SetOrUpdateV2Property(webpartXmlDocument, propName, propValue, propXlmns, isCData);
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
            if (IsV3version(webpartXmlDocument))
                return GetProperty(webpartXmlDocument, propName, WebPartNamespaceV3);
            else if (IsV2version(webpartXmlDocument))
                return GetProperty(webpartXmlDocument, propName, WebPartNamespaceV2);

            throw new Exception("http://schemas.microsoft.com/WebPart/v3 or http://schemas.microsoft.com/WebPart/v2 is expected, but missed");
        }

        internal static string GetProperty(this XDocument webpartXmlDocument, string propName, string xlmns)
        {
            XElement propNode = null;

            if (IsV3version(webpartXmlDocument))
                propNode = GetV3Node(webpartXmlDocument, propName, xlmns);
            else if (IsV2version(webpartXmlDocument))
                propNode = GetV2Node(webpartXmlDocument, propName, xlmns);
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
