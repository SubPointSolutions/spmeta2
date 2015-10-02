using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class TaxonomyTermModelNode : TypedModelNode,
        ITaxonomyTermHostModelNode, ITaxonomyTermModelNode
    {

    }

    public static class TaxonomyTermDefinitionSyntax
    {
        #region methods

        public static TModelNode AddTaxonomyTerm<TModelNode>(this TModelNode model, TaxonomyTermDefinition definition)
            where TModelNode : ModelNode, ITaxonomyTermHostModelNode, new()
        {
            return AddTaxonomyTerm(model, definition, null);
        }

        public static TModelNode AddTaxonomyTerm<TModelNode>(this TModelNode model, TaxonomyTermDefinition definition,
            Action<TaxonomyTermModelNode> action)
            where TModelNode : ModelNode, ITaxonomyTermHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddTaxonomyTerms<TModelNode>(this TModelNode model, IEnumerable<TaxonomyTermDefinition> definitions)
           where TModelNode : ModelNode, ITaxonomyTermHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        public static TModelNode AddHostTaxonomyTerm<TModelNode>(this TModelNode model, TaxonomyTermDefinition definition)
         where TModelNode : ModelNode, ITaxonomyTermHostModelNode, new()
        {
            return AddHostTaxonomyTerm(model, definition, null);
        }
        public static TModelNode AddHostTaxonomyTerm<TModelNode>(this TModelNode model, TaxonomyTermDefinition definition,
            Action<TaxonomyTermModelNode> action)
            where TModelNode : ModelNode, ITaxonomyTermHostModelNode, new()
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }
    }
}
