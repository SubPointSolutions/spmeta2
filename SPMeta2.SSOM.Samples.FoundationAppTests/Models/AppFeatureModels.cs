using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;

namespace SPMeta2.SSOM.Samples.FoundationAppTests.Models
{
    public static class AppSiteFeatureModels
    {
        #region properties

        public static FeatureDefinition Workflows = new FeatureDefinition
        {
            Title = "Workflows",
            Id = new Guid("0af5989a-3aea-4519-8ab0-85d91abe39ff"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = true,
            Enable = false
        };

        #endregion
    }

    public static class AppWebFeatureModels
    {
        #region properties

        public static FeatureDefinition WorkflowsCanUseAppPermissions = new FeatureDefinition
        {
            Title = "Workflows can use app permissions",
            Id = new Guid("ec918931-c874-4033-bd09-4f36b2e31fef"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = true,
            Enable = false
        };

        #endregion
    }
}
