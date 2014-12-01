using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class TaxonomyTermGroupDefinitionSyntax
    {
        [Obsolete("Use AddTaxonomyTermGroup() instead.")]
        public static ModelNode AddTermGroup(this ModelNode model, TaxonomyTermGroupDefinition definition)
        {
            return AddTermGroup(model, definition, null);
        }

        [Obsolete("Use AddTaxonomyTermGroup() instead.")]
        public static ModelNode AddTermGroup(this ModelNode model, TaxonomyTermGroupDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        public static ModelNode AddTaxonomyTermGroup(this ModelNode model, TaxonomyTermGroupDefinition definition)
        {
            return AddTaxonomyTermGroup(model, definition, null);
        }

        public static ModelNode AddTaxonomyTermGroup(this ModelNode model, TaxonomyTermGroupDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        public static ModelNode AddHostTaxonomyTermGroup(this ModelNode model, TaxonomyTermGroupDefinition definition)
        {
            return AddHostTaxonomyTermGroup(model, definition, null);
        }

        public static ModelNode AddHostTaxonomyTermGroup(this ModelNode model, TaxonomyTermGroupDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }
    }
}
