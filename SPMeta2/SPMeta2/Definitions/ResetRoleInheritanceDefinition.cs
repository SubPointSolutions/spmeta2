using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy reset role inheritance on SharePoint securable object.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPSecurableObject", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.SecurableObject", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [Serializable]
    [DataContract]
    [SingletonIdentity]

    [ParentHostCapability(typeof(WebDefinition))]

    [SelfHostCapability]
    public class ResetRoleInheritanceDefinition : DefinitionBase
    {
        #region properties

        #endregion

        #region methods

        public override string ToString()
        {
            // we need that to pass SPMeta2 API tests 'DefinitionsShouldHaveToStringOverride'
            return new ToStringResultRaw()
                         .ToString();
        }

        #endregion
    }
}
