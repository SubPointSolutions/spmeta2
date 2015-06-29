using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public class TaxonomyTermStoreModelNode : TypedModelNode
    {

    }

    public static class TaxonomyTermStoreDefinitionSyntax
    {
        public static SiteModelNode AddTaxonomyTermStore(this SiteModelNode model, TaxonomyTermStoreDefinition definition)
        {
            return AddTaxonomyTermStore(model, definition, null);
        }

        public static SiteModelNode AddTaxonomyTermStore(this SiteModelNode model, TaxonomyTermStoreDefinition definition,
            Action<TaxonomyTermStoreModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static ModelNode AddHostTaxonomyTermStore(this ModelNode model, TaxonomyTermStoreDefinition definition)
        {
            return AddHostTaxonomyTermStore(model, definition, null);
        }

        public static ModelNode AddHostTaxonomyTermStore(this ModelNode model, TaxonomyTermStoreDefinition definition,
            Action<TaxonomyTermStoreModelNode> action)
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }
    }
}
