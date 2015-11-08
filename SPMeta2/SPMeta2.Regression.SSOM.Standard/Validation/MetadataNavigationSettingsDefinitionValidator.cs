using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Validation
{
    public class MetadataNavigationSettingsDefinitionValidator : MetadataNavigationSettingsModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost =
                 modelHost.WithAssertAndCast<ListModelHost>("model", value => value.RequireNotNull());

            var definition = model.WithAssertAndCast<MetadataNavigationSettingsDefinition>("model", value => value.RequireNotNull());
            SPList spObject = typedModelHost.HostList;

            var assert = ServiceFactory.AssertService
                            .NewAssert(definition, spObject)
                            .ShouldNotBeNull(spObject);

            if (definition.Hierarchies.Count > 0)
            {
                // TODO
            }
            else
            {
                assert.SkipProperty(m => m.Hierarchies, "Hierarchies is empty");
            }

            if (definition.KeyFilters.Count > 0)
            {
                // TODO
            }
            else
            {
                assert.SkipProperty(m => m.KeyFilters, "KeyFilters is empty");
            }
        }

        #endregion
    }
}
