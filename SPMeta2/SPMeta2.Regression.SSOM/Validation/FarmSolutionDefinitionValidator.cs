using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;

using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using Microsoft.SharePoint;
using SPMeta2.Regression.Assertion;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class FarmSolutionDefinitionValidator : FarmSolutionModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FarmSolutionDefinition>("model", value => value.RequireNotNull());
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());

            var solution = FindExistingSolution(farmModelHost, definition);

            ServiceFactory.AssertService
                .NewAssert(definition, definition, solution)
                .ShouldNotBeNull(solution)
                .SkipProperty(m => m.FileName, "Skipping FileName property.")
                .ShouldBeEqual(m => m.SolutionId, o => o.SolutionId)
                .SkipProperty(m => m.Content, "Skipping Content property.");
        }
    }
}
