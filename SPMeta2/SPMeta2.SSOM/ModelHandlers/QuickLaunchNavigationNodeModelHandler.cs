using Microsoft.SharePoint;
using Microsoft.SharePoint.Navigation;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHandlers.Base;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class QuickLaunchNavigationNodeModelHandler : NavigationNodeModelHandler<QuickLaunchNavigationNodeDefinition>
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(QuickLaunchNavigationNodeDefinition); }
        }

        #endregion

        protected override SPNavigationNodeCollection GetNavigationNodeCollection(SPWeb web)
        {
            return web.Navigation.QuickLaunch;
        }
    }
}
