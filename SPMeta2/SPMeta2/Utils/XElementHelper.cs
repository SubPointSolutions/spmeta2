using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SPMeta2.Utils
{
    public static class XElementHelper
    {
        public static XElement SetAttribute(this XElement element, string attrName, string attrValue)
        {
            element.Attribute(attrName).Value = attrValue;

            return element;
        }
    }
}
