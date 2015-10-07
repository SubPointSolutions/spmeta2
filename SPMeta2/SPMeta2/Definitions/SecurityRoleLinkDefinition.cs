using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to attach security rile to the target security group.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPRoleDefinition", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.RoleDefinition", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(SecurityGroupLinkDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(WebDefinition))]

    [ExpectManyInstances]

    public class SecurityRoleLinkDefinition : DefinitionBase
    {
        #region constructors

        public SecurityRoleLinkDefinition()
        {

        }

        #endregion

        #region properties

        /// <summary>
        /// Target security role name.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired(GroupName = "Role")]
        [DataMember]
        [IdentityKey]
        public string SecurityRoleName { get; set; }

        /// <summary>
        /// Type of the target role.
        /// 
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired(GroupName = "Role")]
        [DataMember]
        [IdentityKey]
        public string SecurityRoleType { get; set; }

        /// <summary>
        /// ID of the target security role.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired(GroupName = "Role")]
        [DataMember]
        [IdentityKey]
        public int SecurityRoleId { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("SecurityRoleName:[{0}]", SecurityRoleName);
        }

        #endregion
    }
}
