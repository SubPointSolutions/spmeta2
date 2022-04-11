using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;

namespace SPMeta2.Regression.SSOM.Extensions
{
    public static class MasterPageItemExtensions
    {
        public static List<string> GetUIVersion(this SPListItem item)
        {
            var v = new SPFieldMultiChoiceValue(item["UIVersion"] as string);
            var result = new List<string>();

            for (var i = 0; i < v.Count; i++)
                result.Add(v[i]);

            return result;
        }

        public static string GetMasterPageDescription(this SPListItem item)
        {
            return item["MasterPageDescription"] as string;
        }

        public static string GetDefaultCSSFile(this SPListItem item)
        {
            return item["DefaultCssFile"] as string;
        }
    }
}
