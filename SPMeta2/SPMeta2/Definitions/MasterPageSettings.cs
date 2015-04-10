using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions.Base;
using System.Linq.Expressions;
using SPMeta2.Utils;
using System.Runtime.Serialization;

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
    [DataContract]
    public class MasterPageSettingsDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public string SiteMasterPageUrl { get; set; }

        [DataMember]
        public bool? SiteMasterPageInheritFromMaster { get; set; }

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public string SystemMasterPageUrl { get; set; }

        [DataMember]
        public bool? SystemMasterPageInheritFromMaster { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<MasterPageSettingsDefinition>(this)
                          .AddPropertyValue(p => p.SiteMasterPageUrl)
                          .AddPropertyValue(p => p.SiteMasterPageInheritFromMaster)
                          .AddPropertyValue(p => p.SystemMasterPageUrl)
                          .AddPropertyValue(p => p.SystemMasterPageInheritFromMaster)
                          .ToString();
        }

        #endregion
    }

}
