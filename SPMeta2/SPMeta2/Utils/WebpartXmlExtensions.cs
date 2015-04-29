using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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


        public static XDocument SetNumberOfItems(this XDocument webpartXmlDocument, int value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value.ToString());
        }


        public static XDocument SetResultsPerPage(this XDocument webpartXmlDocument, int value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value.ToString());
        }


        public static XDocument SetRenderTemplateId(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static XDocument SetItemTemplateId(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static XDocument SetGroupTemplateId(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }


        public static XDocument SetDataProviderJSON(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static XDocument SetDescription(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static XDocument SetWidth(this XDocument webpartXmlDocument, int value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value.ToString());
        }

        public static XDocument SetHeight(this XDocument webpartXmlDocument, int value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value.ToString());
        }


        public static XDocument SetImportErrorMessage(this XDocument webpartXmlDocument, string value)
        {
            if (IsV3version(webpartXmlDocument))
                return SetOrUpdateMetadataProperty(webpartXmlDocument, "importErrorMessage", value);
            else if (IsV2version(webpartXmlDocument))
                return SetOrUpdateV2Property(webpartXmlDocument, "MissingAssembly", value, WebPartNamespaceV2, false);

            throw new Exception("http://schemas.microsoft.com/WebPart/v3 or http://schemas.microsoft.com/WebPart/v2 is expected, but missed");
        }
        public static string GetImportErrorMessage(this XDocument webpartXmlDocument)
        {
            if (IsV3version(webpartXmlDocument))
                return GetV3MetadataNode(webpartXmlDocument, "importErrorMessage", WebPartNamespaceV3).Value;
            else if (IsV2version(webpartXmlDocument))
                return GetProperty(webpartXmlDocument, "MissingAssembly");

            throw new Exception("http://schemas.microsoft.com/WebPart/v3 or http://schemas.microsoft.com/WebPart/v2 is expected, but missed");
        }

        public static XDocument SetPrimaryTaskListUrl(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static XDocument SetChromeType(this XDocument webpartXmlDocument, string value)
        {
            if (IsV3version(webpartXmlDocument))
                return SetOrUpdateV3Property(webpartXmlDocument, "ChromeType", value, WebPartNamespaceV3, false);
            else if (IsV2version(webpartXmlDocument))
                return SetOrUpdateV2Property(webpartXmlDocument, "FrameType", value, WebPartNamespaceV2, false);

            throw new Exception("http://schemas.microsoft.com/WebPart/v3 or http://schemas.microsoft.com/WebPart/v2 is expected, but missed");
        }

        public static string GetChromeType(this XDocument webpartXmlDocument)
        {
            if (IsV3version(webpartXmlDocument))
                return GetProperty(webpartXmlDocument, "ChromeType", WebPartNamespaceV3);
            else if (IsV2version(webpartXmlDocument))
                return GetProperty(webpartXmlDocument, "FrameType", WebPartNamespaceV2);

            throw new Exception("http://schemas.microsoft.com/WebPart/v3 or http://schemas.microsoft.com/WebPart/v2 is expected, but missed");
        }

        public static XDocument SetChromeState(this XDocument webpartXmlDocument, string value)
        {
            if (IsV3version(webpartXmlDocument))
                return SetOrUpdateV3Property(webpartXmlDocument, "ChromeState", value, WebPartNamespaceV3, false);
            else if (IsV2version(webpartXmlDocument))
                return SetOrUpdateV2Property(webpartXmlDocument, "FrameState", value, WebPartNamespaceV2, false);

            throw new Exception("http://schemas.microsoft.com/WebPart/v3 or http://schemas.microsoft.com/WebPart/v2 is expected, but missed");
        }

        public static string GetChromeState(this XDocument webpartXmlDocument)
        {
            if (IsV3version(webpartXmlDocument))
                return GetProperty(webpartXmlDocument, "ChromeState", WebPartNamespaceV3);
            else if (IsV2version(webpartXmlDocument))
                return GetProperty(webpartXmlDocument, "FrameState", WebPartNamespaceV2);

            throw new Exception("http://schemas.microsoft.com/WebPart/v3 or http://schemas.microsoft.com/WebPart/v2 is expected, but missed");
        }

        public static XDocument SetTitleUrl(this XDocument webpartXmlDocument, string value)
        {
            if (IsV3version(webpartXmlDocument))
                return SetOrUpdateV3Property(webpartXmlDocument, "TitleUrl", value, WebPartNamespaceV3, false);
            else if (IsV2version(webpartXmlDocument))
                return SetOrUpdateV2Property(webpartXmlDocument, "DetailLink", value, WebPartNamespaceV2, false);

            throw new Exception("http://schemas.microsoft.com/WebPart/v3 or http://schemas.microsoft.com/WebPart/v2 is expected, but missed");
        }

        public static string GetTitleUrl(this XDocument webpartXmlDocument)
        {
            if (IsV3version(webpartXmlDocument))
                return GetProperty(webpartXmlDocument, "TitleIconImageUrl", WebPartNamespaceV3);
            else if (IsV2version(webpartXmlDocument))
                return GetProperty(webpartXmlDocument, "PartImageSmall", WebPartNamespaceV2);

            throw new Exception("http://schemas.microsoft.com/WebPart/v3 or http://schemas.microsoft.com/WebPart/v2 is expected, but missed");
        }

        public static XDocument SetTitleIconImageUrl(this XDocument webpartXmlDocument, string value)
        {
            if (IsV3version(webpartXmlDocument))
                return SetOrUpdateV3Property(webpartXmlDocument, "TitleIconImageUrl", value, WebPartNamespaceV3, false);
            else if (IsV2version(webpartXmlDocument))
                return SetOrUpdateV2Property(webpartXmlDocument, "PartImageSmall", value, WebPartNamespaceV2, false);

            throw new Exception("http://schemas.microsoft.com/WebPart/v3 or http://schemas.microsoft.com/WebPart/v2 is expected, but missed");
        }

        public static string GetTitleIconImageUrl(this XDocument webpartXmlDocument)
        {
            if (IsV3version(webpartXmlDocument))
                return GetProperty(webpartXmlDocument, "TitleIconImageUrl", WebPartNamespaceV3);
            else if (IsV2version(webpartXmlDocument))
                return GetProperty(webpartXmlDocument, "PartImageSmall", WebPartNamespaceV2);

            throw new Exception("http://schemas.microsoft.com/WebPart/v3 or http://schemas.microsoft.com/WebPart/v2 is expected, but missed");
        }

        public static XDocument SetListId(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static string GetListId(this XDocument webpartXmlDocument)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Get", string.Empty);
            return GetProperty(webpartXmlDocument, name);
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

        public static XDocument SetJSLink(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static string GetJSLink(this XDocument webpartXmlDocument)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Get", string.Empty);
            return GetProperty(webpartXmlDocument, name);
        }

        public static XDocument SetListName(this XDocument webpartXmlDocument, string value)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Set", string.Empty);
            return SetOrUpdateProperty(webpartXmlDocument, name, value);
        }

        public static string GetListName(this XDocument webpartXmlDocument)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Get", string.Empty);
            return GetProperty(webpartXmlDocument, name);
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

        internal static XElement GetV3MetadataNode(this XDocument webpartXmlDocument, string propName, string propXlmns)
        {
            var mNode = webpartXmlDocument.Descendants("{" + WebPartNamespaceV3 + "}metaData").FirstOrDefault();
            var namespacedPropName = "{" + WebPartNamespaceV3 + "}" + propName;

            return mNode.Descendants()
                                            .FirstOrDefault(e => e.Name.ToString().ToUpper() == namespacedPropName.ToUpper());
        }

        internal static XElement GetV3Node(this XDocument webpartXmlDocument, string propName, string propXlmns)
        {
            var propsNode = webpartXmlDocument.Descendants("{" + WebPartNamespaceV3 + "}properties").FirstOrDefault();

            var propNodePath = "{" + propXlmns + "}property";
            return propsNode.Descendants(propNodePath)
                                            .FirstOrDefault(e => e.Attribute("name") != null && e.Attribute("name").Value == propName);
        }

        internal static XDocument SetOrUpdateV3MetadataProperty(this XDocument webpartXmlDocument, string propName,
            string propValue, string propXlmns, bool isCData)
        {
            var propsNode = webpartXmlDocument.Descendants("{" + WebPartNamespaceV3 + "}metaData").FirstOrDefault();
            var propNode = GetV3MetadataNode(webpartXmlDocument, propName, propXlmns);

            if (propNode == null)
            {
                var newNode = (new XElement("{" + WebPartNamespaceV3 + "}" + propName));

                newNode.SetAttributeValue("name", propName);

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

        internal static XDocument SetOrUpdateV3MetadataPropertyAttribute(this XDocument webpartXmlDocument,
            string propName,
            string attrName,
            string attrValue,
            string propXlmns, bool isCData)
        {
            var propsNode = webpartXmlDocument.Descendants("{" + WebPartNamespaceV3 + "}metaData").FirstOrDefault();
            var propNode = GetV3MetadataNode(webpartXmlDocument, propName, propXlmns);

            if (propNode == null)
            {
                var newNode = (new XElement("{" + WebPartNamespaceV3 + "}" + propName));

                if (propName.ToUpper() == "SOLUTION")
                {
                    newNode = (new XElement("{http://schemas.microsoft.com/sharepoint/}" + propName));
                    newNode.SetAttributeValue("xmlns", "http://schemas.microsoft.com/sharepoint/");
                }

                //newNode.SetAttributeValue("name", propName);
                newNode.SetAttributeValue(attrName, attrValue);

                propsNode.Add(newNode);
            }
            else
            {
                propNode.SetAttributeValue(attrName, attrValue);
            }

            return webpartXmlDocument;
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
                    propNode.ReplaceNodes(new XCData(propValue ?? string.Empty));
                    propNode.SetAttributeValue("null", string.IsNullOrEmpty(propValue));
                }
                else
                {
                    propNode.Value = propValue ?? string.Empty;
                    propNode.SetAttributeValue("null", string.IsNullOrEmpty(propValue));
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

        public static XDocument SetOrUpdateMetadataPropertyAttribute(this XDocument webpartXmlDocument,
            string propName,
            string attrName, string attrValue)
        {
            return SetOrUpdateMetadataPropertyAttribute(webpartXmlDocument, propName, attrName, attrValue, false);
        }

        public static XDocument SetOrUpdateMetadataPropertyAttribute(this XDocument webpartXmlDocument,
            string propName,
            string attrName, string attrValue,
            bool isCData)
        {
            if (IsV3version(webpartXmlDocument))
                return SetOrUpdateV3MetadataPropertyAttribute(webpartXmlDocument, propName, attrName, attrValue, WebPartNamespaceV3, isCData);

            return webpartXmlDocument;
        }

        public static XDocument SetOrUpdateMetadataProperty(this XDocument webpartXmlDocument, string propName,
            string propValue)
        {
            return SetOrUpdateMetadataProperty(webpartXmlDocument, propName, propValue, false);
        }

        public static XDocument SetOrUpdateMetadataProperty(this XDocument webpartXmlDocument, string propName, string propValue, bool isCData)
        {
            if (IsV3version(webpartXmlDocument))
                return SetOrUpdateV3MetadataProperty(webpartXmlDocument, propName, propValue, WebPartNamespaceV3, isCData);

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
