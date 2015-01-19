using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy auditing setting for sites, lists and items.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPAudit", "Microsoft.SharePoint")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [Serializable]

    public class AuditSettingsDefinition : DefinitionBase
    {
        #region constructors

        public AuditSettingsDefinition()
        {

        }

        #endregion

        #region properties

        public Collection<string> AuditFlags { get; set; }

        #endregion
    }
}
