
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using SPMeta2.Definitions.Base;

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
    [ExpectWithExtensionMethod]
    public class ContentTypeLinkDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// ID of the target content type to be attached to the list.
        /// ContentTypeId is used for the first place, then ContentTypeName is used as a second attempt to lookup the content type.
        /// </summary>
        /// 

        [ExpectValidation]
        public string ContentTypeId { get; set; }

        /// <summary>
        /// Name of the target content type to be attached to the list.
        /// ContentTypeId is used for the first place, then ContentTypeName is used as a second attempt to lookup the content type.
        /// </summary>
        /// 

        [ExpectValidation]
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
