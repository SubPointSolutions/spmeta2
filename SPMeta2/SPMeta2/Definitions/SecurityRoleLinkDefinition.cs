using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using System;
using SPMeta2.Definitions.Base;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to attach security rile to the target security group.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPRoleDefinition", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.RoleDefinition", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(SecurityGroupLinkDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
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
