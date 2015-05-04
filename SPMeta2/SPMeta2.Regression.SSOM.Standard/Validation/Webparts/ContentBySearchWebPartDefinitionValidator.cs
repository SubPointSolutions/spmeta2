using System;
using Microsoft.Office.Server.Search.WebControls;
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
    public class ContentBySearchWebPartDefinitionValidator : WebPartDefinitionValidator
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ContentBySearchWebPartDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            // base validation
            base.DeployModel(modelHost, model);

            // content editor specific validation

            var host = modelHost.WithAssertAndCast<WebpartPageModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ContentBySearchWebPartDefinition>("model", value => value.RequireNotNull());

            var item = host.PageListItem;
            WebPartExtensions.WithExistingWebPart(item, definition, (spWebPartManager, spObject) =>
            {
                var typedWebPart = spObject as ContentBySearchWebPart;

                var assert = ServiceFactory.AssertService
                                           .NewAssert(definition, typedWebPart)
                                           .ShouldNotBeNull(typedWebPart);

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

                //

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

            });
        }

        #endregion
    }
}
