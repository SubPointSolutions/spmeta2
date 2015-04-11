using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPInformationRightsManagementSettings", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.InformationRightsManagementSettings", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [Serializable] [DataContract]

    public class InformationRightsManagementSettingsDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        [DataMember]
        public bool AllowPrint { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool AllowScript { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool AllowWriteCopy { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool DisableDocumentBrowserView { get; set; }

        [ExpectValidation]
        [DataMember]
        public int DocumentAccessExpireDays { get; set; }

        [ExpectValidation]
        [DataMember]
        public DateTime DocumentLibraryProtectionExpireDate { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool EnableDocumentAccessExpire { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool EnableDocumentBrowserPublishingView { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool EnableGroupProtection { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool EnableLicenseCacheExpire { get; set; }

        [ExpectValidation]
        [DataMember]
        public string GroupName { get; set; }

        [ExpectValidation]
        [DataMember]
        public int LicenseCacheExpireDays { get; set; }

        [ExpectValidation]
        [DataMember]
        public string PolicyDescription { get; set; }

        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKeyAttribute]
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
