using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public class TaxonomyTermStoreModelNode : TypedModelNode,
        ITaxonomyTermGroupHostModelNode
    {

    }

    public static class TaxonomyTermStoreDefinitionSyntax
    {
        #region methods

        public static TModelNode AddTaxonomyTermStore<TModelNode>(this TModelNode model, TaxonomyTermStoreDefinition definition)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return AddTaxonomyTermStore(model, definition, null);
        }

        public static TModelNode AddTaxonomyTermStore<TModelNode>(this TModelNode model, TaxonomyTermStoreDefinition definition,
            Action<TaxonomyTermStoreModelNode> action)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion


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
