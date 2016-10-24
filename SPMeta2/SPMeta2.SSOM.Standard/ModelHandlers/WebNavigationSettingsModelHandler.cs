using System;
using Microsoft.SharePoint.Publishing.Navigation;

using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers.Fields;
using SPMeta2.SSOM.Standard.ModelHandlers.Taxonomy;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

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
            var allProperties = web.AllProperties;

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
                          null, null, null, null,
                          navigationModel.GlobalNavigationTermSetName,
                          navigationModel.GlobalNavigationTermSetId,
                          navigationModel.GlobalNavigationTermSetLCID);

                        thisWebNavSettings.GlobalNavigation.TermStoreId = globalTermStore.Id;
                        thisWebNavSettings.GlobalNavigation.TermSetId = globalTermSet.Id;
                    }
                    else
                    {
                        int? globalNavigationIncludeTypes = GetGlobalNavigationIncludeTypes(navigationModel,
                          ConvertUtils.ToInt(allProperties[BuiltInWebPropertyId.GlobalNavigationIncludeTypes]));

                        if (globalNavigationIncludeTypes != null)
                        {
                            allProperties[BuiltInWebPropertyId.GlobalNavigationIncludeTypes] = globalNavigationIncludeTypes.Value;
                            shouldUpdateWeb = true;
                        }

                        if (navigationModel.GlobalNavigationMaximumNumberOfDynamicItems.HasValue)
                        {
                            allProperties[BuiltInWebPropertyId.GlobalDynamicChildLimit] = navigationModel.GlobalNavigationMaximumNumberOfDynamicItems.Value;
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
                          null, null, null, null,
                          navigationModel.CurrentNavigationTermSetName,
                          navigationModel.CurrentNavigationTermSetId,
                          navigationModel.CurrentNavigationTermSetLCID);

                        thisWebNavSettings.CurrentNavigation.TermStoreId = currentTermStore.Id;
                        thisWebNavSettings.CurrentNavigation.TermSetId = currentTermSet.Id;
                    }
                    else
                    {
                        int? currentNavigationIncludeTypes = GetCurrentNavigationIncludeTypes(navigationModel,
                           ConvertUtils.ToInt(allProperties[BuiltInWebPropertyId.CurrentNavigationIncludeTypes]));

                        if (currentNavigationIncludeTypes != null)
                        {
                            allProperties[BuiltInWebPropertyId.CurrentNavigationIncludeTypes] = currentNavigationIncludeTypes.Value;
                            shouldUpdateWeb = true;
                        }

                        if (navigationModel.CurrentNavigationMaximumNumberOfDynamicItems.HasValue)
                        {
                            allProperties[BuiltInWebPropertyId.CurrentDynamicChildLimit] = navigationModel.CurrentNavigationMaximumNumberOfDynamicItems.Value;
                            shouldUpdateWeb = true;
                        }
                    }
                }
            }

            if (navigationModel.AddNewPagesToNavigation.HasValue)
            {
                thisWebNavSettings.AddNewPagesToNavigation = navigationModel.AddNewPagesToNavigation.Value;
            }

            if (navigationModel.CreateFriendlyUrlsForNewPages.HasValue)
            {
                thisWebNavSettings.CreateFriendlyUrlsForNewPages = navigationModel.CreateFriendlyUrlsForNewPages.Value;
            }

            if (navigationModel.DisplayShowHideRibbonAction.HasValue)
            {
                allProperties[BuiltInWebPropertyId.DisplayShowHideRibbonActionId] = navigationModel.DisplayShowHideRibbonAction.ToString();
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

        protected int? GetGlobalNavigationIncludeTypes(WebNavigationSettingsDefinition navigationModel,
            int? navigationIncludeTypes)
        {
            if (navigationModel.GlobalNavigationShowPages.HasValue
                || navigationModel.GlobalNavigationShowSubsites.HasValue)
            {
                if (!navigationIncludeTypes.HasValue)
                    navigationIncludeTypes = 2;
            }

            if (navigationModel.GlobalNavigationShowPages.HasValue)
            {
                if (navigationModel.GlobalNavigationShowPages.Value)
                {
                    navigationIncludeTypes |= 2;
                }
                else
                {
                    navigationIncludeTypes &= ~2;
                }
            }

            if (navigationModel.GlobalNavigationShowSubsites.HasValue)
            {
                if (navigationModel.GlobalNavigationShowSubsites.Value)
                {
                    navigationIncludeTypes |= 1;
                }
                else
                {
                    navigationIncludeTypes &= ~1;
                }
            }

            //if (navigationModel.CurrentNavigationShowPages == false
            //    && navigationModel.CurrentNavigationShowSubsites == false)__GlobalNavigationIncludeTypes
            //    currentNavigationIncludeTypes = 0;
            //else if (navigationModel.CurrentNavigationShowPages == true
            //    && navigationModel.CurrentNavigationShowSubsites == true)
            //    currentNavigationIncludeTypes = 3;
            //else if (navigationModel.CurrentNavigationShowPages == true)
            //    currentNavigationIncludeTypes = 2;
            //else if (navigationModel.CurrentNavigationShowSubsites == true)
            //    currentNavigationIncludeTypes = 1;

            return navigationIncludeTypes;
        }

        protected int? GetCurrentNavigationIncludeTypes(WebNavigationSettingsDefinition navigationModel,
            int? navigationIncludeTypes)
        {
            if (navigationModel.CurrentNavigationShowPages.HasValue
               || navigationModel.CurrentNavigationShowSubsites.HasValue)
            {
                if (!navigationIncludeTypes.HasValue)
                    navigationIncludeTypes = 2;
            }

            if (navigationModel.CurrentNavigationShowPages.HasValue)
            {
                if (navigationModel.CurrentNavigationShowPages.Value)
                {
                    navigationIncludeTypes |= 2;
                }
                else
                {
                    navigationIncludeTypes &= ~2;
                }
            }

            if (navigationModel.CurrentNavigationShowSubsites.HasValue)
            {
                if (navigationModel.CurrentNavigationShowSubsites.Value)
                {
                    navigationIncludeTypes |= 1;
                }
                else
                {
                    navigationIncludeTypes &= ~1;
                }
            }

            //if (navigationModel.CurrentNavigationShowPages == false
            //    && navigationModel.CurrentNavigationShowSubsites == false)__GlobalNavigationIncludeTypes
            //    currentNavigationIncludeTypes = 0;
            //else if (navigationModel.CurrentNavigationShowPages == true
            //    && navigationModel.CurrentNavigationShowSubsites == true)
            //    currentNavigationIncludeTypes = 3;
            //else if (navigationModel.CurrentNavigationShowPages == true)
            //    currentNavigationIncludeTypes = 2;
            //else if (navigationModel.CurrentNavigationShowSubsites == true)
            //    currentNavigationIncludeTypes = 1;

            return navigationIncludeTypes;
        }

        #endregion
    }
}
