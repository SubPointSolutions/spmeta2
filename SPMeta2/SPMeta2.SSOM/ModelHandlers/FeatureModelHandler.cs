using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
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

            ProcessFeature(modelHost, web.Features, featureModel);
        }

        private void DeploySiteFeature(object modelHost, FeatureDefinition featureModel)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var site = siteModelHost.HostSite;

            ProcessFeature(modelHost, site.Features, featureModel);
        }

        private void DeployWebApplicationFeature(object modelHost, FeatureDefinition featureModel)
        {
            var webApplication = modelHost.WithAssertAndCast<SPWebApplication>("modelHost", value => value.RequireNotNull());
            ProcessFeature(modelHost, webApplication.Features, featureModel);
        }

        private void DeployFarmFeature(object modelHost, FeatureDefinition featureModel)
        {
            throw new NotImplementedException("SPFarm feature activation has not implemented yet");

            //var farm = modelHost.WithAssertAndCast<SPFarm>("modelHost", value => value.RequireNotNull());
            //ProcessFeature(farm.FeatureDefinitions, featureModel);
        }

        #region utils

        private static bool IsValidHost(object modelHost)
        {
            return modelHost is SPFarm ||
                   modelHost is SPWebApplication ||
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

            if (!featureActivated)
            {
                if (featureModel.Enable)
                {
                    var f = features.Add(featureModel.Id, featureModel.ForceActivate);

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
                    var f = features.Add(featureModel.Id, featureModel.ForceActivate);

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

        #endregion

        #endregion
    }
}
