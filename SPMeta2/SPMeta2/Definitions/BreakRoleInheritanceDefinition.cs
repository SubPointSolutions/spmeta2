using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy break role inheritance on SharePoint securable object.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPSecurableObject", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.SecurableObject", "Microsoft.SharePoint.Client")]

    [RootHostAttribute(typeof(WebDefinition))]
    [ParentHostAttribute(typeof(ListDefinition))]
    public class BreakRoleInheritanceDefinition : DefinitionBase
    {
        #region properties

        public bool CopyRoleAssignments { get; set; }
        public bool ClearSubscopes { get; set; }

        public bool ForceClearSubscopes { get; set; }

        #endregion
    }
}
