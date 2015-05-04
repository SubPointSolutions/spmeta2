using System;
using Microsoft.SharePoint.Publishing.WebControls;
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
    public class ContentByQueryWebPartDefinitionValidator : WebPartDefinitionValidator
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ContentByQueryWebPartDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            // base validation
            base.DeployModel(modelHost, model);

            // content editor specific validation

            var host = modelHost.WithAssertAndCast<WebpartPageModelHost>("modelHost", value => value.RequireNotNull());
            var typedModel = model.WithAssertAndCast<ContentByQueryWebPartDefinition>("model", value => value.RequireNotNull());

            var item = host.PageListItem;
            WebPartExtensions.WithExistingWebPart(item, typedModel, (spWebPartManager, spObject) =>
            {
                var typedWebPart = spObject as ContentByQueryWebPart;

                var assert = ServiceFactory.AssertService
                                           .NewAssert(typedModel, typedWebPart)
                                           .ShouldNotBeNull(typedWebPart);

                if (!string.IsNullOrEmpty(typedModel.ContentTypeBeginsWithId))
                    assert.ShouldBeEqual(m => m.ContentTypeBeginsWithId, o => o.ContentTypeBeginsWithId);
                else
                    assert.SkipProperty(m => m.ContentTypeBeginsWithId, "ContentTypeBeginsWithId is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedModel.DataMappings))
                    assert.ShouldBeEqual(m => m.DataMappings, o => o.DataMappings);
                else
                    assert.SkipProperty(m => m.DataMappings, "DataMappings is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedModel.DataMappingViewFields))
                    assert.ShouldBeEqual(m => m.DataMappingViewFields, o => o.DataMappingViewFields);
                else
                    assert.SkipProperty(m => m.DataMappingViewFields, "DataMappingViewFields is null or empty, skipping.");

                // filter display value 1-2-3
                if (!string.IsNullOrEmpty(typedModel.FilterDisplayValue1))
                    assert.ShouldBeEqual(m => m.FilterDisplayValue1, o => o.FilterDisplayValue1);
                else
                    assert.SkipProperty(m => m.FilterDisplayValue1, "FilterDisplayValue1 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedModel.FilterDisplayValue2))
                    assert.ShouldBeEqual(m => m.FilterDisplayValue2, o => o.FilterDisplayValue2);
                else
                    assert.SkipProperty(m => m.FilterDisplayValue2, "FilterDisplayValue2 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedModel.FilterDisplayValue3))
                    assert.ShouldBeEqual(m => m.FilterDisplayValue3, o => o.FilterDisplayValue3);
                else
                    assert.SkipProperty(m => m.FilterDisplayValue3, "FilterDisplayValue3 is null or empty, skipping.");

                // operators 1-2-3
                if (!string.IsNullOrEmpty(typedModel.FilterOperator1))
                {

                }
                else
                    assert.SkipProperty(m => m.FilterOperator1, "FilterOperator1 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedModel.FilterOperator2))
                {

                }
                else
                    assert.SkipProperty(m => m.FilterOperator2, "FilterOperator2 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedModel.FilterOperator3))
                {

                }
                else
                    assert.SkipProperty(m => m.FilterOperator3, "FilterOperator3 is null or empty, skipping.");

                // type 1-2-3
                if (!string.IsNullOrEmpty(typedModel.FilterType1))
                    assert.ShouldBeEqual(m => m.FilterType1, o => o.FilterType1);
                else
                    assert.SkipProperty(m => m.FilterType1, "FilterType1 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedModel.FilterType2))
                    assert.ShouldBeEqual(m => m.FilterType2, o => o.FilterType2);
                else
                    assert.SkipProperty(m => m.FilterType2, "FilterType2 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedModel.FilterType3))
                    assert.ShouldBeEqual(m => m.FilterType3, o => o.FilterType3);
                else
                    assert.SkipProperty(m => m.FilterType3, "FilterType3 is null or empty, skipping.");

                // filter values 1-2-3
                if (!string.IsNullOrEmpty(typedModel.FilterValue1))
                    assert.ShouldBeEqual(m => m.FilterValue1, o => o.FilterValue1);
                else
                    assert.SkipProperty(m => m.FilterValue1, "FilterValue1 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedModel.FilterValue2))
                    assert.ShouldBeEqual(m => m.FilterValue2, o => o.FilterValue2);
                else
                    assert.SkipProperty(m => m.FilterValue2, "FilterValue2 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedModel.FilterValue3))
                    assert.ShouldBeEqual(m => m.FilterValue3, o => o.FilterValue3);
                else
                    assert.SkipProperty(m => m.FilterValue3, "FilterValue3 is null or empty, skipping.");

                // sorting

                if (!string.IsNullOrEmpty(typedModel.SortBy))
                    assert.ShouldBeEqual(m => m.SortBy, o => o.SortBy);
                else
                    assert.SkipProperty(m => m.SortBy, "SortBy is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedModel.SortByFieldType))
                    assert.ShouldBeEqual(m => m.SortByFieldType, o => o.SortByFieldType);
                else
                    assert.SkipProperty(m => m.SortByFieldType, "SortByFieldType is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedModel.SortByDirection))
                {
                    // TODO
                }
                else
                    assert.SkipProperty(m => m.SortByDirection, "SortByDirection is null or empty, skipping.");

                // xslt
                if (!string.IsNullOrEmpty(typedModel.GroupStyle))
                    assert.ShouldBeEqual(m => m.GroupStyle, o => o.GroupStyle);
                else
                    assert.SkipProperty(m => m.GroupStyle, "GroupStyle is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedModel.ItemStyle))
                    assert.ShouldBeEqual(m => m.ItemStyle, o => o.ItemStyle);
                else
                    assert.SkipProperty(m => m.ItemStyle, "ItemStyle is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedModel.ItemXslLink))
                {
                    // TODO
                }
                else
                    assert.SkipProperty(m => m.ItemXslLink, "ItemXslLink is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedModel.MainXslLink))
                {
                    // TODO
                }
                else
                    assert.SkipProperty(m => m.MainXslLink, "MainXslLink is null or empty, skipping.");


                // list name/url/id
                if (!string.IsNullOrEmpty(typedModel.ListName))
                {
                    // TODO
                }
                else
                    assert.SkipProperty(m => m.ListName, "ListName is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedModel.ListUrl))
                {
                    // TODO
                }
                else
                    assert.SkipProperty(m => m.ListUrl, "ListUrl is null or empty, skipping.");

                if (typedModel.ListGuid.HasValue)
                {
                    // TODO
                }
                else
                    assert.SkipProperty(m => m.ListGuid, "ListGuid is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedModel.WebUrl))
                {
                    // TODO
                }
                else
                    assert.SkipProperty(m => m.WebUrl, "WebUrl is null or empty, skipping.");

                // misc

                if (typedModel.ItemLimit.HasValue)
                {
                    // TODO
                }
                else
                    assert.SkipProperty(m => m.ItemLimit, "ItemLimit is null or empty, skipping.");

                if (typedModel.PlayMediaInBrowser.HasValue)
                {
                    // TODO
                }
                else
                    assert.SkipProperty(m => m.PlayMediaInBrowser, "PlayMediaInBrowser is null or empty, skipping.");

                if (typedModel.ShowUntargetedItems.HasValue)
                {
                    // TODO
                }
                else
                    assert.SkipProperty(m => m.ShowUntargetedItems, "ShowUntargetedItems is null or empty, skipping.");

                if (typedModel.UseCopyUtil.HasValue)
                {
                    // TODO
                }
                else
                    assert.SkipProperty(m => m.UseCopyUtil, "UseCopyUtil is null or empty, skipping.");

                if (typedModel.ServerTemplate.HasValue)
                {
                    // TODO
                }
                else
                    assert.SkipProperty(m => m.ServerTemplate, "ServerTemplate is null or empty, skipping.");

            });
        }

        #endregion
    }
}
