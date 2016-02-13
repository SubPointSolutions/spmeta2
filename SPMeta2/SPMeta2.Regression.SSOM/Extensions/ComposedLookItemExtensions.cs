using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Extensions
{
    internal static class ComposedLookItemExtensions
    {

        public static string GetComposedLookName(this SPListItem item)
        {
            return item["Name"] as string;
        }

        public static SPFieldUrlValue GetComposedLookMasterPageUrl(this SPListItem item)
        {
            return ConvertToSPFieldUrlValue(item["MasterPageUrl"]);
        }

        private static SPFieldUrlValue ConvertToSPFieldUrlValue(object value)
        {
            var stringValue = ConvertUtils.ToString(value);

            if (!string.IsNullOrEmpty(stringValue))
                return new SPFieldUrlValue(stringValue);

            return null;
        }

        public static SPFieldUrlValue GetComposedLookThemeUrl(this SPListItem item)
        {
            return ConvertToSPFieldUrlValue(item["ThemeUrl"]);
        }

        public static SPFieldUrlValue GetComposedLookImageUrl(this SPListItem item)
        {
            return ConvertToSPFieldUrlValue(item["ImageUrl"]);
        }

        public static SPFieldUrlValue GetComposedLookFontSchemeUrl(this SPListItem item)
        {
            return ConvertToSPFieldUrlValue(item["FontSchemeUrl"]);
        }

        public static int? GetComposedLookDisplayOrder(this SPListItem item)
        {
            return ConvertUtils.ToInt(item["DisplayOrder"]);
        }

    }
}
