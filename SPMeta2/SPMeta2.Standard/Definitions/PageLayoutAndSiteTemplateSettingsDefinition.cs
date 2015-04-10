using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using System.Runtime.Serialization;

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
    [DataContract]
    [SingletonIdentity]
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
        [DataMember]
        public bool? InheritWebTemplates { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? UseAnyWebTemplate { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? UseDefinedWebTemplates { get; set; }

        [ExpectValidation]
        [DataMember]
        public Collection<string> DefinedWebTemplates { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? ResetAllSubsitesToInheritWebTemplates { get; set; }

        #endregion

        #region page layouts

        [ExpectValidation]
        [DataMember]
        public bool? InheritPageLayouts { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? UseAnyPageLayout { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? UseDefinedPageLayouts { get; set; }

        [ExpectValidation]
        [DataMember]
        public Collection<string> DefinedPageLayouts { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? ResetAllSubsitesToInheritPageLayouts { get; set; }

        #endregion

        #region page default settings

        [ExpectValidation]
        [DataMember]
        public bool? InheritDefaultPageLayout { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? UseDefinedDefaultPageLayout { get; set; }

        [DataMember]
        public string DefinedDefaultPageLayout { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? ResetAllSubsitesToInheritDefaultPageLayout { get; set; }

        #endregion

        [ExpectValidation]
        [DataMember]
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
