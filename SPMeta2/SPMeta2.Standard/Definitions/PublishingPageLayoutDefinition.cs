using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Capabilities;

namespace SPMeta2.Standard.Definitions
{

    /// <summary>
    /// Allows to define and deploy SharePoint publishing page.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFile", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(ListDefinition), typeof(RootWebDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(ListDefinition))]

    [ExpectManyInstances]

    public class PublishingPageLayoutDefinition : PageDefinitionBase
    {
        #region properties

        /// <summary>
        /// Title of the target page.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        // That is not required for PublishingPageLayoutDefinition
        // https://github.com/SubPointSolutions/spmeta2/issues/607
        //[ExpectRequired]
        [DataMember]
        public override string Title { get; set; }

        /// <summary>
        /// Description of the target publishing page.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Content of the target publishing page layout.
        /// </summary>
        [ExpectRequired]
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public string Content { get; set; }

        /// <summary>
        /// Associated content type of the target publishing page layout.
        /// </summary>
        [ExpectValidation]
        [ExpectUpdateAsPublishingPageContentType]
        //[ExpectRequired]
        [DataMember]
        [ExpectNullable]
        public string AssociatedContentTypeId { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        [ExpectUpdateAsUrl(Extension = ".png")]

        [SiteCollectionTokenCapability]
        [WebTokenCapability]
        public string PreviewImageUrl { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        [ExpectUpdate]
        public string PreviewImageDescription { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())
                          .AddRawPropertyValue("Description", Description)
                          .AddRawPropertyValue("AssociatedContentTypeId", AssociatedContentTypeId)
                          .ToString();
        }

        #endregion
    }
}
