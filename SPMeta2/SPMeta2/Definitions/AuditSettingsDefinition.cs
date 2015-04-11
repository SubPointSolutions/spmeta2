using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy auditing setting for sites, lists and items.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPAudit", "Microsoft.SharePoint")]

    [DefaultRootHostAttribute(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(SiteDefinition))]

    [Serializable] 
    [DataContract]

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
            return new ToStringResult<AuditSettingsDefinition>(this)
                          .AddPropertyValue(p => p.AuditFlags)
                          .ToString();
        }

        #endregion
    }
}
