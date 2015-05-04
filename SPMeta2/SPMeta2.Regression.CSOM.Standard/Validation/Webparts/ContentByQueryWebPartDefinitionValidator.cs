using System;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Utils;
using SPMeta2.Regression.CSOM.Validation;

namespace SPMeta2.Regression.CSOM.Standard.Validation.Webparts
{
    public class ContentByQueryWebPartDefinitionValidator : WebPartDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(ContentByQueryWebPartDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ContentByQueryWebPartDefinition>("model", value => value.RequireNotNull());

            var pageItem = listItemModelHost.HostListItem;

            WithWithExistingWebPart(pageItem, definition, spObject =>
            {
                var assert = ServiceFactory.AssertService
                                           .NewAssert(model, definition, spObject)
                                                 .ShouldNotBeNull(spObject);

                if (!string.IsNullOrEmpty(definition.ContentTypeBeginsWithId))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.ContentTypeBeginsWithId, "ContentTypeBeginsWithId is null or empty, skipping.");

                // mappings
                if (!string.IsNullOrEmpty(definition.DataMappings))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.DataMappings, "DataMappings is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.DataMappingViewFields))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.DataMappingViewFields, "DataMappingViewFields is null or empty, skipping.");

                // filter display values

                if (!string.IsNullOrEmpty(definition.FilterDisplayValue1))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.FilterDisplayValue1, "FilterDisplayValue1 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.FilterDisplayValue2))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.FilterDisplayValue2, "FilterDisplayValue2 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.FilterDisplayValue3))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.FilterDisplayValue3, "FilterDisplayValue3 is null or empty, skipping.");

                // filter operator

                if (!string.IsNullOrEmpty(definition.FilterOperator1))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.FilterOperator1, "FilterOperator1 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.FilterOperator2))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.FilterOperator2, "FilterOperator2 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.FilterOperator3))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.FilterOperator3, "FilterOperator3 is null or empty, skipping.");

                // filter types

                if (!string.IsNullOrEmpty(definition.FilterType1))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.FilterType1, "FilterType1 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.FilterType2))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.FilterType2, "FilterType2 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.FilterType3))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.FilterType3, "FilterType3 is null or empty, skipping.");



                if (!string.IsNullOrEmpty(definition.SortBy))
                {
                    // TODO
                }
                else
                    assert.SkipProperty(m => m.SortBy, "SortBy is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.SortByFieldType))
                {
                    // TODO
                }
                else
                    assert.SkipProperty(m => m.SortByFieldType, "SortByFieldType is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.SortByDirection))
                {
                    // TODO
                }
                else
                    assert.SkipProperty(m => m.SortByDirection, "SortByDirection is null or empty, skipping.");

                // filter values

                if (!string.IsNullOrEmpty(definition.FilterValue1))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.FilterValue1, "FilterValue1 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.FilterValue2))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.FilterValue2, "FilterValue2 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.FilterValue3))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.FilterValue3, "FilterValue3 is null or empty, skipping.");

                // styles

                if (!string.IsNullOrEmpty(definition.GroupStyle))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.GroupStyle, "GroupStyle is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.ItemStyle))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.ItemStyle, "ItemStyle is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.ItemXslLink))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.ItemXslLink, "ItemXslLink is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.MainXslLink))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.MainXslLink, "MainXslLink is null or empty, skipping.");

                // list bindings

                if (!string.IsNullOrEmpty(definition.WebUrl))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.WebUrl, "WebUrl is null or empty, skipping.");

                if (definition.ListGuid.HasGuidValue())
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.ListGuid, "ListGuid is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.ListName))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.ListName, "ListName is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.ListUrl))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.ListUrl, "ListUrl is null or empty, skipping.");

                // misc

                if (definition.ServerTemplate.HasValue)
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.ServerTemplate, "ServerTemplate is null or empty, skipping.");

                if (definition.ItemLimit.HasValue)
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.ItemLimit, "ItemLimit is null or empty, skipping.");

                if (definition.PlayMediaInBrowser.HasValue)
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.PlayMediaInBrowser, "PlayMediaInBrowser is null or empty, skipping.");

                if (definition.ShowUntargetedItems.HasValue)
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.ShowUntargetedItems, "ShowUntargetedItems is null or empty, skipping.");

                if (definition.UseCopyUtil.HasValue)
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.UseCopyUtil, "UseCopyUtil is null or empty, skipping.");

            });
        }
    }
}
