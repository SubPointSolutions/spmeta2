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
    /// Allows to define and deploy break role inheritance on SharePoint securable object.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPSecurableObject", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.SecurableObject", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [Serializable]
    [DataContract]

    [ParentHostCapability(typeof(WebDefinition))]
    [ParentHostCapability(typeof(ListDefinition))]
    [ParentHostCapability(typeof(FolderDefinition))]
    [ParentHostCapability(typeof(ListItemDefinition))]

    [SelfHostCapability]

    public class BreakRoleInheritanceDefinition : DefinitionBase
    {
        #region properties

        //[ExpectValidation]
        [DataMember]
        [IdentityKey]
        public bool CopyRoleAssignments { get; set; }

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public bool ClearSubscopes { get; set; }

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public bool ForceClearSubscopes { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("CopyRoleAssignments", CopyRoleAssignments)
                          .AddRawPropertyValue("ClearSubscopes", ClearSubscopes)
                          .AddRawPropertyValue("ForceClearSubscopes", ForceClearSubscopes)
                          .ToString();
        }

        #endregion
    }
}
