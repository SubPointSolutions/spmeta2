using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Standard.Definitions.DisplayTemplates
{
    /// <summary>
    /// Allows to define and deploy control display template.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFile", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(ListDefinition), typeof(RootWebDefinition))]

    [Serializable]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]
    public class ControlDisplayTemplateDefinition : ItemControlTemplateDefinitionBase
    {
        #region constructors

        public ControlDisplayTemplateDefinition()
        {

        }

        #endregion

        #region properties

        [ExpectUpdateAsUrl(Extension = "xslt")]
        [ExpectValidation]
        public string CrawlerXSLFileURL { get; set; }

        [ExpectUpdate]
        [ExpectValidation]
        public string CrawlerXSLFileDescription { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<ControlDisplayTemplateDefinition>(this, base.ToString())

                          .ToString();
        }

        #endregion
    }
}
