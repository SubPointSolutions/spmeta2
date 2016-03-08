using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;

namespace SPMeta2.Regression.CSOM.Extensions
{
    public static class MasterPageItemExtensions
    {
        public static List<string> GetUIVersion(this ListItem item)
        {
            var result = new List<string>();

            var values = item["UIVersion"] as string[];

            if (values != null && values.Length > 0)
                result.AddRange(values);

            return result;
        }

        public static string GetTitle(this ListItem item)
        {
            return item["Title"] as string;
        }

        public static string GetContentTypeName(this ListItem item)
        {
            return item.ContentType.Name;
        }

        public static string GetDefaultCSSFile(this ListItem item)
        {
            return item["DefaultCssFile"] as string;
        }

        public static string GetFileName(this ListItem item)
        {
            return item["FileLeafRef"] as string;
        }

        public static string GetPublishingPageDescription(this ListItem item)
        {
            return item["Comments"] as string;
        }

        public static string GetPublishingPagePageLayoutFileName(this ListItem item)
        {
            var result = item["PublishingPageLayout"] as FieldUrlValue;

            if (result != null)
                return result.Url;

            return string.Empty;
        }

        public static string GetMasterPageDescription(this ListItem item)
        {
            return item["MasterPageDescription"] as string;
        }
    }
}
