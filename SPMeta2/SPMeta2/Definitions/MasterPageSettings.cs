using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to configure master page related setting on the target web site.
    /// </summary>
    /// 

    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPWeb", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Web", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(SiteDefinition))]
    [DefaultParentHostAttribute(typeof(WebDefinition))]

    [Serializable]
    public class MasterPageSettingsDefinition : DefinitionBase
    {
        #region properties

        public string SiteMasterPageUrl { get; set; }
        public bool? SiteMasterPageInheritFromMaster { get; set; }

        public string SystemMasterPageUrl { get; set; }
        public string SystemMasterPageInheritFromMaster { get; set; }

        #endregion
    }
}
