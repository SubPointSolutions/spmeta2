using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint web part page.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFile", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(ListDefinition))]

    [Serializable]
    public class WebPartPageDefinition : PageDefinitionBase
    {
        #region constructors

        public WebPartPageDefinition()
        {
            NeedOverride = true;
        }

        #endregion

        #region properties

        /// <summary>
        /// Out of the box web part page layout id.
        /// 
        /// BuiltInWebpartPageTemplateId class can be used to utilize out of the box SharePoint web part page templates.
        /// </summary>
        /// 
        [ExpectValidation]

        public int PageLayoutTemplate { get; set; }

        /// <summary>
        /// Custom web part page layout content.
        /// </summary>
        /// 
        [ExpectValidation]

        public string CustomPageLayout { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("{0} PageLayoutTemplate:[{1}]", base.ToString(), PageLayoutTemplate);
        }

        #endregion
    }
}
