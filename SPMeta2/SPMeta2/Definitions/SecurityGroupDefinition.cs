using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using System;
using SPMeta2.Definitions.Base;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint security group.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPGroup", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Group", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(SiteDefinition))]
    [DefaultParentHostAttribute(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    public class SecurityGroupDefinition : DefinitionBase
    {
        #region  constructors

        public SecurityGroupDefinition()
        {

        }

        #endregion

        #region properties

        /// <summary>
        /// Name of the target security group.
        /// </summary>
        /// 

        [DataMember]
        [ExpectValidation]
        [ExpectRequired]
        [IdentityKey]
        public string Name { get; set; }

        /// <summary>
        /// Description of the target security group.
        /// </summary>
        /// 

        [DataMember]
        [ExpectValidation]
        [ExpectUpdate]
        [ExpectNullable]
        public string Description { get; set; }

        /// <summary>
        /// Login name of the owner for the target security group.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdateAsUser]
        [DataMember]
        public string Owner { get; set; }

        /// <summary>
        /// Default user login for the target security group.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string DefaultUser { get; set; }

        /// <summary>
        /// Membership view options.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public bool OnlyAllowMembersViewMembership { get; set; }

        /// <summary>
        /// Flag to mimic out of the box AssociatedOwnerGroup
        /// </summary>
        /// 
        [DataMember]
        public bool IsAssociatedVisitorsGroup { get; set; }

        /// <summary>
        /// Flag to mimic AssociatedMemberGroup
        /// </summary>
        /// 
        [DataMember]
        public bool IsAssociatedMemberGroup { get; set; }

        /// <summary>
        /// Flag to mimic AssociatedOwnerGroup
        /// </summary>

        [DataMember]
        public bool IsAssociatedOwnerGroup { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? AllowMembersEditMembership { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? AllowRequestToJoinLeave { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? AutoAcceptRequestToJoinLeave { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Name:[{0}] Description:[{1}]", Name, Description);
        }

        #endregion
    }
}
