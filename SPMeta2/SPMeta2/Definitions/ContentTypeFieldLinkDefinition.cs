using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to attach field to the target content type.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldLink", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldLink", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(ContentTypeDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ExpectManyInstances]

    [ParentHostCapability(typeof(ContentTypeDefinition))]
    public class ContentTypeFieldLinkDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// ID of the target field to be attached to content type.
        /// BuiltInFieldId class can be used to utilize out of the box SharePoint fields. 
        /// </summary>
        /// 

        [ExpectValidation]
        [ExpectRequired(GroupName = "FieldIdOrName")]
        [DataMember]
        [IdentityKey]
        public Guid? FieldId { get; set; }

        [ExpectValidation]
        [ExpectRequired(GroupName = "FieldIdOrName")]
        [DataMember]
        [IdentityKey]
        public string FieldInternalName { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? Required { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? Hidden { get; set; }

        /// <summary>
        /// Is not supported in CSOM yet!
        /// https://officespdev.uservoice.com/forums/224641-general/suggestions/7024931-enhance-fieldlink-class-with-additional-properties
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public string DisplayName { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("FieldId:[{0}]", FieldId);
        }

        #endregion
    }
}
