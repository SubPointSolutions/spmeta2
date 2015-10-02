using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default.Extensions;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class TaxonomyTermLabelModelNode : TypedModelNode
    {

    }

    public static class TaxonomyTermLabelDefinitionSyntax
    {
        #region methods

        public static TModelNode AddTaxonomyTermLabel<TModelNode>(this TModelNode model, TaxonomyTermLabelDefinition definition)
            where TModelNode : ModelNode, ITaxonomyTermModelNode, new()
        {
            return AddTaxonomyTermLabel(model, definition, null);
        }

        public static TModelNode AddTaxonomyTermLabel<TModelNode>(this TModelNode model, TaxonomyTermLabelDefinition definition,
            Action<TaxonomyTermLabelModelNode> action)
            where TModelNode : ModelNode, ITaxonomyTermModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddTaxonomyTermLabels<TModelNode>(this TModelNode model, IEnumerable<TaxonomyTermLabelDefinition> definitions)
           where TModelNode : ModelNode, ITaxonomyTermModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
