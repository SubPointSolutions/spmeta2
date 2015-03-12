using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using Microsoft.SharePoint.Administration;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class FeatureModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(FeatureDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var featureModel = model.WithAssertAndCast<FeatureDefinition>("model", value => value.RequireNotNull());
            if (!IsValidHost(modelHost))
                throw new Exception("model host needs to be SPFarm, SPWebApplication, SPSit or SPWeb instance");

            DeployFeatureInternal(modelHost, featureModel);
        }

        private void DeployFeatureInternal(object modelHost, FeatureDefinition featureModel)
        {
            switch (featureModel.Scope)
            {
                case FeatureDefinitionScope.Farm:
                    DeployFarmFeature(modelHost, featureModel);
                    break;

                case FeatureDefinitionScope.WebApplication:
                    DeployWebApplicationFeature(modelHost, featureModel);
                    break;

                case FeatureDefinitionScope.Site:
                    DeploySiteFeature(modelHost, featureModel);
                    break;

                case FeatureDefinitionScope.Web:
                    DeployWebFeature(modelHost, featureModel);
                    break;
            }
        }

        private void DeployWebFeature(object modelHost, FeatureDefinition featureModel)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var web = webModelHost.HostWeb;

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Deploying web feature.");

            ProcessFeature(modelHost, web.Features, featureModel);
        }

        private void DeploySiteFeature(object modelHost, FeatureDefinition featureModel)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var site = siteModelHost.HostSite;

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Deploying site feature.");

            ProcessFeature(modelHost, site.Features, featureModel);
        }

        private void DeployWebApplicationFeature(object modelHost, FeatureDefinition featureModel)
        {
            var webApplicationModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var webApplication = webApplicationModelHost.HostWebApplication;

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Deploying web application feature.");

            ProcessFeature(modelHost, webApplication.Features, featureModel);
        }

        private void DeployFarmFeature(object modelHost, FeatureDefinition featureModel)
        {
            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Deploying farm feature.");

            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var farm = farmModelHost.HostFarm;

            var adminService = SPWebService.AdministrationService;
            ProcessFeature(modelHost, adminService.Features, featureModel);
        }

        #region utils

        private static bool IsValidHost(object modelHost)
        {
            return modelHost is SPFarm || modelHost is FarmModelHost ||
                   modelHost is SPWebApplication || modelHost is WebApplicationModelHost ||
                   modelHost is SiteModelHost ||
                   modelHost is WebModelHost;
        }

        protected SPFeature GetFeature(SPFeatureCollection features, FeatureDefinition featureModel)
        {
            return features.FirstOrDefault(f => f.DefinitionId == featureModel.Id);
        }

        protected bool IsFeatureActivated(SPFeature currentFeature)
        {
            return currentFeature != null;
        }

        private void ProcessFeature(
            object modelHost,
            SPFeatureCollection features, FeatureDefinition featureModel)
        {
            var currentFeature = GetFeature(features, featureModel);
            var featureActivated = IsFeatureActivated(currentFeature);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentFeature,
                ObjectType = typeof(SPFeature),
                ObjectDefinition = featureModel,
                ModelHost = modelHost
            });

            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Is feature activated: [{0}]", featureActivated);

            if (!featureActivated)
            {
                if (featureModel.Enable)
                {
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Enabling feature");
                    var f = SafelyActivateFeature(features, featureModel);

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = f,
                        ObjectType = typeof(SPFeature),
                        ObjectDefinition = featureModel,
                        ModelHost = modelHost
                    });
                }
                else
                {
                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = currentFeature,
                        ObjectType = typeof(SPFeature),
                        ObjectDefinition = featureModel,
                        ModelHost = modelHost
                    });
                }
            }
            else
            {
                if (featureModel.Enable && featureModel.ForceActivate)
                {
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Feature enabled, but ForceActivate = true. Force activating.");

                    var f = SafelyActivateFeature(features, featureModel);

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = f,
                        ObjectType = typeof(SPFeature),
                        ObjectDefinition = featureModel,
                        ModelHost = modelHost
                    });
                }
                else if (!featureModel.Enable)
                {
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Removing feature.");

                    features.Remove(featureModel.Id, featureModel.ForceActivate);

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = null,
                        ObjectType = typeof(SPFeature),
                        ObjectDefinition = featureModel,
                        ModelHost = modelHost
                    });
                }
                else
                {
                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = currentFeature,
                        ObjectType = typeof(SPFeature),
                        ObjectDefinition = featureModel,
                        ModelHost = modelHost
                    });
                }
            }
        }

        private static SPFeature SafelyActivateFeature(SPFeatureCollection features, FeatureDefinition featureModel)
        {
            SPFeature result = null;

            try
            {
                result = features.Add(featureModel.Id, featureModel.ForceActivate);
            }
            catch (Exception e)
            {
                // sandbox site/web features?
                // they need to ne activated with SPFeatureDefinitionScope.Site scope
                if ((featureModel.Scope == FeatureDefinitionScope.Site || featureModel.Scope == FeatureDefinitionScope.Web)
                    && e.Message.ToUpper().Contains(featureModel.Id.ToString("D").ToUpper()))
                {
                    result = features.Add(featureModel.Id, featureModel.ForceActivate, SPFeatureDefinitionScope.Site);
                }
                else
                {
                    throw e;
                }
            }

            return result;
        }

        #endregion

        #endregion
    }
}
