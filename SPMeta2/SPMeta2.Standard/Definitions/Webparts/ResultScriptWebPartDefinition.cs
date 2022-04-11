using System;
using System.Runtime.Serialization;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Standard.Definitions.Webparts
{
    /// <summary>
    /// Allows to define and deploy 'Site Feed' web part.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "System.Web.UI.WebControls.WebParts.WebPart", "System.Web")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WebParts.WebPart", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebPartPageDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]

    [ExpectManyInstances]

    [ExpectWebpartType(WebPartType = "Microsoft.Office.Server.Search.WebControls.ResultScriptWebPart, Microsoft.Office.Server.Search, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c")]

    public class ResultScriptWebPartDefinition : WebPartDefinition
    {
        #region properties

        [DataMember]
        [ExpectValidation]
        public string DataProviderJSON { get; set; }

        [DataMember]
        [ExpectValidation]
        public string EmptyMessage { get; set; }

        [DataMember]
        [ExpectValidation]
        public int? ResultsPerPage { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? ShowResultCount { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? ShowLanguageOptions { get; set; }

        [DataMember]
        [ExpectValidation]
        public int? MaxPagesBeforeCurrent { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? ShowBestBets { get; set; }

        [DataMember]
        [ExpectValidation]
        public string AdvancedSearchPageAddress { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? UseSharedDataProvider { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? ShowPreferencesLink { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? ShowViewDuplicates { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? RepositionLanguageDropDown { get; set; }

        [DataMember]
        [ExpectValidation]
        public string PreloadedItemTemplateIdsJson { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? ShowPaging { get; set; }

        [DataMember]
        [ExpectValidation]
        public string ResultTypeId { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? ShowResults { get; set; }

        [DataMember]
        [ExpectValidation]
        public string ItemTemplateId { get; set; }

        [DataMember]
        [ExpectValidation]
        public string ItemBodyTemplateId { get; set; }

        [DataMember]
        [ExpectValidation]
        public string HitHighlightedPropertiesJson { get; set; }

        [DataMember]
        [ExpectValidation]
        public string AvailableSortsJson { get; set; }

        [DataMember]
        [ExpectValidation]
        public string RenderTemplateId { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? ShowPersonalFavorites { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? ShowSortOptions { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? ShowAlertMe { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? ShowDidYouMean { get; set; }

        [DataMember]
        [ExpectValidation]
        public string QueryGroupName { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? ShowAdvancedLink { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? BypassResultTypes { get; set; }

        [DataMember]
        [ExpectValidation]
        public string GroupTemplateId { get; set; }
        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())
                .ToString();
        }

        #endregion
    }
}
