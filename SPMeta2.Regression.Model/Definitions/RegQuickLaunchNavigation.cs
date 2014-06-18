using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;

namespace SPMeta2.Regression.Model.Definitions
{
    public static class RegQuickLaunchNavigation
    {
        public static QuickLaunchNavigationNodeDefinition GoogleLink = new QuickLaunchNavigationNodeDefinition
        {
            Title = "Google",
            Url = "http://google.com",
            IsExternal = true
        };

        public static QuickLaunchNavigationNodeDefinition RelatoveLink = new QuickLaunchNavigationNodeDefinition
        {
            Title = "Home page",
            Url = "pages/home.aspx",
            IsExternal = true
        };
    }
}
