using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Models;
using SPMeta2.Utils;
using FeatureDefinitionScope = SPMeta2.Definitions.FeatureDefinitionScope;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class FeatureModelHandler : ModelHandlerBase
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
                    throw new NotSupportedException("Farm features are not supported with CSOM.");

                case FeatureDefinitionScope.WebApplication:
                    throw new NotSupportedException("Web application features are not supported with CSOM.");

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

            context.Load(web, w => w.Features);
            context.ExecuteQuery();

            // a bit unclear why it should be Farm scope
            // seems to be a scope to find feature definition in so that for sandbox solutions it would be Site?
            // http://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.featurecollection.add%28v=office.15%29.aspx
            ProcessFeature(context, web.Features, featureModel, Microsoft.SharePoint.Client.FeatureDefinitionScope.Farm);
        }

        private void DeploySiteFeature(object modelHost, FeatureDefinition featureModel)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;
            var context = site.Context;

            context.Load(site, w => w.Features);
            context.ExecuteQuery();

            // a bit unclear why it should be Farm scope
            // seems to be a scope to find feature definition in so that for sandbox solutions it would be Site?
            ProcessFeature(context, site.Features, featureModel, Microsoft.SharePoint.Client.FeatureDefinitionScope.Farm);
        }

        private void ProcessFeature(ClientRuntimeContext context,
                    FeatureCollection features,
                    FeatureDefinition featureModel,
                    Microsoft.SharePoint.Client.FeatureDefinitionScope scope)
        {
            var featureActivated = features.FirstOrDefault(f => f.DefinitionId == featureModel.Id) != null;

            if (!featureActivated)
            {
                if (featureModel.Enable)
                {
                    features.Add(featureModel.Id, featureModel.ForceActivate, scope);
                    context.ExecuteQuery();
                }
                else
                {
                    // TODO, warning trace
                }
            }
            else
            {
                if (featureModel.Enable && featureModel.ForceActivate)
                {
                    features.Add(featureModel.Id, featureModel.ForceActivate, scope);
                    context.ExecuteQuery();
                }

                if (!featureModel.Enable)
                {
                    features.Remove(featureModel.Id, featureModel.ForceActivate);
                    context.ExecuteQuery();
                }
            }


        }

        #endregion

        #region static

        private static bool IsValidHost(object modelHost)
        {
            return modelHost is SiteModelHost ||
                   modelHost is WebModelHost;
        }


        #endregion
    }
}
