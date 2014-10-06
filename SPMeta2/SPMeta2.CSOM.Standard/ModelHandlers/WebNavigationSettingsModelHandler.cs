using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client.Publishing.Navigation;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.Definitions;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers
{
    public class WebNavigationSettingsModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(WebNavigationSettingsDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var navigationModel = model.WithAssertAndCast<WebNavigationSettingsDefinition>("model", value => value.RequireNotNull());

            DeployNavigationSettings(modelHost, webModelHost, navigationModel);
        }

        private void DeployNavigationSettings(object modelHost, WebModelHost webModelHost, WebNavigationSettingsDefinition navigationModel)
        {
            var web = webModelHost.HostWeb;
            var context = web.Context;

            var thisWebNavSettings = new WebNavigationSettings(context, web);

            thisWebNavSettings.GlobalNavigation.Source = (StandardNavigationSource)Enum.Parse(typeof(StandardNavigationSource), navigationModel.GlobalNavigationSource);
            thisWebNavSettings.CurrentNavigation.Source = (StandardNavigationSource)Enum.Parse(typeof(StandardNavigationSource), navigationModel.CurrentNavigationSource);

            thisWebNavSettings.Update(null);
            context.ExecuteQuery();
        }

        #endregion
    }
}
