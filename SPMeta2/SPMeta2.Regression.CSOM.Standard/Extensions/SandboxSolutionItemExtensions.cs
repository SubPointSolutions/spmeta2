using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;

namespace SPMeta2.Regression.CSOM.Standard.Extensions
{
    internal static class SandboxSolutionItemExtensions
    {
        #region methods

        public static string GetSolutionFileName(this ListItem item)
        {
            return item["FileLeafRef"].ToString();
        }

        public static Guid GetSolutionId(this ListItem item)
        {
            return new Guid(item["SolutionId"].ToString());
        }

        public static bool GetSolutionActivationStatus(this ListItem item)
        {
            return (item["Status"] as FieldLookupValue).LookupId == 1;
        }

        #endregion
    }
}
