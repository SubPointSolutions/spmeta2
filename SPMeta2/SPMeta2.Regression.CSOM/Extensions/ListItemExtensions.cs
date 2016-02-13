using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Regression.CSOM.Extensions
{
    internal static class ListItemExtensions
    {
        public static string GetFileLeafRef(this ListItem item)
        {
            return item.FieldValues["FileLeafRef"] as string;
        }

        public static string GetWikiField(this ListItem pageItem)
        {
            return pageItem["WikiField"] as string;
        }
    }
}
