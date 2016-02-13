using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Standard.Enumerations;

namespace SPMeta2.Regression.SSOM.Standard.Extensions
{
    internal static class PublishingPageLayoutItemExtensions
    {
        public static string GetPublishingPageLayoutDescription(this SPListItem item)
        {
            return item["MasterPageDescription"] as string;
        }

        public static string GetPublishingPageLayoutAssociatedContentTypeId(this SPListItem item)
        {
            var value = item[BuiltInPublishingFieldId.AssociatedContentType].ToString();
            var values = value.Split(new string[] { ";#" }, StringSplitOptions.None);

            return values[2];
        }

        public static string GetPublishingPageLayoutAssociatedContentTypeName(this SPListItem item)
        {
            var value = item[BuiltInPublishingFieldId.AssociatedContentType].ToString();
            var values = value.Split(new string[] { ";#" }, StringSplitOptions.None);

            return values[1];
        }
    }
}
