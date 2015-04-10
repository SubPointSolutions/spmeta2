
using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using System;
using SPMeta2.Definitions.Base;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to attach content type to the target list.
    /// </summary>
    /// 

    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPContentType", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.ContentType", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(ListDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    public class ContentTypeLinkDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// ID of the target content type to be attached to the list.
        /// ContentTypeId is used for the first place, then ContentTypeName is used as a second attempt to lookup the content type.
        /// </summary>
        /// 
        [ExpectRequired(GroupName = "ContentType Link")]
        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public string ContentTypeId { get; set; }

        /// <summary>
        /// Name of the target content type to be attached to the list.
        /// ContentTypeId is used for the first place, then ContentTypeName is used as a second attempt to lookup the content type.
        /// </summary>
        /// 

        [ExpectRequired(GroupName = "ContentType Link")]
        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public string ContentTypeName { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("ContentTypeName:[{0}] ContentTypeId:[{1}]", ContentTypeName, ContentTypeId);
        }

        #endregion
    }
}
