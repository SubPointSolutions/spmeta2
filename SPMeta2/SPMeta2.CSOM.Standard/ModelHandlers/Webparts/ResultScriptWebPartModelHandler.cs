using System;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers.Webparts
{
    public class ResultScriptWebPartModelHandler : WebPartModelHandler
    {
        public ResultScriptWebPartModelHandler()
        {
            ShouldUseWebPartStoreKeyForWikiPage = true;
        }

        #region properties

        public override Type TargetType
        {
            get { return typeof(ResultScriptWebPartDefinition); }
        }

        #endregion

        #region methods

        protected override string GetWebpartXmlDefinition(ListItemModelHost listItemModelHost, WebPartDefinitionBase webPartModel)
        {
            var definition = webPartModel.WithAssertAndCast<ResultScriptWebPartDefinition>("model", value => value.RequireNotNull());
            var xml = WebpartXmlExtensions.LoadWebpartXmlDocument(ProcessCommonWebpartProperties(BuiltInWebPartTemplates.ResultScriptWebPart, webPartModel));
            
            if (!string.IsNullOrEmpty(definition.DataProviderJSON))
                xml.SetOrUpdateProperty("DataProviderJSON", definition.DataProviderJSON);

            if (!string.IsNullOrEmpty(definition.EmptyMessage))
                xml.SetOrUpdateProperty("EmptyMessage", definition.EmptyMessage);

            if (definition.ResultsPerPage.HasValue)
                xml.SetOrUpdateProperty("ResultsPerPage", definition.ResultsPerPage.Value.ToString());

            if (definition.ShowResultCount.HasValue)
                xml.SetOrUpdateProperty("ShowResultCount", definition.ShowResultCount.Value.ToString());

            if (definition.MaxPagesBeforeCurrent.HasValue)
                xml.SetOrUpdateProperty("MaxPagesBeforeCurrent", definition.MaxPagesBeforeCurrent.Value.ToString());

            if (definition.ShowBestBets.HasValue)
                xml.SetOrUpdateProperty("ShowBestBets", definition.ShowBestBets.Value.ToString());

            if (definition.ShowViewDuplicates.HasValue)
                xml.SetOrUpdateProperty("ShowViewDuplicates", definition.ShowViewDuplicates.Value.ToString());

            if (definition.Height.HasValue)
                xml.SetOrUpdateProperty("Height", definition.Height.Value.ToString());

            if (!string.IsNullOrEmpty(definition.AdvancedSearchPageAddress))
                xml.SetOrUpdateProperty("AdvancedSearchPageAddress", definition.AdvancedSearchPageAddress);

            if (definition.UseSharedDataProvider.HasValue)
                xml.SetOrUpdateProperty("UseSharedDataProvider", definition.UseSharedDataProvider.Value.ToString());

            if (definition.ShowPreferencesLink.HasValue)
                xml.SetOrUpdateProperty("ShowPreferencesLink", definition.ShowPreferencesLink.Value.ToString());

            if (definition.RepositionLanguageDropDown.HasValue)
                xml.SetOrUpdateProperty("RepositionLanguageDropDown", definition.RepositionLanguageDropDown.Value.ToString());

            if (!string.IsNullOrEmpty(definition.PreloadedItemTemplateIdsJson))
                xml.SetOrUpdateProperty("PreloadedItemTemplateIdsJson", definition.PreloadedItemTemplateIdsJson);

            if (definition.ShowPaging.HasValue)
                xml.SetOrUpdateProperty("ShowPaging", definition.ShowPaging.Value.ToString());

            if (!string.IsNullOrEmpty(definition.ResultTypeId))
                xml.SetOrUpdateProperty("ResultTypeId", definition.ResultTypeId);

            if (!string.IsNullOrEmpty(definition.Title))
                xml.SetOrUpdateProperty("Title", definition.Title);

            if (definition.ShowResults.HasValue)
                xml.SetOrUpdateProperty("ShowResults", definition.ShowResults.Value.ToString());

            if (definition.Hidden.HasValue)
                xml.SetOrUpdateProperty("Hidden", definition.Hidden.Value.ToString());

            if (!string.IsNullOrEmpty(definition.ItemTemplateId))
                xml.SetOrUpdateProperty("ItemTemplateId", definition.ItemTemplateId);

            if (!string.IsNullOrEmpty(definition.ItemBodyTemplateId))
                xml.SetOrUpdateProperty("ItemBodyTemplateId", definition.ItemBodyTemplateId);

            if (!string.IsNullOrEmpty(definition.HitHighlightedPropertiesJson))
                xml.SetOrUpdateProperty("HitHighlightedPropertiesJson", definition.HitHighlightedPropertiesJson);

            if (!string.IsNullOrEmpty(definition.AvailableSortsJson))
                xml.SetOrUpdateProperty("AvailableSortsJson", definition.AvailableSortsJson);

            if (!string.IsNullOrEmpty(definition.RenderTemplateId))
                xml.SetOrUpdateProperty("RenderTemplateId", definition.RenderTemplateId);

            if (definition.ShowPersonalFavorites.HasValue)
                xml.SetOrUpdateProperty("ShowPersonalFavorites", definition.ShowPersonalFavorites.Value.ToString());

            if (definition.ShowSortOptions.HasValue)
                xml.SetOrUpdateProperty("ShowSortOptions", definition.ShowSortOptions.Value.ToString());

            if (definition.ShowLanguageOptions.HasValue)
                xml.SetOrUpdateProperty("ShowLanguageOptions", definition.ShowLanguageOptions.Value.ToString());

            if (!string.IsNullOrEmpty(definition.Description))
                xml.SetOrUpdateProperty("Description", definition.Description);

            if (!string.IsNullOrEmpty(definition.TitleUrl))
                xml.SetOrUpdateProperty("TitleUrl", definition.TitleUrl);

            if (definition.ShowAlertMe.HasValue)
                xml.SetOrUpdateProperty("ShowAlertMe", definition.ShowAlertMe.Value.ToString());

            if (definition.ShowDidYouMean.HasValue)
                xml.SetOrUpdateProperty("ShowDidYouMean", definition.ShowDidYouMean.Value.ToString());

            if (!string.IsNullOrEmpty(definition.QueryGroupName))
                xml.SetOrUpdateProperty("QueryGroupName", definition.QueryGroupName);

            if (definition.Width.HasValue)
                xml.SetOrUpdateProperty("Width", definition.Width.Value.ToString());

            if (definition.ShowAdvancedLink.HasValue)
                xml.SetOrUpdateProperty("ShowAdvancedLink", definition.ShowAdvancedLink.Value.ToString());

            if (definition.BypassResultTypes.HasValue)
                xml.SetOrUpdateProperty("BypassResultTypes", definition.BypassResultTypes.Value.ToString());

            if (!string.IsNullOrEmpty(definition.GroupTemplateId))
                xml.SetOrUpdateProperty("GroupTemplateId", definition.GroupTemplateId);

            if (!string.IsNullOrEmpty(definition.TitleIconImageUrl))
                xml.SetOrUpdateProperty("TitleIconImageUrl", definition.TitleIconImageUrl);

            return xml.ToString();
        }

        #endregion
    }
}
