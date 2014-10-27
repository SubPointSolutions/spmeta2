using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy break role inheritance on SharePoint securable object.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPSecurableObject", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.SecurableObject", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(ListDefinition))]
    public class BreakRoleInheritanceDefinition : DefinitionBase
    {
        #region properties

        //[ExpectValidation]
        public bool CopyRoleAssignments { get; set; }

        [ExpectValidation]
        public bool ClearSubscopes { get; set; }

        [ExpectValidation]
        public bool ForceClearSubscopes { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<BreakRoleInheritanceDefinition>(this)
                          .AddPropertyValue(p => p.CopyRoleAssignments)
                          .AddPropertyValue(p => p.ClearSubscopes)
                          .AddPropertyValue(p => p.ForceClearSubscopes)
                          .ToString();
        }

        #endregion
    }
}
