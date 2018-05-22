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
    /// Allows to define and deploy tree view navigation setting for the target web site.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPWeb", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Web", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(WebDefinition))]
    [Serializable]
    [DataContract]
    [SingletonIdentity]

    [ParentHostCapability(typeof(WebDefinition))]
    public class TreeViewSettingsDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool? QuickLaunchEnabled { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool? TreeViewEnabled { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("QuickLaunchEnabled", QuickLaunchEnabled)
                          .AddRawPropertyValue("TreeViewEnabled", TreeViewEnabled)

                          .ToString();
        }

        #endregion
    }
}
