using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Model.Definitions
{
    public static class RegWebFeatures
    {
        #region properties

        public static FeatureDefinition PublishingWeb = BuiltInWebFeatures.SharePointServerPublishing
                                                                       .Inherit()
                                                                       .Enable();


        #endregion
    }
}
