using System;
using System.ComponentModel;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Utils;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Capabilities;

namespace SPMeta2.Standard.Definitions
{
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Publishing.Navigation.WebNavigationSettings", "Microsoft.SharePoint.Publishing")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Publishing.Navigation.WebNavigationSettings", "Microsoft.SharePoint.Client.Publishing")]

    [DefaultParentHost(typeof(WebDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [SingletonIdentity]

    [ParentHostCapability(typeof(WebDefinition))]
    public class WebNavigationSettingsDefinition : DefinitionBase
    {
        #region constructors

        public WebNavigationSettingsDefinition()
        {
            CurrentNavigationTermSetLCID = 1033;
            GlobalNavigationTermSetLCID = 1033;
        }


        #endregion

        #region properties

        [DataMember]
        public bool? DisplayShowHideRibbonAction { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? AddNewPagesToNavigation { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? CreateFriendlyUrlsForNewPages { get; set; }

        [DataMember]
        [ExpectValidation]
        [ExpectRequired(GroupName = "NavigationSource")]
        public string GlobalNavigationSource { get; set; }

        #region global navigation term store support

        [DataMember]
        public Guid? GlobalNavigationTermStoreId { get; set; }

        [DataMember]
        public string GlobalNavigationTermStoreName { get; set; }

        [DataMember]
        public bool? GlobalNavigationUseDefaultSiteCollectionTermStore { get; set; }

        [DataMember]
        public Guid? GlobalNavigationTermGroupId { get; set; }

        [DataMember]
        public string GlobalNavigationTermGroupName { get; set; }

        [DataMember]
        public Guid? GlobalNavigationTermSetId { get; set; }

        [DataMember]
        public int GlobalNavigationTermSetLCID { get; set; }


        [DataMember]
        public string GlobalNavigationTermSetName { get; set; }

        #endregion

        [DataMember]
        [ExpectValidation]
        [ExpectRequired(GroupName = "NavigationSource")]
        public string CurrentNavigationSource { get; set; }

        #region current navigation term store support

        [DataMember]
        public Guid? CurrentNavigationTermStoreId { get; set; }

        [DataMember]
        public string CurrentNavigationTermStoreName { get; set; }

        [DataMember]
        public bool? CurrentNavigationUseDefaultSiteCollectionTermStore { get; set; }

        [DataMember]
        public Guid? CurrentNavigationTermGroupId { get; set; }

        [DataMember]
        public string CurrentNavigationTermGroupName { get; set; }

        [DataMember]
        public Guid? CurrentNavigationTermSetId { get; set; }

        [DataMember]
        public int CurrentNavigationTermSetLCID { get; set; }

        [DataMember]
        public string CurrentNavigationTermSetName { get; set; }

        #endregion

        [DataMember]
        public bool? ResetToDefaults { get; set; }

        [DataMember]
        [ExpectValidation]
        [ExpectUpdate]
        public bool? GlobalNavigationShowPages { get; set; }

        [DataMember]
        [ExpectValidation]
        [ExpectUpdate]
        public bool? GlobalNavigationShowSubsites { get; set; }

        [DataMember]
        [ExpectValidation]
        [ExpectUpdateAsIntRange(MinValue = 5, MaxValue = 10)]
        public int? GlobalNavigationMaximumNumberOfDynamicItems { get; set; }

        [DataMember]
        [ExpectValidation]
        [ExpectUpdate]
        public bool? CurrentNavigationShowPages { get; set; }

        [DataMember]
        [ExpectValidation]
        [ExpectUpdate]
        public bool? CurrentNavigationShowSubsites { get; set; }

        [DataMember]
        [ExpectValidation]
        [ExpectUpdateAsIntRange(MinValue = 5, MaxValue = 10)]
        public int? CurrentNavigationMaximumNumberOfDynamicItems { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("GlobalNavigationSource", GlobalNavigationSource)
                          .AddRawPropertyValue("CurrentNavigationSource", CurrentNavigationSource)
                          .ToString();
        }

        #endregion
    }
}