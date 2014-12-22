using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;

namespace SPMeta2.Standard.Definitions
{

    /// <summary>
    /// Allows to define and deploy SharePoint Search Result
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.Office.Server.Search.Administration.Query.Source", "Microsoft.Office.Server.Search")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Search.Portability.SearchConfigurationPortability", "Microsoft.SharePoint.Client.Search")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(SiteDefinition))]

    [Serializable]

    public class SearchResultDefinition : DefinitionBase
    {
        #region constructors

        public SearchResultDefinition()
        {
            Description = string.Empty;
        }

        #endregion

        #region properties

        [ExpectValidation]
        public bool IsDefault { get; set; }

        [ExpectValidation]
        public string Name { get; set; }

        [ExpectValidation]
        public string Description { get; set; }

        [ExpectValidation]
        public string Query { get; set; }

        [ExpectValidation]
        public string ProviderName { get; set; }

        [ExpectValidation]
        public Guid? ProviderId { get; set; }

        #endregion
    }
}
