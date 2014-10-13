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

            if (!string.IsNullOrEmpty(navigationModel.GlobalNavigationSource) ||
                !string.IsNullOrEmpty(navigationModel.CurrentNavigationSource))
            {
                var thisWebNavSettings = new WebNavigationSettings(context, web);

                if (!string.IsNullOrEmpty(navigationModel.GlobalNavigationSource))
                    thisWebNavSettings.GlobalNavigation.Source = (StandardNavigationSource)Enum.Parse(typeof(StandardNavigationSource), navigationModel.GlobalNavigationSource);

                if (!string.IsNullOrEmpty(navigationModel.CurrentNavigationSource))
                    thisWebNavSettings.CurrentNavigation.Source = (StandardNavigationSource)Enum.Parse(typeof(StandardNavigationSource), navigationModel.CurrentNavigationSource);

                thisWebNavSettings.Update(null);
                context.ExecuteQuery();
            }

            // update include types
            int? currentNavigationIncludeTypes = null;

            if (navigationModel.CurrentNavigationShowPages == false &&
                navigationModel.CurrentNavigationShowSubsites == false)
                currentNavigationIncludeTypes = 0;
            else if (navigationModel.CurrentNavigationShowPages == true &&
               navigationModel.CurrentNavigationShowSubsites == true)
                currentNavigationIncludeTypes = 3;
            else if (navigationModel.CurrentNavigationShowPages == true)
                currentNavigationIncludeTypes = 2;
            else if (navigationModel.CurrentNavigationShowSubsites == true)
                currentNavigationIncludeTypes = 1;

            if (currentNavigationIncludeTypes != null)
                web.AllProperties["__CurrentNavigationIncludeTypes"] = currentNavigationIncludeTypes;

            int? globalNavigationIncludeTypes = null;

            if (navigationModel.GlobalNavigationShowPages == false &&
                navigationModel.GlobalNavigationShowSubsites == false)
                globalNavigationIncludeTypes = 0;
            else if (navigationModel.GlobalNavigationShowPages == true &&
               navigationModel.GlobalNavigationShowSubsites == true)
                globalNavigationIncludeTypes = 3;
            else if (navigationModel.GlobalNavigationShowPages == true)
                globalNavigationIncludeTypes = 2;
            else if (navigationModel.GlobalNavigationShowSubsites == true)
                globalNavigationIncludeTypes = 1;

            if (globalNavigationIncludeTypes != null)
                web.AllProperties["__GlobalNavigationIncludeTypes"] = globalNavigationIncludeTypes;

            if (currentNavigationIncludeTypes != null || globalNavigationIncludeTypes != null)
            {
                web.Update();
                context.ExecuteQuery();
            }
        }

        #endregion
    }
}
