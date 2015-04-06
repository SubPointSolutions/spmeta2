using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Standard.Definitions
{
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Publishing.Navigation.WebNavigationSettings", "Microsoft.SharePoint.Publishing")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Publishing.Navigation.WebNavigationSettings", "Microsoft.SharePoint.Client.Publishing")]

    [DefaultParentHost(typeof(WebDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    public class WebNavigationSettingsDefinition : DefinitionBase
    {
        #region constructors


        #endregion

        #region properties

        [DataMember]
        public string GlobalNavigationSource { get; set; }

        [DataMember]
        public Guid? GlobalNavigationTermStoreId { get; set; }

        [DataMember]
        public Guid? GlobalNavigationTermSetId { get; set; }

        [DataMember]
        public string CurrentNavigationSource { get; set; }
        [DataMember]
        public Guid? CurrentNavigationTermStoreId { get; set; }
        [DataMember]
        public Guid? CurrentNavigationTermSetId { get; set; }

        [DataMember]
        public bool? ResetToDefaults { get; set; }

        [DataMember]
        public bool? GlobalNavigationShowPages { get; set; }
        [DataMember]
        public bool? GlobalNavigationShowSubsites { get; set; }

        [DataMember]
        public int? GlobalNavigationMaximumNumberOfDynamicItems { get; set; }

        [DataMember]
        public bool? CurrentNavigationShowPages { get; set; }
        [DataMember]
        public bool? CurrentNavigationShowSubsites { get; set; }

        [DataMember]
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
