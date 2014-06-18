using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.Regression.SSOM.Utils;
using SPMeta2.Utils;
using FeatureDefinitionScope = SPMeta2.Definitions.FeatureDefinitionScope;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientFeatureDefinitionValidator : FeatureModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var featureModel = model.WithAssertAndCast<FeatureDefinition>("model", value => value.RequireNotNull());

            if (!IsValidHost(modelHost))
                throw new Exception("model host needs to be SPFarm, SPWebApplication, SPSit or SPWeb instance");

            ValidateFeatureModel(modelHost, featureModel);
        }

        private void ValidateFeatureModel(object modelHost, FeatureDefinition featureModel)
        {
            switch (featureModel.Scope)
            {
                case FeatureDefinitionScope.Farm:
                    throw new NotSupportedException("Farm features are not supported with CSOM.");

                case FeatureDefinitionScope.WebApplication:
                    throw new NotSupportedException("Web application features are not supported with CSOM.");

                case FeatureDefinitionScope.Site:
                    ValidateSiteFeature(modelHost, featureModel);
                    break;

                case FeatureDefinitionScope.Web:
                    ValidateWebFeature(modelHost, featureModel);
                    break;
            }
        }

        private void ValidateWebFeature(object modelHost, FeatureDefinition featureModel)
        {
            var host = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var features = LoadWebFeatures(host);

            var currentFeature = GetFeature(features, featureModel);

            Trace(featureModel, currentFeature);
        }

        private void ValidateSiteFeature(object modelHost, FeatureDefinition featureModel)
        {
            var host = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var features = LoadSiteFeatures(host);

            var currentFeature = GetFeature(features, featureModel);

            Trace(featureModel, currentFeature);
        }

        private void Trace(FeatureDefinition featureModel, Feature currentFeature)
        {
            var featureActivated = currentFeature != null;

            TraceUtils.WithScope(traceScope =>
            {
                traceScope.WriteLine(string.Format("Validating model:[{0}] feature:[{1}]", featureModel, currentFeature));

                traceScope.WriteLine(string.Format("Model prop [{0}] on obj prop [{1}]: model:[{2}] obj:[{3}]",
                 "Enable",
                 "Enable",
                 featureModel.Enable,
                 featureActivated));

                Assert.AreEqual(featureModel.Enable, featureActivated);
            });
        }
    }
}
