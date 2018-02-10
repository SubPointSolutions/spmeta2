using System;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Office.Server.Search.WebControls;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers.Webparts
{
    public class ResultScriptWebPartModelHandler : WebPartModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ResultScriptWebPartDefinition); }
        }

        #endregion

        #region methods

        protected override void OnBeforeDeployModel(WebpartPageModelHost host, WebPartDefinition webpartModel)
        {
            var typedModel = webpartModel.WithAssertAndCast<ResultScriptWebPartDefinition>("webpartModel", value => value.RequireNotNull());
            typedModel.WebpartType = typeof(ResultScriptWebPart).AssemblyQualifiedName;
        }

        protected override void ProcessWebpartProperties(WebPart webpartInstance, WebPartDefinition webpartModel)
        {
            base.ProcessWebpartProperties(webpartInstance, webpartModel);

            var typedWebpart = webpartInstance.WithAssertAndCast<ResultScriptWebPart>("webpartInstance", value => value.RequireNotNull());
            var definition = webpartModel.WithAssertAndCast<ResultScriptWebPartDefinition>("webpartModel", value => value.RequireNotNull());

            if (!string.IsNullOrEmpty(definition.DataProviderJSON))
                typedWebpart.DataProviderJSON = definition.DataProviderJSON;

            if (!string.IsNullOrEmpty(definition.EmptyMessage))
                typedWebpart.EmptyMessage = definition.EmptyMessage;

            if (definition.ResultsPerPage.HasValue)
                typedWebpart.ResultsPerPage = definition.ResultsPerPage.Value;

            if (definition.ShowResultCount.HasValue)
                typedWebpart.ShowResultCount = definition.ShowResultCount.Value;

            if (definition.ShowLanguageOptions.HasValue)
                typedWebpart.ShowLanguageOptions = definition.ShowLanguageOptions.Value;

            if (definition.MaxPagesBeforeCurrent.HasValue)
                typedWebpart.MaxPagesBeforeCurrent = definition.MaxPagesBeforeCurrent.Value;

            if (definition.ShowBestBets.HasValue)
                typedWebpart.ShowBestBets = definition.ShowBestBets.Value;

            if (!string.IsNullOrEmpty(definition.AdvancedSearchPageAddress))
                typedWebpart.AdvancedSearchPageAddress = definition.AdvancedSearchPageAddress;

            if (definition.UseSharedDataProvider.HasValue)
                typedWebpart.UseSharedDataProvider = definition.UseSharedDataProvider.Value;

            if (definition.ShowPreferencesLink.HasValue)
                typedWebpart.ShowPreferencesLink = definition.ShowPreferencesLink.Value;

            if (definition.ShowViewDuplicates.HasValue)
                typedWebpart.ShowViewDuplicates = definition.ShowViewDuplicates.Value;

            if (definition.RepositionLanguageDropDown.HasValue)
                typedWebpart.RepositionLanguageDropDown = definition.RepositionLanguageDropDown.Value;

            if (!string.IsNullOrEmpty(definition.PreloadedItemTemplateIdsJson))
                typedWebpart.PreloadedItemTemplateIdsJson = definition.PreloadedItemTemplateIdsJson;

            if (definition.ShowPaging.HasValue)
                typedWebpart.ShowPaging = definition.ShowPaging.Value;

            if (!string.IsNullOrEmpty(definition.ResultTypeId))
                typedWebpart.ResultTypeId = definition.ResultTypeId;

            if (definition.ShowResults.HasValue)
                typedWebpart.ShowResults = definition.ShowResults.Value;

            if (!string.IsNullOrEmpty(definition.ItemTemplateId))
                typedWebpart.ItemTemplateId = definition.ItemTemplateId;

            if (!string.IsNullOrEmpty(definition.HitHighlightedPropertiesJson))
                typedWebpart.HitHighlightedPropertiesJson = definition.HitHighlightedPropertiesJson;

            if (!string.IsNullOrEmpty(definition.AvailableSortsJson))
                typedWebpart.AvailableSortsJson = definition.AvailableSortsJson;

            if (!string.IsNullOrEmpty(definition.RenderTemplateId))
                typedWebpart.RenderTemplateId = definition.RenderTemplateId;

            if (definition.ShowPersonalFavorites.HasValue)
                typedWebpart.ShowPersonalFavorites = definition.ShowPersonalFavorites.Value;

            if (definition.ShowSortOptions.HasValue)
                typedWebpart.ShowSortOptions = definition.ShowSortOptions.Value;

            if (definition.ShowAlertMe.HasValue)
                typedWebpart.ShowAlertMe = definition.ShowAlertMe.Value;

            if (definition.ShowDidYouMean.HasValue)
                typedWebpart.ShowDidYouMean = definition.ShowDidYouMean.Value;

            if (!string.IsNullOrEmpty(definition.QueryGroupName))
                typedWebpart.QueryGroupName = definition.QueryGroupName;

            if (definition.ShowAdvancedLink.HasValue)
                typedWebpart.ShowAdvancedLink = definition.ShowAdvancedLink.Value;

            if (definition.BypassResultTypes.HasValue)
                typedWebpart.BypassResultTypes = definition.BypassResultTypes.Value;

            if (!string.IsNullOrEmpty(definition.GroupTemplateId))
                typedWebpart.GroupTemplateId = definition.GroupTemplateId;
        }

        #endregion
    }
}
