using System;
using System.Runtime.Serialization;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
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

        public static TModelNode AddHostTaxonomyTermStore<TModelNode>(this TModelNode model, TaxonomyTermStoreDefinition definition)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return AddHostTaxonomyTermStore(model, definition, null);
        }
        public static TModelNode AddHostTaxonomyTermStore<TModelNode>(this TModelNode model, TaxonomyTermStoreDefinition definition,
            Action<TaxonomyTermStoreModelNode> action)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }
    }
}
