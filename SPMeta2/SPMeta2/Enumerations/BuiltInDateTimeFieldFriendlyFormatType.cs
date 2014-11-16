using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Enumerations
{
    /// <summary>
    /// Builtin SharePoint datetime format type.
    /// Reflects Microsoft.SharePoint.SPDateTimeFieldFriendlyFormatType enum.
    /// </summary>
    public static class BuiltInDateTimeFieldFriendlyFormatType
    {
        #region properties

        public static string Unspecified = "Unspecified";
        public static string Disabled = "Disabled";
        public static string Relative = "Relative";

        #endregion
    }
}
