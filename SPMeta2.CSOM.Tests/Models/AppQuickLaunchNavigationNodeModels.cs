using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;

namespace SPMeta2.CSOM.Tests.Models
{
    public static class AppQuickLaunchNavigationNodeModels
    {
        public static QuickLaunchNavigationNodeDefinition RootHome = new QuickLaunchNavigationNodeDefinition
        {
            Title = "Root Home",
            Url = "/root-home",
            IsExternal = true
        };

        public static QuickLaunchNavigationNodeDefinition Help = new QuickLaunchNavigationNodeDefinition
        {
            Title = "Help 3",
            Url = "/root-help",
            IsExternal = true
        };

        public static QuickLaunchNavigationNodeDefinition MedHelp = new QuickLaunchNavigationNodeDefinition
        {
            Title = "Medical Help 3",
            Url = "/med-root-help",
            IsExternal = true
        };

        public static QuickLaunchNavigationNodeDefinition ItHelp = new QuickLaunchNavigationNodeDefinition
        {
            Title = "IT Help 3",
            Url = "/it-root-help",
            IsExternal = true
        };
    }
}
