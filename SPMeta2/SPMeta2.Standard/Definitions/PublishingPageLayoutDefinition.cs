using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;

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
    [ExpectWithExtensionMethod]
    public class PublishingPageLayoutDefinition : PageDefinitionBase
    {
        #region properties

        /// <summary>
        /// Description of the target publishing page.
        /// </summary>
        /// 
        [ExpectValidation]
        public string Description { get; set; }

        /// <summary>
        /// Content of the target publishing page layout.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Associated content type of the target publishing page layout.
        /// </summary>
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
