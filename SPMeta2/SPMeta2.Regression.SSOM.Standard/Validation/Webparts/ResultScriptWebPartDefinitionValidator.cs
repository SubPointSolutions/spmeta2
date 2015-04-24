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

            var item = host.PageListItem;
            WebPartExtensions.WithExistingWebPart(item, definition, (spWebPartManager, spObject) =>
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

        #endregion
    }
}
