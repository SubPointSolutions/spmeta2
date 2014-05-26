using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;
using Microsoft.SharePoint.Administration;

namespace SPMeta2.SSOM.ModelHandlers
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
            var web = modelHost.WithAssertAndCast<SPWeb>("modelHost", value => value.RequireNotNull());
            ProcessFeature(web.Features, featureModel);
        }

        private void DeploySiteFeature(object modelHost, FeatureDefinition featureModel)
        {
            var site = modelHost.WithAssertAndCast<SPSite>("modelHost", value => value.RequireNotNull());
            ProcessFeature(site.Features, featureModel);
        }

        private void DeployWebApplicationFeature(object modelHost, FeatureDefinition featureModel)
        {
            var webApplication = modelHost.WithAssertAndCast<SPWebApplication>("modelHost", value => value.RequireNotNull());
            ProcessFeature(webApplication.Features, featureModel);
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
                   modelHost is SPSite ||
                   modelHost is SPWeb;
        }

        private static void ProcessFeature(SPFeatureCollection features, FeatureDefinition featureModel)
        {
            var featureActivated = features.FirstOrDefault(f => f.DefinitionId == featureModel.Id) != null;

            if (!featureActivated)
            {
                if (featureModel.Enable)
                {
                    features.Add(featureModel.Id, featureModel.ForceActivate);
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
                    features.Add(featureModel.Id, featureModel.ForceActivate);
                }

                if (!featureModel.Enable)
                {
                    features.Remove(featureModel.Id, featureModel.ForceActivate);
                }
            }
        }

        #endregion

        #endregion
    }
}
