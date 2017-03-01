using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint security group.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPPrincipal", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Principal", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(SiteDefinition))]

    [ExpectManyInstances]

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
        [ExpectRequired(GroupName = "Name, IsAssociatedVisitorsGroup, IsAssociatedMemberGroup or IsAssociatedOwnerGroup")]
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
        [ExpectRequiredBoolRange(true)]
        [ExpectRequired(GroupName = "Name, IsAssociatedVisitorsGroup, IsAssociatedMemberGroup or IsAssociatedOwnerGroup")]
        public bool IsAssociatedVisitorsGroup { get; set; }

        /// <summary>
        /// Flag to mimic AssociatedMemberGroup
        /// </summary>
        /// 
        [DataMember]
        [ExpectRequiredBoolRange(true)]
        [ExpectRequired(GroupName = "Name, IsAssociatedVisitorsGroup, IsAssociatedMemberGroup or IsAssociatedOwnerGroup")]
        public bool IsAssociatedMemberGroup { get; set; }

        /// <summary>
        /// Flag to mimic AssociatedOwnerGroup
        /// </summary>

        [DataMember]
        [ExpectRequiredBoolRange(true)]
        [ExpectRequired(GroupName = "Name, IsAssociatedVisitorsGroup, IsAssociatedMemberGroup or IsAssociatedOwnerGroup")]
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
