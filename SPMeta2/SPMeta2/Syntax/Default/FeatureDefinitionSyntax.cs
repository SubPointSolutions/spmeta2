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

    public class FeatureModelNode : TypedModelNode
    {

    }

    public static class FeatureDefinitionSyntax
    {
        #region methods

        public static FarmModelNode AddFeature(this FarmModelNode model, FeatureDefinition definition)
        {
            return AddFeature(model, definition, null);
        }

        public static FarmModelNode AddFeature(this FarmModelNode model, FeatureDefinition definition, Action<FeatureModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static WebApplicationModelNode AddFeature(this WebApplicationModelNode model, FeatureDefinition definition)
        {
            return AddFeature(model, definition, null);
        }

        public static WebApplicationModelNode AddFeature(this WebApplicationModelNode model, FeatureDefinition definition, Action<FeatureModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static SiteModelNode AddFeature(this SiteModelNode model, FeatureDefinition definition)
        {
            return AddFeature(model, definition, null);
        }

        public static SiteModelNode AddFeature(this SiteModelNode model, FeatureDefinition definition, Action<FeatureModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static WebModelNode AddFeature(this WebModelNode model, FeatureDefinition definition)
        {
            return AddFeature(model, definition, null);
        }

        public static WebModelNode AddFeature(this WebModelNode model, FeatureDefinition definition, Action<FeatureModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #region array overload

        public static ModelNode AddFeatures(this ModelNode model, IEnumerable<FeatureDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        public static SiteModelNode AddSiteFeature(this SiteModelNode model, FeatureDefinition definition)
        {
            return AddSiteFeature(model, definition, null);
        }

        public static SiteModelNode AddSiteFeature(this SiteModelNode model, FeatureDefinition definition, Action<FeatureModelNode> action)
        {
            return AddFeature(model, definition, action) as SiteModelNode;
        }


        #region array overload

        public static SiteModelNode AddSiteFeature(this SiteModelNode model, IEnumerable<FeatureDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        public static WebModelNode AddWebFeature(this WebModelNode model, FeatureDefinition definition)
        {
            return AddWebFeature(model, definition, null);
        }

        public static WebModelNode AddWebFeature(this WebModelNode model, FeatureDefinition definition, Action<FeatureModelNode> action)
        {
            return AddFeature(model, definition, action) as WebModelNode;
        }

        #region array overload

        public static WebModelNode AddWebFeature(this WebModelNode model, IEnumerable<FeatureDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        public static WebApplicationModelNode AddWebApplicationFeature(this WebApplicationModelNode model, FeatureDefinition definition)
        {
            return AddWebApplicationFeature(model, definition, null);
        }

        public static WebApplicationModelNode AddWebApplicationFeature(this WebApplicationModelNode model, FeatureDefinition definition, Action<FeatureModelNode> action)
        {
            return AddFeature(model, definition, action) as WebApplicationModelNode;
        }

        #region array overload

        public static WebApplicationModelNode AddWebApplicationFeatures(this WebApplicationModelNode model, IEnumerable<FeatureDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        public static FarmModelNode AddFarmFeature(this FarmModelNode model, FeatureDefinition definition)
        {
            return AddFarmFeature(model, definition, null);
        }

        public static FarmModelNode AddFarmFeature(this FarmModelNode model, FeatureDefinition definition, Action<FeatureModelNode> action)
        {
            return AddFeature(model, definition, action) as FarmModelNode;
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
