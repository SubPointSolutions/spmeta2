using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Extensions
{
    internal static class ComposedLookItemExtensions
    {
        public static string GetComposedLookName(this ListItem item)
        {
            return item["Name"] as string;
        }

        public static int? GetComposedLookDisplayOrder(this ListItem item)
        {
            return ConvertUtils.ToInt(item["DisplayOrder"]);
        }

        public static FieldUrlValue GetComposedLookFontSchemeUrl(this ListItem item)
        {
            if (item["FontSchemeUrl"] != null)
                return item["FontSchemeUrl"] as FieldUrlValue;

            return null;
        }

        public static FieldUrlValue GetComposedLookImageUrl(this ListItem item)
        {
            if (item["ImageUrl"] != null)
                return item["ImageUrl"] as FieldUrlValue;

            return null;
        }

        public static FieldUrlValue GetComposedLookMasterPageUrl(this ListItem item)
        {
            if (item["MasterPageUrl"] != null)
                return item["MasterPageUrl"] as FieldUrlValue;

            return null;
        }

        public static FieldUrlValue GetComposedLookThemeUrl(this ListItem item)
        {
            if (item["ThemeUrl"] != null)
                return item["ThemeUrl"] as FieldUrlValue;

            return null;
        }
    }
}
