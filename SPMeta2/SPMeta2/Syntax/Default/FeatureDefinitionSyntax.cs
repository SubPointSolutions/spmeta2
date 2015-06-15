using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class DefinitionExtensions
    {
        public static TDefinition Inherit<TDefinition>(this DefinitionBase definition)
            where TDefinition : DefinitionBase, new()
        {
            return Inherit<TDefinition>(definition, null);
        }

        public static TDefinition Inherit<TDefinition>(this DefinitionBase definition, Action<TDefinition> config)
            where TDefinition : DefinitionBase, new()
        {
            var model = definition.Clone() as TDefinition;

            if (config != null)
                config(model);

            return model;
        }
    }

    public static class FeatureDefinitionSyntax
    {
        #region methods

        public static ModelNode AddFeature(this ModelNode model, FeatureDefinition definition)
        {
            return AddFeature(model, definition, null);
        }

        public static ModelNode AddFeature(this ModelNode model, FeatureDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #region array overload

        public static ModelNode AddFeatures(this ModelNode model, IEnumerable<FeatureDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        public static ModelNode AddSiteFeature(this ModelNode model, FeatureDefinition definition)
        {
            return AddSiteFeature(model, definition, null);
        }

        public static ModelNode AddSiteFeature(this ModelNode model, FeatureDefinition definition, Action<ModelNode> action)
        {
            return AddFeature(model, definition, action);
        }


        #region array overload

        public static ModelNode AddSiteFeature(this ModelNode model, IEnumerable<FeatureDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        public static ModelNode AddWebFeature(this ModelNode model, FeatureDefinition definition)
        {
            return AddWebFeature(model, definition, null);
        }

        public static ModelNode AddWebFeature(this ModelNode model, FeatureDefinition definition, Action<ModelNode> action)
        {
            return AddFeature(model, definition, action);
        }

        #region array overload

        public static ModelNode AddWebFeature(this ModelNode model, IEnumerable<FeatureDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        public static ModelNode AddWebApplicationFeature(this ModelNode model, FeatureDefinition definition)
        {
            return AddWebApplicationFeature(model, definition, null);
        }

        public static ModelNode AddWebApplicationFeature(this ModelNode model, FeatureDefinition definition, Action<ModelNode> action)
        {
            return AddFeature(model, definition, action);
        }

        #region array overload

        public static ModelNode AddWebApplicationFeatures(this ModelNode model, IEnumerable<FeatureDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        public static ModelNode AddFarmFeature(this ModelNode model, FeatureDefinition definition)
        {
            return AddFarmFeature(model, definition, null);
        }

        public static ModelNode AddFarmFeature(this ModelNode model, FeatureDefinition definition, Action<ModelNode> action)
        {
            return AddFeature(model, definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddFarmFeatures(this ModelNode model, IEnumerable<FeatureDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
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

        public static FeatureDefinition Disable(this FeatureDefinition definition)
        {
            definition.Enable = false;

            return definition;
        }

        public static FeatureDefinition ForceActivate(this FeatureDefinition definition)
        {
            definition.ForceActivate = true;

            return definition;
        }
    }
}
