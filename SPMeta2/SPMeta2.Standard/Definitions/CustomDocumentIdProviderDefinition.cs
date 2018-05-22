using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Capabilities;

namespace SPMeta2.Standard.Definitions
{
    /// <summary>
    /// Allows to define and deploy custom document id provider.
    /// </summary>
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPSite", "Microsoft.SharePoint")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]

    [ParentHostCapability(typeof(SiteDefinition))]
    public class CustomDocumentIdProviderDefinition : DefinitionBase
    {
        #region properties

        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string DocumentProviderType { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("DocumentProviderType", DocumentProviderType)
                          .ToString();
        }

        #endregion
    }
}
