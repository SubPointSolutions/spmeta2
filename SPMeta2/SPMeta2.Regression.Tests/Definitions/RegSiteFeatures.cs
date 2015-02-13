using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Definitions
{
    internal static class RegSiteFeatures
    {
        public static FeatureDefinition Publishing = BuiltInSiteFeatures.SharePointServerPublishingInfrastructure
                                                               .Inherit(f =>
                                                               {
                                                                   f.Enable = true;
                                                               });
    }
}
