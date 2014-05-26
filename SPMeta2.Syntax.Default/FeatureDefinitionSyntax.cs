using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class FeatureDefinitionSyntax
    {
        #region methods

        public static ModelNode AddFeature(this ModelNode model, DefinitionBase featureDefinition)
        {
            model.ChildModels.Add(new ModelNode { Value = featureDefinition });

            return model;
        }

        public static ModelNode AddSiteFeature(this ModelNode siteModel, DefinitionBase featureDefinition)
        {
            return AddFeature(siteModel, featureDefinition);
        }

        public static ModelNode AddWebFeature(this ModelNode webModel, DefinitionBase featureDefinition)
        {
            return AddFeature(webModel, featureDefinition);
        }

        public static ModelNode AddWebApplicationFeature(this ModelNode webApplicationModel, DefinitionBase featureDefinition)
        {
            return AddFeature(webApplicationModel, featureDefinition);
        }

        public static ModelNode AddFarmFeature(this ModelNode farmModel, DefinitionBase featureDefinition)
        {
            return AddFeature(farmModel, featureDefinition);
        }

        #endregion
    }
}
