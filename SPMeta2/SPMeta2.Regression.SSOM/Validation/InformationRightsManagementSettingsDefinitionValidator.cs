using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class InformationRightsManagementSettingsDefinitionValidator : InformationRightsManagementSettingsModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<InformationRightsManagementSettingsDefinition>("model", value => value.RequireNotNull());

            //var assert = ServiceFactory.AssertService
            //      .NewAssert(definition, definition, solution)
            //      .ShouldNotBeNull(solution)
            //      .SkipProperty(m => m.FileName, "Skipping FileName property.")
            //      .ShouldBeEqual(m => m.SolutionId, o => o.SolutionId)
            //      .SkipProperty(m => m.Content, "Skipping Content property.");
        }

        #endregion
    }
}
