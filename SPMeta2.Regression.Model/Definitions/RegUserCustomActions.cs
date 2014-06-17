using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;

namespace SPMeta2.Regression.Model.Definitions
{
    public static class RegUserCustomActions
    {
        #region properties

        public static UserCustomActionDefinition jQueryScript = new UserCustomActionDefinition
        {
            Location = "ScriptLink",
            Name = "jQuery",
            Title = "jQuery",
            Group = "spmeta2",
            Description = "Global jQuery registration",
            ScriptSrc = "~SiteCollection/SiteAssets/jquery-1.9.1.js",
            Sequence = 100
        };

        #endregion
    }
}
