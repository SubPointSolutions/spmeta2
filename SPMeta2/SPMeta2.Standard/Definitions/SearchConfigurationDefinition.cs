using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Standard.Definitions
{
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.Office.Server.Search.Portability.SearchConfigurationPortability", "Microsoft.Office.Server.Search")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Search.Portability.SearchConfigurationPortability", "Microsoft.SharePoint.Client.Search")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(SiteDefinition))]

    [Serializable]

    public class SearchConfigurationDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        public string SearchConfiguration { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<SearchConfigurationDefinition>(this)
                          .AddPropertyValue(p => p.SearchConfiguration)
                          .ToString();
        }

        #endregion
    }
}
