using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers.Base;
using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.CSOM.ModelHandlers
{
    public class TopNavigationNodeModelHandler : NavigationNodeModelHandler<TopNavigationNodeDefinition>
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(TopNavigationNodeDefinition); }
        }

        #endregion

        #region methods

        protected override NavigationNodeCollection GetNavigationNodeCollection(Web web)
        {
            return web.Navigation.TopNavigationBar;
        }

        #endregion
    }
}
