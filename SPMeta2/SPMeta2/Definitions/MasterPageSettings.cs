using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to configure master page related setting on the target web site.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPWeb", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Web", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(WebDefinition))]

    [Serializable]
    [DataContract]

    [ParentHostCapability(typeof(WebDefinition))]
    public class MasterPageSettingsDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        [SiteCollectionTokenCapability]
        [WebTokenCapability]
        public string SiteMasterPageUrl { get; set; }

        [DataMember]
        public bool? SiteMasterPageInheritFromMaster { get; set; }

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        [SiteCollectionTokenCapability]
        [WebTokenCapability]
        public string SystemMasterPageUrl { get; set; }

        [DataMember]
        public bool? SystemMasterPageInheritFromMaster { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("SiteMasterPageUrl", SiteMasterPageUrl)
                          .AddRawPropertyValue("SiteMasterPageInheritFromMaster", SiteMasterPageInheritFromMaster)
                          .AddRawPropertyValue("SystemMasterPageUrl", SystemMasterPageUrl)
                          .AddRawPropertyValue("SystemMasterPageInheritFromMaster", SystemMasterPageInheritFromMaster)
                          .ToString();
        }

        #endregion
    }

}
