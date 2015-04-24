using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using System.Runtime.Serialization;

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

    public class PublishingPageLayoutDefinition : PageDefinitionBase
    {
        #region properties

        /// <summary>
        /// Description of the target publishing page.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public string Description { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        /// <summary>
        /// Content of the target publishing page layout.
        /// </summary>
        [ExpectRequired]
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

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<PublishingPageLayoutDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.Description)
                          .AddPropertyValue(p => p.AssociatedContentTypeId)
                          .ToString();
        }

        #endregion
    }
}
