using Microsoft.SharePoint.Navigation;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Utils;
using SPMeta2.Common;
using SPMeta2.SSOM.ModelHandlers.Base;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class TopNavigationNodeModelHandler : NavigationNodeModelHandler<TopNavigationNodeDefinition>
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(TopNavigationNodeDefinition); }
        }

        #endregion

        protected override SPNavigationNodeCollection GetNavigationNodeCollection(Microsoft.SharePoint.SPWeb web)
        {
            return web.Navigation.TopNavigationBar;
        }
    }
}
