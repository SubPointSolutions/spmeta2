using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Enumerations
{
    /// <summary>
    /// Builtin SharePoint calendar types.
    /// Reflects Microsoft.SharePoint.SPFieldUserSelectionMode enum.
    /// </summary>
    public static class BuiltInFieldUserSelectionMode
    {
        #region properties

        public static string PeopleOnly = "PeopleOnly";
        public static string PeopleAndGroups = "PeopleAndGroups";

        #endregion
    }
}
