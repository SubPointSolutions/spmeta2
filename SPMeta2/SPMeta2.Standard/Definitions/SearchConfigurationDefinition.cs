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
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.Office.Server.Search.Portability.SearchConfigurationPortability", "Microsoft.Office.Server.Search")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Search.Portability.SearchConfigurationPortability", "Microsoft.SharePoint.Client.Search")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [SingletonIdentity]

    [ParentHostCapability(typeof(SiteDefinition))]
    public class SearchConfigurationDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public string SearchConfiguration { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("SearchConfiguration", SearchConfiguration)
                          .ToString();
        }

        #endregion
    }
}
