using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Standard.Definitions
{
    /// <summary>
    /// Allows to define and deploy page layout and site template setting for the target web site.
    /// </summary>
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPWeb", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Web", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(WebDefinition))]
   

    [Serializable]
    public class PageLayoutAndSiteTemplateSettingsDefinition : DefinitionBase
    {
        #region constructors

        public PageLayoutAndSiteTemplateSettingsDefinition()
        {
            DefinedWebTemplates = new Collection<string>();
            DefinedPageLayouts = new Collection<string>();
        }

        #endregion

        #region properties

        #region web templates

        [ExpectValidation]
        public bool? InheritWebTemplates { get; set; }

        [ExpectValidation]
        public bool? UseAnyWebTemplate { get; set; }

        [ExpectValidation]
        public bool? UseDefinedWebTemplates { get; set; }

        [ExpectValidation]
        public Collection<string> DefinedWebTemplates { get; set; }

        [ExpectValidation]
        public bool? ResetAllSubsitesToInheritWebTemplates { get; set; }

        #endregion

        #region page layouts

        [ExpectValidation]
        public bool? InheritPageLayouts { get; set; }

        [ExpectValidation]
        public bool? UseAnyPageLayout { get; set; }

        [ExpectValidation]
        public bool? UseDefinedPageLayouts { get; set; }

        [ExpectValidation]
        public Collection<string> DefinedPageLayouts { get; set; }

        [ExpectValidation]
        public bool? ResetAllSubsitesToInheritPageLayouts { get; set; }

        #endregion

        #region page default settings

        [ExpectValidation]
        public bool? InheritDefaultPageLayout { get; set; }

        [ExpectValidation]
        public bool? UseDefinedDefaultPageLayout { get; set; }

        public string DefinedDefaultPageLayout { get; set; }

        [ExpectValidation]
        public bool? ResetAllSubsitesToInheritDefaultPageLayout { get; set; }

        #endregion

        [ExpectValidation]
        public bool? ConverBlankSpacesIntoHyphen { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<PageLayoutAndSiteTemplateSettingsDefinition>(this)
                //.AddPropertyValue(p => p.AudienceName)
                //.AddPropertyValue(p => p.AudienceDescription)
                          .ToString();
        }

        #endregion
    }
}
