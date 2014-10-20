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
    public class SandboxSolutionDefinitionValidator : SandboxSolutionModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<SandboxSolutionDefinition>("model", value => value.RequireNotNull());
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());

            var solution = FindExistingSolution(siteModelHost, definition);

            ServiceFactory.AssertService
                            .NewAssert(definition, definition, solution)
                                .ShouldNotBeNull(solution)
                                .ShouldBeEqual(m => m.SolutionId, o => o.SolutionId)
                                .ShouldBeEqual(m => m.Activate, o => o.IsActivated())
                                .SkipProperty(m => m.FileName, "Solution should be deployed fine.")
                                .SkipProperty(m => m.Content, "Solution should be deployed fine.");

            // TODO, check feature activation

        }
    }

    internal static class SPUserSolutionExtensions
    {
        public static bool IsActivated(this SPUserSolution solution)
        {
            return solution.Status == SPUserSolutionStatus.Activated;
        }
    }
}
