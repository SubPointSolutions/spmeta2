using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;

namespace SPMeta2.Regression.CSOM.Standard.Extensions
{
    internal static class PublishingPageLayoutItemExtensions
    {
        public static string GetPublishingPageLayoutDescription(this ListItem item)
        {
            return item["MasterPageDescription"] as string;
        }

        public static string GetPublishingPageLayoutAssociatedContentTypeId(this ListItem item)
        {
            var value = item["PublishingAssociatedContentType"].ToString();
            var values = value.Split(new string[] { ";#" }, StringSplitOptions.None);

            return values[2];
        }

        public static string GetPublishingPageLayoutAssociatedContentTypeName(this ListItem item)
        {
            var value = item["PublishingAssociatedContentType"].ToString();
            var values = value.Split(new string[] { ";#" }, StringSplitOptions.None);

            return values[1];
        }
    }
}
