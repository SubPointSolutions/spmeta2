using System;
using Microsoft.Office.Server.Search.WebControls;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Regression.SSOM.Validation;
using SPMeta2.SSOM.Extensions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Validation.Webparts
{
    public class ResultScriptWebPartDefinitionValidator : WebPartDefinitionValidator
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ResultScriptWebPartDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            // base validation
            base.DeployModel(modelHost, model);

            var host = modelHost.WithAssertAndCast<WebpartPageModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ResultScriptWebPartDefinition>("model", value => value.RequireNotNull());

            //var item = host.PageListItem;

            WebPartExtensions.WithExistingWebPart(host.HostFile, definition, (spWebPartManager, spObject) =>
            {
                var typedWebPart = spObject as ResultScriptWebPart;

                var assert = ServiceFactory.AssertService
                    .NewAssert(definition, typedWebPart)
                    .ShouldNotBeNull(typedWebPart);

                if (!string.IsNullOrEmpty(definition.DataProviderJSON))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.DataProviderJSON, "DataProviderJSON is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.EmptyMessage))
                    assert.ShouldBeEqual(m => m.EmptyMessage, o => o.EmptyMessage);
                else
                    assert.SkipProperty(m => m.EmptyMessage, "EmptyMessage is null or empty, skipping.");

                if (definition.ResultsPerPage.HasValue)
                    assert.ShouldBeEqual(m => m.ResultsPerPage.Value, o => o.ResultsPerPage);
                else
                    assert.SkipProperty(m => m.ResultsPerPage, "ResultsPerPage is null or empty, skipping.");

                if (definition.ShowResultCount.HasValue)
                    assert.ShouldBeEqual(m => m.ShowResultCount.Value, o => o.ShowResultCount);
                else
                    assert.SkipProperty(m => m.ShowResultCount, "ShowResultCount is null or empty, skipping.");

                if (definition.ShowLanguageOptions.HasValue)
                    assert.ShouldBeEqual(m => m.ShowLanguageOptions.Value, o => o.ShowLanguageOptions);
                else
                    assert.SkipProperty(m => m.ShowLanguageOptions, "ShowLanguageOptions is null or empty, skipping.");

                if (definition.MaxPagesBeforeCurrent.HasValue)
                    assert.ShouldBeEqual(m => m.MaxPagesBeforeCurrent.Value, o => o.MaxPagesBeforeCurrent);
                else
                    assert.SkipProperty(m => m.MaxPagesBeforeCurrent, "MaxPagesBeforeCurrent is null or empty, skipping.");

                if (definition.ShowBestBets.HasValue)
                    assert.ShouldBeEqual(m => m.ShowBestBets.Value, o => o.ShowBestBets);
                else
                    assert.SkipProperty(m => m.ShowBestBets, "ShowBestBets is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.AdvancedSearchPageAddress))
                    assert.ShouldBeEqual(m => m.AdvancedSearchPageAddress, o => o.AdvancedSearchPageAddress);
                else
                    assert.SkipProperty(m => m.AdvancedSearchPageAddress, "AdvancedSearchPageAddress is null or empty, skipping.");

                if (definition.UseSharedDataProvider.HasValue)
                    assert.ShouldBeEqual(m => m.UseSharedDataProvider.Value, o => o.UseSharedDataProvider);
                else
                    assert.SkipProperty(m => m.UseSharedDataProvider, "UseSharedDataProvider is null or empty, skipping.");

                if (definition.ShowPreferencesLink.HasValue)
                    assert.ShouldBeEqual(m => m.ShowPreferencesLink.Value, o => o.ShowPreferencesLink);
                else
                    assert.SkipProperty(m => m.ShowPreferencesLink, "ShowPreferencesLink is null or empty, skipping.");

                if (definition.ShowViewDuplicates.HasValue)
                    assert.ShouldBeEqual(m => m.ShowViewDuplicates.Value, o => o.ShowViewDuplicates);
                else
                    assert.SkipProperty(m => m.ShowViewDuplicates, "ShowViewDuplicates is null or empty, skipping.");

                if (definition.RepositionLanguageDropDown.HasValue)
                    assert.ShouldBeEqual(m => m.RepositionLanguageDropDown.Value, o => o.RepositionLanguageDropDown);
                else
                    assert.SkipProperty(m => m.RepositionLanguageDropDown, "RepositionLanguageDropDown is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.PreloadedItemTemplateIdsJson))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.PreloadedItemTemplateIdsJson, "PreloadedItemTemplateIdsJson is null or empty, skipping.");

                if (definition.ShowPaging.HasValue)
                    assert.ShouldBeEqual(m => m.ShowPaging.Value, o => o.ShowPaging);
                else
                    assert.SkipProperty(m => m.ShowPaging, "ShowPaging is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.ResultTypeId))
                    assert.ShouldBeEqual(m => m.ResultTypeId, o => o.ResultTypeId);
                else
                    assert.SkipProperty(m => m.ResultTypeId, "ResultTypeId is null or empty, skipping.");

                if (definition.ShowResults.HasValue)
                    assert.ShouldBeEqual(m => m.ShowResults.Value, o => o.ShowResults);
                else
                    assert.SkipProperty(m => m.ShowResults, "ShowResults is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.ItemTemplateId))
                    assert.ShouldBeEqual(m => m.ItemTemplateId, o => o.ItemTemplateId);
                else
                    assert.SkipProperty(m => m.ItemTemplateId, "ItemTemplateId is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.ItemBodyTemplateId))
                    assert.ShouldBeEqual(m => m.ItemBodyTemplateId, o => o.ItemBodyTemplateId);
                else
                    assert.SkipProperty(m => m.ItemBodyTemplateId, "ItemBodyTemplateId is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.HitHighlightedPropertiesJson))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.HitHighlightedPropertiesJson, "HitHighlightedPropertiesJson is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.AvailableSortsJson))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.AvailableSortsJson, "AvailableSortsJson is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.RenderTemplateId))
                    assert.ShouldBeEqual(m => m.RenderTemplateId, o => o.RenderTemplateId);
                else
                    assert.SkipProperty(m => m.RenderTemplateId, "RenderTemplateId is null or empty, skipping.");

                if (definition.ShowPersonalFavorites.HasValue)
                    assert.ShouldBeEqual(m => m.ShowPersonalFavorites.Value, o => o.ShowPersonalFavorites);
                else
                    assert.SkipProperty(m => m.ShowPersonalFavorites, "ShowPersonalFavorites is null or empty, skipping.");

                if (definition.ShowSortOptions.HasValue)
                    assert.ShouldBeEqual(m => m.ShowSortOptions.Value, o => o.ShowSortOptions);
                else
                    assert.SkipProperty(m => m.ShowSortOptions, "ShowSortOptions is null or empty, skipping.");

                if (definition.ShowAlertMe.HasValue)
                    assert.ShouldBeEqual(m => m.ShowAlertMe.Value, o => o.ShowAlertMe);
                else
                    assert.SkipProperty(m => m.ShowAlertMe, "ShowAlertMe is null or empty, skipping.");

                if (definition.ShowDidYouMean.HasValue)
                    assert.ShouldBeEqual(m => m.ShowDidYouMean.Value, o => o.ShowDidYouMean);
                else
                    assert.SkipProperty(m => m.ShowDidYouMean, "ShowDidYouMean is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.QueryGroupName))
                    assert.ShouldBeEqual(m => m.QueryGroupName, o => o.QueryGroupName);
                else
                    assert.SkipProperty(m => m.QueryGroupName, "QueryGroupName is null or empty, skipping.");

                if (definition.ShowAdvancedLink.HasValue)
                    assert.ShouldBeEqual(m => m.ShowAdvancedLink.Value, o => o.ShowAdvancedLink);
                else
                    assert.SkipProperty(m => m.ShowAdvancedLink, "ShowAdvancedLink is null or empty, skipping.");

                if (definition.BypassResultTypes.HasValue)
                    assert.ShouldBeEqual(m => m.BypassResultTypes.Value, o => o.BypassResultTypes);
                else
                    assert.SkipProperty(m => m.BypassResultTypes, "BypassResultTypes is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.GroupTemplateId))
                    assert.ShouldBeEqual(m => m.GroupTemplateId, o => o.GroupTemplateId);
                else
                    assert.SkipProperty(m => m.GroupTemplateId, "GroupTemplateId is null or empty, skipping.");

            });
        }

        #endregion
    }
}
