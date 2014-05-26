using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Definitions
{
    public class QuickLaunchNavigationNodeDefinition : DefinitionBase
    {
        #region contructors

        public QuickLaunchNavigationNodeDefinition()
        {
            IsVisible = true;
        }

        #endregion

        #region properties

        public string Title { get; set; }
        public string Url { get; set; }

        public bool IsExternal { get; set; }
        public bool IsVisible { get; set; }

        #endregion
    }
}
