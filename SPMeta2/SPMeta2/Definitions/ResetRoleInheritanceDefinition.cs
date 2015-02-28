using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy reset role inheritance on SharePoint securable object.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPSecurableObject", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.SecurableObject", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [Serializable]

    public class ResetRoleInheritanceDefinition : DefinitionBase
    {
        #region properties

        #endregion
    }
}
