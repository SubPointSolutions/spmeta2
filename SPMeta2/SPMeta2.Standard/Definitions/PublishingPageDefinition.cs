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
    [DefaultParentHost(typeof(ListDefinition))]

    [ExpectAddHostExtensionMethod]
    [Serializable] 
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    public class PublishingPageDefinition : PageDefinitionBase
    {
        #region properties

        /// <summary>
        /// Page layout name of the target publishing page.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdateAsPageLayoutFileName]
        [ExpectRequired]
        [DataMember]
        public string PageLayoutFileName { get; set; }

        /// <summary>
        /// Description of the target publishing page.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        [ExpectNullable]
        public string Description { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string ContentTypeName { get; set; }

        /// <summary>
        /// Content of the target publishing page.
        /// </summary>
        //[ExpectUpdate]
        //[ExpectValidation]

        [DataMember]
        [ExpectNullable]
        public string Content { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<PublishingPageDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.PageLayoutFileName)
                          .AddPropertyValue(p => p.Description)
                          .ToString();
        }

        #endregion
    }
}
