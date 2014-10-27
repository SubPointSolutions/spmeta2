using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Utils;

namespace SPMeta2.Standard.Definitions
{
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Publishing.Navigation.WebNavigationSettings", "Microsoft.SharePoint.Publishing")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Publishing.Navigation.WebNavigationSettings", "Microsoft.SharePoint.Client.Publishing")]

    [DefaultParentHost(typeof(WebDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    public class WebNavigationSettingsDefinition : DefinitionBase
    {
        #region constructors


        #endregion

        #region properties

        public string GlobalNavigationSource { get; set; }
        public Guid? GlobalNavigationTermStoreId { get; set; }
        public Guid? GlobalNavigationTermSetId { get; set; }

        public string CurrentNavigationSource { get; set; }
        public Guid? CurrentNavigationTermStoreId { get; set; }
        public Guid? CurrentNavigationTermSetId { get; set; }

        public bool? ResetToDefaults { get; set; }

        public bool? GlobalNavigationShowPages { get; set; }
        public bool? GlobalNavigationShowSubsites { get; set; }

        public int? GlobalNavigationMaximumNumberOfDynamicItems { get; set; }

        public bool? CurrentNavigationShowPages { get; set; }
        public bool? CurrentNavigationShowSubsites { get; set; }

        public int? CurrentNavigationMaximumNumberOfDynamicItems { get; set; }


        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<WebNavigationSettingsDefinition>(this)
                          .AddPropertyValue(p => p.GlobalNavigationSource)
                          .AddPropertyValue(p => p.CurrentNavigationSource)
                          .ToString();
        }

        #endregion
    }
}
