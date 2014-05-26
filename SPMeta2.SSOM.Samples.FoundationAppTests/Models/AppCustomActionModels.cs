using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;

namespace SPMeta2.SSOM.Samples.FoundationAppTests.Models
{
    public class AppCustomActionModels
    {
        #region peoprties

        public static UserCustomActionDefinition SiteScriptJQuery = new UserCustomActionDefinition
        {
            Title = "SiteScriptJQuery",
            Name = "SiteScriptJQuery",
            ScriptSrc = "~sitecollection/style library/jQuery/jquery-2.1.0.js",
            Location = "ScriptLink",
            Sequence = 100
        };

        #endregion
    }
}
