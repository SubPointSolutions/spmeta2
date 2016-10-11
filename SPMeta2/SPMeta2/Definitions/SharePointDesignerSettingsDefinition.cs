using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{

    /// <summary>
    /// Allows to provision SharePointDesigner settings on the target site.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPSite", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Site", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]

    //[ExpectWithExtensionMethod]
    //[ExpectArrayExtensionMethod]

    [SingletonIdentity]
    [ExpectManyInstances]

    [ParentHostCapability(typeof(WebDefinition))]
    public class SharePointDesignerSettingsDefinition : DefinitionBase
    {
        #region constructors

        public SharePointDesignerSettingsDefinition()
        {

        }

        #endregion

        #region properties

        [DataMember]
        [ExpectValidation]
        [ExpectUpdate]
        public bool? EnableSharePointDesigner { get; set; }

        [DataMember]
        [ExpectValidation]
        [ExpectUpdate]
        public bool? EnableDetachingPages { get; set; }

        [DataMember]
        [ExpectValidation]
        [ExpectUpdate]
        public bool? EnableCustomizingMasterPagesAndPageLayouts { get; set; }

        [DataMember]
        [ExpectValidation]
        [ExpectUpdate]
        public bool? EnableManagingWebSiteUrlStructure { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<SharePointDesignerSettingsDefinition>(this)
                          .AddPropertyValue(p => p.EnableSharePointDesigner)
                          .AddPropertyValue(p => p.EnableDetachingPages)
                          .AddPropertyValue(p => p.EnableCustomizingMasterPagesAndPageLayouts)
                          .AddPropertyValue(p => p.EnableManagingWebSiteUrlStructure)
                          .ToString();
        }

        #endregion
    }
}
