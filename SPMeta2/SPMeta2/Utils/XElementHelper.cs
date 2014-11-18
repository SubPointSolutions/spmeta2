using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SPMeta2.Utils
{
    /// <summary>
    /// XElement usefulness.
    /// </summary>
    public static class XElementHelper
    {
        #region methods

        public static string GetAttributeValue(this XElement element, string attrName)
        {
            var upperName = attrName.ToUpper();
            var attr = element.Attributes().FirstOrDefault(a => a.Name.ToString().ToUpper() == upperName);

            if (attr != null)
            {
                return attr.Value;
            }
            else
            {
                return string.Empty;
            }
        }

        public static XElement SetAttribute(this XElement element, string attrName, object attrValue)
        {
            var upperName = attrName.ToUpper();
            var attrStringValue = attrValue != null ? attrValue.ToString() : string.Empty;

            var attr = element.Attributes().FirstOrDefault(a => a.Name.ToString().ToUpper() == upperName);

            if (attr != null)
            {
                attr.Value = attrStringValue;
            }
            else
            {
                var newAttr = new XAttribute(attrName, attrStringValue);
                element.Add(newAttr);
            }

            return element;
        }

        public static XElement SetSubNode(this XElement element, string nodeName, string nodeValue)
        {
            var upperName = nodeName.ToUpper();
            var node = element.Elements().FirstOrDefault(a => a.Name.ToString().ToUpper() == upperName);

            if (node == null)
            {
                element.Add(new XElement(nodeName)
                {
                    Value = nodeValue
                });
            }
            else
            {
                node.Value = nodeValue;
            }

            return element;
        }

        #endregion
    }
}
