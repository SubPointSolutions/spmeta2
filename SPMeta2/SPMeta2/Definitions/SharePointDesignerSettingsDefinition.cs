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
    //[ExpectManyInstances]

    [ParentHostCapability(typeof(WebDefinition))]
    public class SharePointDesignerSettingsDefinition : DefinitionBase
    {
        #region constructors

        public SharePointDesignerSettingsDefinition()
        {

        }

        #endregion

        #region properties

        /// <summary>
        /// Maps out AllowDesigner property bag value at the root web.
        /// </summary>
        [DataMember]
        [ExpectValidation]
        [ExpectUpdate]
        public bool? EnableSharePointDesigner { get; set; }

        /// <summary>
        /// Maps out AllowRevertFromTemplate property bag value at the root web.
        /// </summary>
        [DataMember]
        [ExpectValidation]
        [ExpectUpdate]
        public bool? EnableDetachingPages { get; set; }

        /// <summary>
        /// Maps out AllowMasterpageEditing property bag value at the root web.
        /// </summary>
        [DataMember]
        [ExpectValidation]
        [ExpectUpdate]
        public bool? EnableCustomizingMasterPagesAndPageLayouts { get; set; }

        /// <summary>
        /// Maps out ShowUrlStructure property bag value at the root web.
        /// </summary>
        [DataMember]
        [ExpectValidation]
        [ExpectUpdate]
        public bool? EnableManagingWebSiteUrlStructure { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("EnableSharePointDesigner", EnableSharePointDesigner)
                          .AddRawPropertyValue("EnableDetachingPages", EnableDetachingPages)
                          .AddRawPropertyValue("EnableCustomizingMasterPagesAndPageLayouts", EnableCustomizingMasterPagesAndPageLayouts)
                          .AddRawPropertyValue("EnableManagingWebSiteUrlStructure", EnableManagingWebSiteUrlStructure)
                          .ToString();
        }

        #endregion
    }
}
