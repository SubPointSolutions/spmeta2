using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPInformationRightsManagementSettings", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.InformationRightsManagementSettings", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [Serializable]

    public class InformationRightsManagementSettingsDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        public bool AllowPrint { get; set; }

        [ExpectValidation]
        public bool AllowScript { get; set; }

        [ExpectValidation]
        public bool AllowWriteCopy { get; set; }

        [ExpectValidation]
        public bool DisableDocumentBrowserView { get; set; }

        [ExpectValidation]
        public int DocumentAccessExpireDays { get; set; }

        [ExpectValidation]
        public DateTime DocumentLibraryProtectionExpireDate { get; set; }

        [ExpectValidation]
        public bool EnableDocumentAccessExpire { get; set; }

        [ExpectValidation]
        public bool EnableDocumentBrowserPublishingView { get; set; }

        [ExpectValidation]
        public bool EnableGroupProtection { get; set; }

        [ExpectValidation]
        public bool EnableLicenseCacheExpire { get; set; }

        [ExpectValidation]
        public string GroupName { get; set; }

        [ExpectValidation]
        public int LicenseCacheExpireDays { get; set; }

        [ExpectValidation]
        public string PolicyDescription { get; set; }

        [ExpectValidation]
        public string PolicyTitle { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<InformationRightsManagementSettingsDefinition>(this)
                          .AddPropertyValue(p => p.PolicyTitle)
                          .AddPropertyValue(p => p.PolicyDescription)
                          .AddPropertyValue(p => p.GroupName)
                          .AddPropertyValue(p => p.AllowPrint)
                          .AddPropertyValue(p => p.AllowScript)
                          .AddPropertyValue(p => p.AllowWriteCopy)
                          .ToString();
        }

        #endregion
    }
}
