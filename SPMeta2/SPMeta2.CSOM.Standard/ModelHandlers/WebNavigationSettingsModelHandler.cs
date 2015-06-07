using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Publishing.Navigation;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers.Fields;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Services;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;
using SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy;
using Microsoft.SharePoint.Client.Taxonomy;

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
                        int? globalNavigationIncludeTypes = GetGlobalNavigationIncludeTypes(navigationModel);

                        if (globalNavigationIncludeTypes != null)
                        {
                            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Setting GlobalNavigationIncludeTypes to: [{0}]", globalNavigationIncludeTypes);
                            web.AllProperties[BuiltInWebPropertyId.GlobalNavigationIncludeTypes] = globalNavigationIncludeTypes;

                            shouldUpdateWeb = true;
                        }

                        if (navigationModel.GlobalNavigationMaximumNumberOfDynamicItems.HasValue)
                        {
                            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Setting GlobalDynamicChildLimit to: [{0}]", navigationModel.GlobalNavigationMaximumNumberOfDynamicItems.Value);
                            web.AllProperties[BuiltInWebPropertyId.GlobalDynamicChildLimit] = navigationModel.GlobalNavigationMaximumNumberOfDynamicItems.Value;

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
                        int? currentNavigationIncludeTypes = GetCurrentNavigationIncludeTypes(navigationModel);

                        if (currentNavigationIncludeTypes != null)
                        {
                            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Setting CurrentNavigationIncludeTypes to: [{0}]", currentNavigationIncludeTypes);
                            web.AllProperties[BuiltInWebPropertyId.CurrentNavigationIncludeTypes] = currentNavigationIncludeTypes;

                            shouldUpdateWeb = true;
                        }

                        if (navigationModel.CurrentNavigationMaximumNumberOfDynamicItems.HasValue)
                        {
                            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Setting CurrentDynamicChildLimit to: [{0}]", navigationModel.CurrentNavigationMaximumNumberOfDynamicItems.Value);
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

            if (!string.IsNullOrEmpty(navigationModel.GlobalNavigationSource) ||
                !string.IsNullOrEmpty(navigationModel.CurrentNavigationSource))
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Updating navigation settings");

                thisWebNavSettings.Update(null);
                context.ExecuteQueryWithTrace();
            }

            // update include types

            if (shouldUpdateWeb)
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Updating web");
                web.Update();
                context.ExecuteQueryWithTrace();
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
