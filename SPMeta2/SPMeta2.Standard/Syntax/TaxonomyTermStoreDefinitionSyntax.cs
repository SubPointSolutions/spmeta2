using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class TaxonomyTermStoreDefinitionSyntax
    {
        [Obsolete("Use AddTaxonomyTermStore() instead.")]
        public static ModelNode AddTermStore(this ModelNode model, TaxonomyTermStoreDefinition definition)
        {
            return AddTermStore(model, definition, null);
        }

        [Obsolete("Use AddTaxonomyTermStore() instead.")]
        public static ModelNode AddTermStore(this ModelNode model, TaxonomyTermStoreDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        public static ModelNode AddTaxonomyTermStore(this ModelNode model, TaxonomyTermStoreDefinition definition)
        {
            return AddTaxonomyTermStore(model, definition, null);
        }

        public static ModelNode AddTaxonomyTermStore(this ModelNode model, TaxonomyTermStoreDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        public static ModelNode AddHostTaxonomyTermStore(this ModelNode model, TaxonomyTermStoreDefinition definition)
        {
            return AddHostTaxonomyTermStore(model, definition, null);
        }

        public static ModelNode AddHostTaxonomyTermStore(this ModelNode model, TaxonomyTermStoreDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }
    }
}
