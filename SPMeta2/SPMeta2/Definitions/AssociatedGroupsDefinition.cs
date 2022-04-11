using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Attaches associated groups to SharePoint web 
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPWeb", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Web", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(WebDefinition))]

    [Serializable]
    [DataContract]

    //[ExpectWithExtensionMethod]
    //[ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(WebDefinition))]

        public class AssociatedGroupsDefinition : DefinitionBase
    {
        #region  constructors

        public AssociatedGroupsDefinition()
        {

        }

        #endregion

        #region properties

        /// <summary>
        /// Name of the security group to be set as AssociatedMemberGroup, group must exist 
        /// </summary>
        [DataMember]
        [ExpectValidation]
        [ExpectRequired(GroupName = "MemberGroupName, OwnerGroupName, VisitorGroupName")]
        [IdentityKey]
        public string MemberGroupName { get; set; }

        /// <summary>
        /// Name of the security group to be set as AssociatedOwnerGroup, group must exist 
        /// </summary>
        [DataMember]
        [ExpectValidation]
        [ExpectRequired(GroupName = "MemberGroupName, OwnerGroupName, VisitorGroupName")]
        [IdentityKey]
        public string OwnerGroupName { get; set; }

        /// <summary>
        /// Name of the security group to be set as AssociatedOwnerGroup, group must exist 
        /// </summary>
        [DataMember]
        [ExpectValidation]
        [ExpectRequired(GroupName = "MemberGroupName, OwnerGroupName, VisitorGroupName")]
        [IdentityKey]
        public string VisitorGroupName { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("MemberGroupName:[{0}] OwnerGroupName:[{1}] VisitorGroupName:[{1}]",
                MemberGroupName, OwnerGroupName, VisitorGroupName);
        }

        #endregion
    }
}
