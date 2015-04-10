using System;
using System.Collections.Generic;
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
    /// Allows to define and deploy alternate URL.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPAlternateUrl", "Microsoft.SharePoint")]

    [DefaultRootHost(typeof(WebApplicationDefinition))]
    [DefaultParentHost(typeof(WebApplicationDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]
    public class AlternateUrlDefinition : DefinitionBase
    {
        #region properties

        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string Url { get; set; }

        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string UrlZone { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<AlternateUrlDefinition>(this)
                          .AddPropertyValue(p => p.Url)
                          .AddPropertyValue(p => p.UrlZone)

                          .ToString();
        }

        #endregion
    }
}
