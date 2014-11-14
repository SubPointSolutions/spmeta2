using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Enumerations
{
    /// <summary>
    /// Builtin SharePoint datetime format type.
    /// Reflects Microsoft.SharePoint.SPDateTimeFieldFormatType enum.
    /// </summary>
    public static class BuiltInDateTimeFieldFormatType
    {
        #region properties

        public static string DateOnly = "DateOnly";
        public static string DateTime = "DateTime";

        #endregion
    }
}
