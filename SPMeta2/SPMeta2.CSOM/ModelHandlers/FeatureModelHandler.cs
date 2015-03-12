using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.ModelHandlers;
using SPMeta2.Models;
using SPMeta2.Services;
using SPMeta2.Utils;
using FeatureDefinitionScope = SPMeta2.Definitions.FeatureDefinitionScope;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class FeatureModelHandler : CSOMModelHandlerBase
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
                    {
                        TraceService.Error((int)LogEventId.ModelProvisionCoreCall, "Farm features are not supported with CSOM. Throwing SPMeta2NotSupportedException.");
                        throw new SPMeta2NotSupportedException("Farm features are not supported with CSOM.");
                    }

                case FeatureDefinitionScope.WebApplication:
                    {
                        TraceService.Error((int)LogEventId.ModelProvisionCoreCall, "Web application features are not supported with CSOM. Throwing SPMeta2NotSupportedException.");
                        throw new SPMeta2NotSupportedException("Web application features are not supported with CSOM.");
                    }

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
            var context = web.Context;

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Deploying web feature.");

            // a bit unclear why it should be Farm scope
            // seems to be a scope to find feature definition in so that for sandbox solutions it would be Site?
            // http://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.featurecollection.add%28v=office.15%29.aspx
            ProcessFeature(modelHost, context, web.Features, featureModel, Microsoft.SharePoint.Client.FeatureDefinitionScope.Farm);
        }

        private void DeploySiteFeature(object modelHost, FeatureDefinition featureModel)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;
            var context = site.Context;

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Deploying site feature.");

            // a bit unclear why it should be Farm scope
            // seems to be a scope to find feature definition in so that for sandbox solutions it would be Site?
            ProcessFeature(modelHost, context, site.Features, featureModel, Microsoft.SharePoint.Client.FeatureDefinitionScope.Farm);
        }

        protected bool IsFeatureActivated(Feature feature)
        {
            return feature != null && feature.ServerObjectIsNull == false;
        }

        private void ProcessFeature(
                    object modelHost,
                    ClientRuntimeContext context,
                    FeatureCollection features,
                    FeatureDefinition featureModel,
                    Microsoft.SharePoint.Client.FeatureDefinitionScope scope)
        {
            var featureId = featureModel.Id;

            var currentFeature = features.GetById(featureId);
            features.Context.ExecuteQueryWithTrace();

            var featureActivated = IsFeatureActivated(currentFeature);

            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Is feature activated: [{0}]", featureActivated);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentFeature,
                ObjectType = typeof(Feature),
                ObjectDefinition = featureModel,
                ModelHost = modelHost
            });

            if (!featureActivated)
            {
                Feature tmpFeature;

                if (featureModel.Enable)
                {
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Enabling feature");
                    tmpFeature = SafelyActivateWebFeature(context, features, featureModel, scope);
                }
                else
                {
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Fetching feature by ID");
                    tmpFeature = features.GetById(featureId);

                    features.Context.ExecuteQueryWithTrace();
                }

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = tmpFeature,
                    ObjectType = typeof(Feature),
                    ObjectDefinition = featureModel,
                    ModelHost = modelHost
                });
            }
            else
            {
                if (featureModel.Enable && featureModel.ForceActivate)
                {
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Feature enabled, but ForceActivate = true. Force activating.");

                    var f = SafelyActivateWebFeature(context, features, featureModel, scope);

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = f,
                        ObjectType = typeof(Feature),
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
                        ObjectType = typeof(Feature),
                        ObjectDefinition = featureModel,
                        ModelHost = modelHost
                    });

                    context.ExecuteQueryWithTrace();
                }
                else
                {
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Fetching feature by ID");
                    var tmpFeature = features.GetById(featureId);

                    features.Context.ExecuteQueryWithTrace();

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = tmpFeature,
                        ObjectType = typeof(Feature),
                        ObjectDefinition = featureModel,
                        ModelHost = modelHost
                    });
                }
            }


        }

        private static Feature SafelyActivateWebFeature(ClientRuntimeContext context, FeatureCollection features,
            FeatureDefinition featureModel, Microsoft.SharePoint.Client.FeatureDefinitionScope scope)
        {
            var result = features.Add(featureModel.Id, featureModel.ForceActivate, scope);

            try
            {
                context.ExecuteQueryWithTrace();
            }
            catch (Exception e)
            {
                // sandbox site/web features?
                // they need to ne activated with SPFeatureDefinitionScope.Site scope
                if ((featureModel.Scope == FeatureDefinitionScope.Site || featureModel.Scope == FeatureDefinitionScope.Web)
                    && e.Message.ToUpper().Contains(featureModel.Id.ToString("D").ToUpper()))
                {
                    result = features.Add(featureModel.Id, featureModel.ForceActivate, Microsoft.SharePoint.Client.FeatureDefinitionScope.Site);
                    context.ExecuteQueryWithTrace();
                }
                else
                {
                    throw e;
                }
            }

            return result;
        }

        #endregion

        #region static

        protected static bool IsValidHost(object modelHost)
        {
            return modelHost is SiteModelHost ||
                   modelHost is WebModelHost;
        }


        #endregion
    }
}
