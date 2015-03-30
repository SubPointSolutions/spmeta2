using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Standard.Definitions.DisplayTemplates
{
    /// <summary>
    /// Allows to define and deploy item display template.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFile", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(ListDefinition), typeof(RootWebDefinition))]

    [Serializable]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]
    public class JavaScriptDisplayTemplateDefinition : TemplateDefinitionBase
    {
        #region constructors

        public JavaScriptDisplayTemplateDefinition()
        {
        }

        #endregion

        #region properties

        [ExpectUpdate]
        [ExpectValidation]
        public string TargetControlType { get; set; }

        [ExpectUpdateAsUrl(Extension = "png")]
        [ExpectValidation]
        public string IconUrl { get; set; }

        [ExpectUpdate]
        [ExpectValidation]
        public string IconDescription { get; set; }

        [ExpectUpdate]
        [ExpectValidation]
        public string Standalone { get; set; }

        [ExpectUpdate]
        [ExpectValidation]
        public string TargetScope { get; set; }

        [ExpectUpdate]
        [ExpectValidation]
        public string TargetListTemplateId { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<JavaScriptDisplayTemplateDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.TargetControlType)
                          .AddPropertyValue(p => p.Standalone)
                          .AddPropertyValue(p => p.TargetScope)
                          .AddPropertyValue(p => p.TargetListTemplateId)
                          .ToString();
        }

        #endregion
    }
}
