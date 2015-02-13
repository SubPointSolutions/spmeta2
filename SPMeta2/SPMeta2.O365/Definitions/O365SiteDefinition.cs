using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions.Base;

namespace SPMeta2.O365.Definitions
{
    /// <summary>
    /// Allows to define and deploy O365 site collection.
    /// </summary>
    public class O365SiteDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// Absolute URL of the target site collection.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Owner login of the target site collection.
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// Template of the target site collection.
        /// 
        /// SPMeta2.Enumerations.BuiltInWebTemplates class can be used to utilize out of the box SharePoint site templates.
        /// </summary>
        public string Template { get; set; }

        #endregion
    }
}
