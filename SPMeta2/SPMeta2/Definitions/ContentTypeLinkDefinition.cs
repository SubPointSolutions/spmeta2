
using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to attach content type to the target list.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPContentType", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.ContentType", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ExpectManyInstances]

    [ParentHostCapability(typeof(ListDefinition))]
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
