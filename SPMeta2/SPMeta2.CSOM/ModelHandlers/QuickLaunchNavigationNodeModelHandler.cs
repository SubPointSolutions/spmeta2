using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
using SPMeta2.Common;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHandlers.Base;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class QuickLaunchNavigationNodeModelHandler : NavigationNodeModelHandler<QuickLaunchNavigationNodeDefinition>
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(QuickLaunchNavigationNodeDefinition); }
        }

        #endregion

        #region methods

        protected override NavigationNodeCollection GetNavigationNodeCollection(Web web)
        {
            return web.Navigation.QuickLaunch;
        }

        #endregion
    }
}
