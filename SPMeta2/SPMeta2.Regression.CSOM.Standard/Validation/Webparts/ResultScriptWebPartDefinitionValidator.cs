using System;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Regression.CSOM.Validation;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation.Webparts
{
    public class ResultScriptWebPartDefinitionValidator : WebPartDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(ResultScriptWebPartDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ResultScriptWebPartDefinition>("model", value => value.RequireNotNull());

            var pageItem = listItemModelHost.HostListItem;

            WithWithExistingWebPart(pageItem, definition, spObject =>
            {
                var assert = ServiceFactory.AssertService
                                           .NewAssert(model, definition, spObject)
                                                 .ShouldNotBeNull(spObject);

                if (!string.IsNullOrEmpty(definition.DataProviderJSON))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.DataProviderJSON, "DataProviderJSON is null or empty, skipping.");

                if (!string.IsNullOrEmpty(definition.EmptyMessage))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.EmptyMessage, "EmptyMessage is null or empty, skipping.");

                if (definition.ResultsPerPage.HasValue)
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.ResultsPerPage, "ResultsPerPage is null or empty, skipping.");

                if (definition.ShowResultCount.HasValue)
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.ShowResultCount, "ShowResultCount is null or empty, skipping.");

            });
        }
    }
}
