using System;
using Microsoft.SharePoint.Client.Publishing.Navigation;
using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers.Fields;
using SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Services;
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
            context.ExecuteQueryWithTrace();

            return thisWebNavSettings;
        }

        private void DeployNavigationSettings(object modelHost, WebModelHost webModelHost, WebNavigationSettingsDefinition navigationModel)
        {
            var shouldUpdateWeb = false;

            var site = webModelHost.HostSite;
            var web = webModelHost.HostWeb;

            var context = web.Context;
            var allProperties = web.AllProperties;

            context.Load(allProperties);

            // the GetWebNavigationSettings will call ExecuteQueryWithTrace
            var thisWebNavSettings = GetWebNavigationSettings(webModelHost, navigationModel);

            StandardNavigationSource? globalSource = null;
            StandardNavigationSource? currentSource = null;

            if (!string.IsNullOrEmpty(navigationModel.GlobalNavigationSource))
                globalSource = (StandardNavigationSource)Enum.Parse(typeof(StandardNavigationSource), navigationModel.GlobalNavigationSource);

            if (!string.IsNullOrEmpty(navigationModel.CurrentNavigationSource))
                currentSource = (StandardNavigationSource)Enum.Parse(typeof(StandardNavigationSource), navigationModel.CurrentNavigationSource);

            TermStore currentTermStore = null;
            TermSet currentTermSet = null;

            if (currentSource == StandardNavigationSource.TaxonomyProvider)
            {
                currentTermStore = TaxonomyTermStoreModelHandler.FindTermStore(site,
                           navigationModel.CurrentNavigationTermStoreName,
                           navigationModel.CurrentNavigationTermStoreId,
                           navigationModel.CurrentNavigationUseDefaultSiteCollectionTermStore);

                currentTermSet = TaxonomyFieldModelHandler.LookupTermSet(currentTermStore.Context,
                  currentTermStore,
                  site,
                  null,
                  null,
                  null,
                  navigationModel.CurrentNavigationTermSetName,
                  navigationModel.CurrentNavigationTermSetId,
                  navigationModel.CurrentNavigationTermSetLCID);
            }

            TermStore globalTermStore = null;
            TermSet globalTermSet = null;

            if (globalSource == StandardNavigationSource.TaxonomyProvider)
            {
                globalTermStore = TaxonomyTermStoreModelHandler.FindTermStore(site,
                          navigationModel.GlobalNavigationTermStoreName,
                          navigationModel.GlobalNavigationTermStoreId,
                          navigationModel.GlobalNavigationUseDefaultSiteCollectionTermStore);

                globalTermSet = TaxonomyFieldModelHandler.LookupTermSet(site.Context,
                  globalTermStore,
                  site,
                  null,
                  null,
                  null,
                  navigationModel.GlobalNavigationTermSetName,
                  navigationModel.GlobalNavigationTermSetId,
                  navigationModel.GlobalNavigationTermSetLCID);
            }

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
                if (globalSource.HasValue)
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Setting GlobalNavigation to: [{0}]", navigationModel.GlobalNavigationSource);

                    thisWebNavSettings.GlobalNavigation.Source = globalSource.Value;

                    if (globalTermStore != null)
                    {
                        thisWebNavSettings.GlobalNavigation.TermStoreId = globalTermStore.Id;
                        thisWebNavSettings.GlobalNavigation.TermSetId = globalTermSet.Id;
                    }
                    else
                    {
                        var value = allProperties.FieldValues.ContainsKey(BuiltInWebPropertyId.GlobalNavigationIncludeTypes)
                                        ? allProperties[BuiltInWebPropertyId.GlobalNavigationIncludeTypes]
                                        : string.Empty;

                        int? globalNavigationIncludeTypes = GetGlobalNavigationIncludeTypes(
                                    navigationModel,
                                    ConvertUtils.ToInt(value));

                        if (globalNavigationIncludeTypes != null)
                        {
                            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Setting GlobalNavigationIncludeTypes to: [{0}]", globalNavigationIncludeTypes);
                            allProperties[BuiltInWebPropertyId.GlobalNavigationIncludeTypes] = globalNavigationIncludeTypes.Value;

                            shouldUpdateWeb = true;
                        }

                        if (navigationModel.GlobalNavigationMaximumNumberOfDynamicItems.HasValue)
                        {
                            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Setting GlobalDynamicChildLimit to: [{0}]", navigationModel.GlobalNavigationMaximumNumberOfDynamicItems.Value);
                            allProperties[BuiltInWebPropertyId.GlobalDynamicChildLimit] = navigationModel.GlobalNavigationMaximumNumberOfDynamicItems.Value;

                            shouldUpdateWeb = true;
                        }
                    }
                }

                if (currentSource.HasValue)
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall,
                        "Setting CurrentNavigation to: [{0}]", navigationModel.CurrentNavigationSource);

                    thisWebNavSettings.CurrentNavigation.Source = currentSource.Value;

                    if (currentTermStore != null)
                    {
                        thisWebNavSettings.CurrentNavigation.TermStoreId = currentTermStore.Id;
                        thisWebNavSettings.CurrentNavigation.TermSetId = currentTermSet.Id;
                    }
                    else
                    {
                        var value = allProperties.FieldValues.ContainsKey(BuiltInWebPropertyId.CurrentNavigationIncludeTypes)
                                        ? allProperties[BuiltInWebPropertyId.CurrentNavigationIncludeTypes]
                                        : string.Empty;


                        int? currentNavigationIncludeTypes = GetCurrentNavigationIncludeTypes(
                            navigationModel,
                            ConvertUtils.ToInt(value));

                        if (currentNavigationIncludeTypes != null)
                        {
                            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Setting CurrentNavigationIncludeTypes to: [{0}]", currentNavigationIncludeTypes);
                            allProperties[BuiltInWebPropertyId.CurrentNavigationIncludeTypes] = currentNavigationIncludeTypes.Value;

                            shouldUpdateWeb = true;
                        }

                        if (navigationModel.CurrentNavigationMaximumNumberOfDynamicItems.HasValue)
                        {
                            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Setting CurrentDynamicChildLimit to: [{0}]", navigationModel.CurrentNavigationMaximumNumberOfDynamicItems.Value);
                            allProperties[BuiltInWebPropertyId.CurrentDynamicChildLimit] = navigationModel.CurrentNavigationMaximumNumberOfDynamicItems.Value;

                            shouldUpdateWeb = true;
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

            if (!string.IsNullOrEmpty(navigationModel.GlobalNavigationSource) ||
                !string.IsNullOrEmpty(navigationModel.CurrentNavigationSource))
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Updating navigation settings");

                web.Update();
                thisWebNavSettings.Update(null);
                shouldUpdateWeb = true;
            }

            if (shouldUpdateWeb)
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Updating web");
                web.Update();
                context.ExecuteQueryWithTrace();
            }
        }

        protected int? GetGlobalNavigationIncludeTypes(
            WebNavigationSettingsDefinition navigationModel,
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

            return navigationIncludeTypes;
        }

        protected int? GetCurrentNavigationIncludeTypes(
            WebNavigationSettingsDefinition navigationModel,
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

            return navigationIncludeTypes;
        }

        #endregion
    }
}
