using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class TaxonomyTermSetDefinitionSyntax
    {
        [Obsolete("Use AddTaxonomyTermSet() instead.")]
        public static ModelNode AddTermSet(this ModelNode model, TaxonomyTermSetDefinition definition)
        {
            return AddTermSet(model, definition, null);
        }

        [Obsolete("Use AddTaxonomyTermSet() instead.")]
        public static ModelNode AddTermSet(this ModelNode model, TaxonomyTermSetDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        public static ModelNode AddTaxonomyTermSet(this ModelNode model, TaxonomyTermSetDefinition definition)
        {
            return AddTaxonomyTermSet(model, definition, null);
        }

        public static ModelNode AddTaxonomyTermSet(this ModelNode model, TaxonomyTermSetDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        public static ModelNode AddHostTaxonomyTermSet(this ModelNode model, TaxonomyTermSetDefinition definition)
        {
            return AddHostTaxonomyTermSet(model, definition, null);
        }

        public static ModelNode AddHostTaxonomyTermSet(this ModelNode model, TaxonomyTermSetDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }
    }
}
