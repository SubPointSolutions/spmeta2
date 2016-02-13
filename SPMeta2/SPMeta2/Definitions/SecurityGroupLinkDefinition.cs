using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to add security group to the target SharePoint security container - web, list, folder, list items and so on.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPRoleAssignment", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.RoleAssignment", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(BreakRoleInheritanceDefinition))]
    [ExpectWithExtensionMethod]

    [Serializable]
    [DataContract]

    [ParentHostCapability(typeof(WebDefinition))]
    [ParentHostCapability(typeof(ListDefinition))]
    [ParentHostCapability(typeof(FolderDefinition))]
    [ParentHostCapability(typeof(ListItemDefinition))]

    [ExpectManyInstances]

    public class SecurityGroupLinkDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// Target security group name.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public string SecurityGroupName { get; set; }

        /// <summary>
        /// Flag to mimic out of the box AssociatedOwnerGroup
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public bool IsAssociatedVisitorGroup { get; set; }

        /// <summary>
        /// Flag to mimic AssociatedMemberGroup
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public bool IsAssociatedMemberGroup { get; set; }

        /// <summary>
        /// Flag to mimic AssociatedOwnerGroup
        /// </summary>
        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public bool IsAssociatedOwnerGroup { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("SecurityGroupName:[{0}]", SecurityGroupName);
        }

        #endregion
    }
}
