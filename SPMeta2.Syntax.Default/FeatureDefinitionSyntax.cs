using System;
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
            return AddFeature(model, featureDefinition, null);
        }

        public static ModelNode AddFeature(this ModelNode model, DefinitionBase featureDefinition, Action<ModelNode> action)
        {
            var newModelNode = new ModelNode { Value = featureDefinition };

            model.ChildModels.Add(newModelNode);

            if (action != null)
                action(newModelNode);

            return model;
        }

        public static ModelNode AddSiteFeature(this ModelNode siteModel, DefinitionBase featureDefinition)
        {
            return AddSiteFeature(siteModel, featureDefinition, null);
        }

        public static ModelNode AddSiteFeature(this ModelNode siteModel, DefinitionBase featureDefinition, Action<ModelNode> action)
        {
            return AddFeature(siteModel, featureDefinition, action);
        }

        public static ModelNode AddWebFeature(this ModelNode webModel, DefinitionBase featureDefinition)
        {
            return AddWebFeature(webModel, featureDefinition, null);
        }

        public static ModelNode AddWebFeature(this ModelNode webModel, DefinitionBase featureDefinition, Action<ModelNode> action)
        {
            return AddFeature(webModel, featureDefinition, action);
        }

        public static ModelNode AddWebApplicationFeature(this ModelNode webApplicationModel,
            DefinitionBase featureDefinition)
        {
            return AddWebApplicationFeature(webApplicationModel, featureDefinition, null);
        }

        public static ModelNode AddWebApplicationFeature(this ModelNode webApplicationModel, DefinitionBase featureDefinition, Action<ModelNode> action)
        {
            return AddFeature(webApplicationModel, featureDefinition, action);
        }

        public static ModelNode AddFarmFeature(this ModelNode webApplicationModel,
           DefinitionBase featureDefinition)
        {
            return AddFarmFeature(webApplicationModel, featureDefinition, null);
        }

        public static ModelNode AddFarmFeature(this ModelNode farmModel, DefinitionBase featureDefinition, Action<ModelNode> action)
        {
            return AddFeature(farmModel, featureDefinition, action);
        }

        #endregion

        public static FeatureDefinition Inherit(this FeatureDefinition definition)
        {
            return Inherit(definition, null);
        }

        public static FeatureDefinition Inherit(this FeatureDefinition definition, Action<FeatureDefinition> config)
        {
            var model = definition.Clone() as FeatureDefinition;

            if (config != null)
                config(model);

            return model;
        }

        public static FeatureDefinition Enable(this FeatureDefinition definition)
        {
            definition.Enable = true;

            return definition;
        }

        public static FeatureDefinition ForceActivate(this FeatureDefinition definition)
        {
            definition.ForceActivate = true;

            return definition;
        }
    }
}
