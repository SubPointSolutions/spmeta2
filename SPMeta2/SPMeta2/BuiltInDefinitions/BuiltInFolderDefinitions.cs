using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.BuiltInDefinitions
{
    /// <summary>
    /// Out of the box SharePoint folders.
    /// </summary>
    public static class BuiltInFolderDefinitions
    {
        #region properties

        /// <summary>
        /// Forms folder in the libraries and lists.
        /// </summary>
        public static FolderDefinition Forms = new FolderDefinition
        {
            Name = "Forms"
        };

        #endregion
    }
}
