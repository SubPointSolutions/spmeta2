using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;

namespace SPMeta2.Regression.Model.Definitions
{
    public static class RegWebs
    {
        #region properties

        public static WebDefinition TeamWeb = new WebDefinition
        {
            Title = "Default Team Web",
            Description = "Some team web",
            WebTemplate = "STS#0",
            Url = "spmt2rg_team_web"
        };

        public static WebDefinition BlankWeb = new WebDefinition
        {
            Title = "Default Blank Web",
            Description = "Some blank web",
            WebTemplate = "STS#1",
            Url = "spmt2rg_blank_web"
        };

        public static WebDefinition BlogWeb = new WebDefinition
        {
            Title = "Default Blog Web",
            Description = "Some blog web",
            WebTemplate = "BLOG#0",
            Url = "spmt2rg_blog_web"
        };

        public static WebDefinition DocumentCenterWeb = new WebDefinition
        {
            Title = "Default Document Center Web",
            Description = "Some doc center web",
            WebTemplate = "BDR#0",
            Url = "spmt2rg_document_center_web"
        };

        public static WebDefinition SearchCenterWithTabsWeb = new WebDefinition
        {
            Title = "Default Search Center With Tabs",
            Description = "Some doc center web",
            WebTemplate = "SRCHCEN#0",
            Url = "spmt2rg_ss_with_tabs_web"
        };

        public static WebDefinition SearchCenterLightWeb = new WebDefinition
        {
            Title = "Default Search Center With Tabs",
            Description = "Some doc center web",
            WebTemplate = "SRCHCENTERLITE#0",
            Url = "spmt2rg_ss_light_web"
        };

        #endregion
    }
}
