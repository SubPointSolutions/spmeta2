using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Standard.Enumerations;

namespace SPMeta2.Regression.SSOM.Standard.Extensions
{
    internal static class PublishingPageItemExtensions
    {
        public static string GetPublishingPageDescription(this SPListItem item)
        {
            return item[BuiltInPublishingFieldId.Description] as string;
        }

        public static string GetPublishingPagePageLayoutFileName(this SPListItem item)
        {
            return (new SPFieldUrlValue(item[BuiltInPublishingFieldId.PageLayout].ToString())).Url;
        }

        public static string GetPublishingPageContentType(this SPListItem item)
        {
            return item["ContentType"].ToString();
        }
    }

}
