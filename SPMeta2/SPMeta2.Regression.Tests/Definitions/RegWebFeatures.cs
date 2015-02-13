using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Definitions
{
    internal static class RegWebFeatures
    {
        public static FeatureDefinition Publishing = BuiltInWebFeatures.SharePointServerPublishing
                                                               .Inherit(f =>
                                                               {
                                                                   f.Enable = true;
                                                               });
    }
}
