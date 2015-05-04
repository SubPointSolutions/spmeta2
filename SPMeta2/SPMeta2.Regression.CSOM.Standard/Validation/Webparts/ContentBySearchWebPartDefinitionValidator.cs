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
    public class ContentBySearchWebPartDefinitionValidator : WebPartDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(ContentBySearchWebPartDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ContentBySearchWebPartDefinition>("model", value => value.RequireNotNull());

            var pageItem = listItemModelHost.HostListItem;

            WithWithExistingWebPart(pageItem, definition, spObject =>
            {
                var assert = ServiceFactory.AssertService
                                           .NewAssert(model, definition, spObject)
                                                 .ShouldNotBeNull(spObject);

                if (!string.IsNullOrEmpty(definition.GroupTemplateId))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.GroupTemplateId, "GroupTemplateId is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.ItemTemplateId))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.ItemTemplateId, "ItemTemplateId is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.RenderTemplateId))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.RenderTemplateId, "RenderTemplateId is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.DataProviderJSON))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.DataProviderJSON, "DataProviderJSON is null or empty, skipping.");

                if (definition.NumberOfItems.HasValue)
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.NumberOfItems, "NumberOfItems is null or empty, skipping.");

                if (definition.ResultsPerPage.HasValue)
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.ResultsPerPage, "ResultsPerPage is null or empty, skipping.");

                //
                if (!string.IsNullOrEmpty(definition.PropertyMappings))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.PropertyMappings, "PropertyMappings is null or empty, skipping.");

                if (definition.OverwriteResultPath.HasValue)
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.OverwriteResultPath, "OverwriteResultPath is null or empty, skipping.");

                if (definition.ShouldHideControlWhenEmpty.HasValue)
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.ShouldHideControlWhenEmpty, "ShouldHideControlWhenEmpty is null or empty, skipping.");

                if (definition.LogAnalyticsViewEvent.HasValue)
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.LogAnalyticsViewEvent, "LogAnalyticsViewEvent is null or empty, skipping.");

                if (definition.AddSEOPropertiesFromSearch.HasValue)
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.AddSEOPropertiesFromSearch, "AddSEOPropertiesFromSearch is null or empty, skipping.");

                if (definition.StartingItemIndex.HasValue)
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.StartingItemIndex, "StartingItemIndex is null or empty, skipping.");
            });
        }
    }
}
