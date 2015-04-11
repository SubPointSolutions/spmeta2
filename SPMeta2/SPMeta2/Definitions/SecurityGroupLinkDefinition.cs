using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions.Base;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to add security group to the target SharePoint security container - web, list, folder, list items and so on.
    /// </summary>
    /// 

    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPRoleAssignment", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.RoleAssignment", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(BreakRoleInheritanceDefinition))]
    [ExpectWithExtensionMethod]

    [Serializable]
    [DataContract]

    public class SecurityGroupLinkDefinition : DefinitionBase
    {
        public SecurityGroupLinkDefinition()
        {

        }

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
