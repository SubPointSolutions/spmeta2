using System;
using Microsoft.SharePoint.Publishing.Navigation;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers.Taxonomy;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;
using SPMeta2.SSOM.Standard.ModelHandlers.Fields;

namespace SPMeta2.SSOM.Standard.ModelHandlers
{
    public class WebNavigationSettingsModelHandler : SSOMModelHandlerBase
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
            var thisWebNavSettings = new WebNavigationSettings(web);

            return thisWebNavSettings;
        }

        private void DeployNavigationSettings(object modelHost, WebModelHost webModelHost, WebNavigationSettingsDefinition navigationModel)
        {
            var site = webModelHost.HostWeb.Site;
            var web = webModelHost.HostWeb;
            var thisWebNavSettings = GetWebNavigationSettings(webModelHost, navigationModel);

            var shouldUpdateWeb = false;

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
                {
                    var globalSource = (StandardNavigationSource)Enum.Parse(typeof(StandardNavigationSource), navigationModel.GlobalNavigationSource);

                    thisWebNavSettings.GlobalNavigation.Source = globalSource;

                    if (globalSource == StandardNavigationSource.TaxonomyProvider)
                    {
                        var globalTermStore = TaxonomyTermStoreModelHandler.FindTermStore(site,
                           navigationModel.GlobalNavigationTermStoreName,
                           navigationModel.GlobalNavigationTermStoreId,
                           navigationModel.GlobalNavigationUseDefaultSiteCollectionTermStore);

                        var globalTermSet = TaxonomyFieldModelHandler.LookupTermSet(globalTermStore,
                          navigationModel.GlobalNavigationTermSetName,
                          navigationModel.GlobalNavigationTermSetId,
                          navigationModel.GlobalNavigationTermSetLCID);

                        thisWebNavSettings.GlobalNavigation.TermStoreId = globalTermStore.Id;
                        thisWebNavSettings.GlobalNavigation.TermSetId = globalTermSet.Id;
                    }
                    else
                    {
                        int? globalNavigationIncludeTypes = GetGlobalNavigationIncludeTypes(navigationModel);

                        if (globalNavigationIncludeTypes != null)
                        {
                            web.AllProperties[BuiltInWebPropertyId.GlobalNavigationIncludeTypes] = globalNavigationIncludeTypes;
                            shouldUpdateWeb = true;
                        }

                        if (navigationModel.GlobalNavigationMaximumNumberOfDynamicItems.HasValue)
                        {
                            web.AllProperties[BuiltInWebPropertyId.GlobalDynamicChildLimit] = navigationModel.GlobalNavigationMaximumNumberOfDynamicItems.Value;
                            shouldUpdateWeb = true;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(navigationModel.CurrentNavigationSource))
                {
                    var currentSource = (StandardNavigationSource)Enum.Parse(typeof(StandardNavigationSource), navigationModel.CurrentNavigationSource);

                    thisWebNavSettings.CurrentNavigation.Source = currentSource;

                    if (currentSource == StandardNavigationSource.TaxonomyProvider)
                    {
                        var currentTermStore = TaxonomyTermStoreModelHandler.FindTermStore(site,
                          navigationModel.CurrentNavigationTermStoreName,
                          navigationModel.CurrentNavigationTermStoreId,
                          navigationModel.CurrentNavigationUseDefaultSiteCollectionTermStore);

                        var currentTermSet = TaxonomyFieldModelHandler.LookupTermSet(currentTermStore,
                          navigationModel.CurrentNavigationTermSetName,
                          navigationModel.CurrentNavigationTermSetId,
                          navigationModel.CurrentNavigationTermSetLCID);

                        thisWebNavSettings.CurrentNavigation.TermStoreId = currentTermStore.Id;
                        thisWebNavSettings.CurrentNavigation.TermSetId = currentTermSet.Id;
                    }
                    else
                    {
                        int? currentNavigationIncludeTypes = GetCurrentNavigationIncludeTypes(navigationModel);

                        if (currentNavigationIncludeTypes != null)
                        {
                            web.AllProperties[BuiltInWebPropertyId.CurrentNavigationIncludeTypes] = currentNavigationIncludeTypes;
                            shouldUpdateWeb = true;
                        }

                        if (navigationModel.CurrentNavigationMaximumNumberOfDynamicItems.HasValue)
                        {
                            web.AllProperties[BuiltInWebPropertyId.CurrentDynamicChildLimit] = navigationModel.CurrentNavigationMaximumNumberOfDynamicItems.Value;
                            shouldUpdateWeb = true;
                        }
                    }
                }
            }

            if (navigationModel.DisplayShowHideRibbonAction.HasValue)
            {
                web.AllProperties["__DisplayShowHideRibbonActionId"] = navigationModel.DisplayShowHideRibbonAction.ToString();
                shouldUpdateWeb = true;
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

            if (shouldUpdateWeb)
                web.Update();

            if (!string.IsNullOrEmpty(navigationModel.GlobalNavigationSource) ||
                !string.IsNullOrEmpty(navigationModel.CurrentNavigationSource))
            {
                thisWebNavSettings.Update(null);
            }


        }

        protected int? GetGlobalNavigationIncludeTypes(WebNavigationSettingsDefinition navigationModel)
        {
            int? currentNavigationIncludeTypes = null;

            if (navigationModel.CurrentNavigationShowPages == false
                && navigationModel.CurrentNavigationShowSubsites == false)
                currentNavigationIncludeTypes = 0;
            else if (navigationModel.CurrentNavigationShowPages == true
                && navigationModel.CurrentNavigationShowSubsites == true)
                currentNavigationIncludeTypes = 3;
            else if (navigationModel.CurrentNavigationShowPages == true)
                currentNavigationIncludeTypes = 2;
            else if (navigationModel.CurrentNavigationShowSubsites == true)
                currentNavigationIncludeTypes = 1;

            return currentNavigationIncludeTypes;
        }

        protected int? GetCurrentNavigationIncludeTypes(WebNavigationSettingsDefinition navigationModel)
        {
            int? globalNavigationIncludeTypes = null;

            if (navigationModel.GlobalNavigationShowPages == false
                && navigationModel.GlobalNavigationShowSubsites == false)
                globalNavigationIncludeTypes = 0;
            else if (navigationModel.GlobalNavigationShowPages == true
                && navigationModel.GlobalNavigationShowSubsites == true)
                globalNavigationIncludeTypes = 3;
            else if (navigationModel.GlobalNavigationShowPages == true)
                globalNavigationIncludeTypes = 2;
            else if (navigationModel.GlobalNavigationShowSubsites == true)
                globalNavigationIncludeTypes = 1;

            return globalNavigationIncludeTypes;
        }


        #endregion
    }
}
