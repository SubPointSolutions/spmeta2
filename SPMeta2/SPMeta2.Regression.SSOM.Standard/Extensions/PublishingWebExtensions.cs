using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Publishing;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Extensions
{
    public static class PublishingWebExtensions
    {
        public static bool GetInheritWebTemplates(this PublishingWeb publishingWeb)
        {
            return publishingWeb.IsInheritingAvailableWebTemplates;
        }

        public static bool GetConverBlankSpacesIntoHyphen(this PublishingWeb publishingWeb)
        {
            var web = publishingWeb.Web;

            var key = "__AllowSpacesInNewPageName";

            if (!web.AllProperties.ContainsKey(key))
                return false;

            return ConvertUtils.ToBool(web.AllProperties[key]).Value;
        }
    }
}
