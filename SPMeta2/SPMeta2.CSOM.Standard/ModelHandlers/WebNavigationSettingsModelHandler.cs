using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Publishing.Navigation;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions;
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

        protected WebNavigationSettings GetWebNavigationSettings(WebModelHost webModelHost, WebNavigationSettingsDefinition navigationModel)
        {
            var web = webModelHost.HostWeb;

            var context = web.Context;
            var thisWebNavSettings = new WebNavigationSettings(context, web);

            context.Load(thisWebNavSettings);
            context.ExecuteQuery();

            return thisWebNavSettings;
        }

        private void DeployNavigationSettings(object modelHost, WebModelHost webModelHost, WebNavigationSettingsDefinition navigationModel)
        {
            var web = webModelHost.HostWeb;
            var context = web.Context;

            var thisWebNavSettings = GetWebNavigationSettings(webModelHost, navigationModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = thisWebNavSettings,
                ObjectType = typeof(WebNavigationSettings),
                ObjectDefinition = navigationModel,
                ModelHost = modelHost
            });



            if (!string.IsNullOrEmpty(navigationModel.GlobalNavigationSource) ||
                !string.IsNullOrEmpty(navigationModel.CurrentNavigationSource))
            {
                if (!string.IsNullOrEmpty(navigationModel.GlobalNavigationSource))
                    thisWebNavSettings.GlobalNavigation.Source = (StandardNavigationSource)Enum.Parse(typeof(StandardNavigationSource), navigationModel.GlobalNavigationSource);

                if (!string.IsNullOrEmpty(navigationModel.CurrentNavigationSource))
                    thisWebNavSettings.CurrentNavigation.Source = (StandardNavigationSource)Enum.Parse(typeof(StandardNavigationSource), navigationModel.CurrentNavigationSource);
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = thisWebNavSettings,
                ObjectType = typeof(WebNavigationSettings),
                ObjectDefinition = navigationModel,
                ModelHost = modelHost
            });

            if (!string.IsNullOrEmpty(navigationModel.GlobalNavigationSource) ||
                !string.IsNullOrEmpty(navigationModel.CurrentNavigationSource))
            {
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
                web.AllProperties[BuiltInWebPropertyId.CurrentNavigationIncludeTypes] = currentNavigationIncludeTypes;

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
                web.AllProperties[BuiltInWebPropertyId.GlobalNavigationIncludeTypes] = globalNavigationIncludeTypes;

            if (navigationModel.CurrentNavigationMaximumNumberOfDynamicItems.HasValue)
                web.AllProperties[BuiltInWebPropertyId.CurrentDynamicChildLimit] = navigationModel.CurrentNavigationMaximumNumberOfDynamicItems.Value;

            if (navigationModel.GlobalNavigationMaximumNumberOfDynamicItems.HasValue)
                web.AllProperties[BuiltInWebPropertyId.GlobalDynamicChildLimit] = navigationModel.GlobalNavigationMaximumNumberOfDynamicItems.Value;

            if (currentNavigationIncludeTypes != null ||
                globalNavigationIncludeTypes != null ||
                navigationModel.CurrentNavigationMaximumNumberOfDynamicItems.HasValue ||
                navigationModel.GlobalNavigationMaximumNumberOfDynamicItems.HasValue)
            {
                web.Update();
                context.ExecuteQuery();
            }
        }

        #endregion
    }
}
