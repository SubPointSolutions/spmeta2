using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy auditing setting for sites, lists and items.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPAudit", "Microsoft.SharePoint")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]

    [ParentHostCapability(typeof(SiteDefinition))]
    [ParentHostCapability(typeof(WebDefinition))]
    [ParentHostCapability(typeof(ListDefinition))]
    public class AuditSettingsDefinition : DefinitionBase
    {
        #region constructors

        public AuditSettingsDefinition()
        {
            AuditFlags = new Collection<string>();
        }

        #endregion

        #region properties

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public Collection<string> AuditFlags { get; set; }

        #endregion

        #region methods
        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("AuditFlags", AuditFlags)
                          .ToString();
        }

        #endregion
    }
}
