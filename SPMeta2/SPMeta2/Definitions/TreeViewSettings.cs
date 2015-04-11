using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy tree view navigation setting for the target web site.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPWeb", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Web", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(SiteDefinition))]
    [DefaultParentHostAttribute(typeof(WebDefinition))]
    [Serializable]
    [DataContract]
    [SingletonIdentity]
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
            return new ToStringResult<TreeViewSettingsDefinition>(this)
                          .AddPropertyValue(p => p.QuickLaunchEnabled)
                          .AddPropertyValue(p => p.TreeViewEnabled)

                          .ToString();
        }

        #endregion
    }
}
